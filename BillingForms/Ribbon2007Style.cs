using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Billing.Pattern.AbstractFactory;
using BillingForms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Helpers;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using NLog;


namespace BillingForms
{
    public partial class Ribbon2007Style : RibbonForm
    {
        public Ribbon2007Style()
        {
            InitializeComponent();
            logger = LogManager.GetCurrentClassLogger();
            InitSkinGallery();
            this.Text += @" " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadIniSets();
        }
        private Logger logger { get; set; }
        private List<IniSet> _iniSets { get; set; } 
        private string SqlCreateTable { get; set; }

        void InitSkinGallery()
        {
            logger.Info("InitSkinGallery");
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        void LoadIniSets(string path = "config")
        {
            _iniSets = new List<IniSet>();
            string[] files = Directory.GetFiles(path);
            // находим и заполняем список с конфиг файлами
            var fileList = (from file in files let extension = Path.GetExtension(file) where extension == ".config" select file).ToList();
            foreach (var file in fileList)
            {
                var set = new IniSet(file);
                set.LoadSettings();
                _iniSets.Add(set);
            }

            using (var reader = new StreamReader("SQL\\calls.sql"))
            {
                SqlCreateTable = reader.ReadToEnd();
            }
        }

        private void iNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            var form = new ImportFiles();
            logger.Info("Импорт из файлов");
            form.TopLevel = false;
            form.Parent = this;
            form.Show();
        }
       
        public void ImportRun(ImportFiles form)
        {
            label1.Text = @"Загрузка...";
            // нужно пройти по каталогу и подкаталогам и загрузить документы
            var directory = new DirectoryInfo(form.GetFolderRoot());
            var filePathList = new List<string>();
            Utils.WalkDirectoryTree(directory, form.GetMaskFiles(), ref filePathList);

            // Initializing progress bar properties
            // Количество файлов
            progressBarControl1.Properties.Step = 1;
            progressBarControl1.Properties.PercentView = true;
            progressBarControl1.Properties.Maximum = filePathList.Count;
            progressBarControl1.Properties.Minimum = 0;

            progressBarControl1.Visible = true;
            progressBarControl2.Visible = true;

            // проходим по списку найденных файлов и начинаем сравнивать с конфиг файлами
            var num = 1;
            var numdocumload = 0;
            foreach (var filepath in filePathList)
            {
                progressBarControl1.PerformStep();
                progressBarControl1.Update();
                foreach (var set in _iniSets)
                {
                    if (Path.GetExtension(filepath) != set.GetFileExt().Substring(1)) continue;
                    label1.Text = Path.GetFileName(filepath);
                    
                    // нужно проверить колонки данной xls
                    // включаем работу с xls
                    var product = new ExcelProduct(logger);
                    product.OpenExcelApp(filepath);
                    var columns = product.GetColumns(1);
                    product.MigateDate();
                    product.CloseExcelApp();
                    // начинаем сравнивать у данной xls колонки с настроенными колонками в конфиге
                    // если нашли подходящий конфиг -> создаем таблицу sqlite в памяти, согласно таблице в файле SQL\calls.sql
                    if (!columns.SequenceEqual(set.GetColumns().Keys)) continue;
                    var GSMform = new GSMEyeForm();
                    GSMform.Text = string.Format("billing{0}.db", num);
                    GSMform.MdiParent = this;
                    

                    #region TABLE DEFINITION

                    var dict = new Dictionary<string, string>()
                    {
                        // группа столбцов из данных (информативные столбцы)
                        {"call_date_time", "DATETIME"},
                        {"duration", "INT"},
                        {"direction", "TEXT"},
                        {"service", "TEXT"},
                        {"number_a", "TEXT"},
                        {"number_b", "TEXT"},
                        {"imsi_a", "TEXT"},
                        {"imsi_b", "TEXT"},
                        {"imei_a", "TEXT"},
                        {"imei_b", "TEXT"},
                        {"lac_cell_a", "TEXT"},
                        {"lac_cell_b", "TEXT"},
                        {"place_a", "TEXT"},
                        {"place_b", "TEXT"},
                        {"message", "TEXT"},
                        {"commutator_a", "TEXT"},
                        {"commutator_b", "TEXT"},
                        // группа служебных столбцов (описывают источник данных и т.п.)
                        {"id", "INT"},
                        {"file_type", "TEXT"},
                        {"file_name", "TEXT"},
                        {"raw_record", "TEXT"},
                        // группа  обогащенных  столбцов  (значения которых заполняются при обогащении)
                        {"number_a_data", "TEXT"},
                        {"number_b_data", "TEXT"},
                        {"imsi_a_data", "TEXT"},
                        {"imsi_b_data", "TEXT"},
                        {"imei_a_data", "TEXT"},
                        {"imei_b_data", "TEXT"},
                        {"commutator_a_data", "TEXT"},
                        {"commutator_b_data", "TEXT"}
                    };

                    #endregion

                    #region CREATE TABLE CALLS

                    // открываем команду по созданию таблицы calls
                    string sqlcommand;
                    using (var reader = new StreamReader("SQL\\calls.sql", Encoding.GetEncoding(1251)))
                    {
                        sqlcommand = reader.ReadToEnd();
                    }

                    var mDbConnection = new SQLiteConnection("Data Source=:memory:");
                    mDbConnection.Open();

                    // создаем таблицу в памяти
                    using (var cmd = new SQLiteCommand(sqlcommand))
                    {
                        cmd.Connection = mDbConnection;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(@"Ошибка выполнении запроса создание таблицы calls" + ex.Message);
                            siStatus.Caption = string.Format("Прервано в результате ошибки");
                            progressBarControl1.Visible = false;
                            progressBarControl2.Visible = false;
                            progressBarControl1.Reset();
                            progressBarControl2.Reset();
                            return;
                        }
                        finally
                        {
                            cmd.Dispose();
                        }
                    }

                    #endregion

                    #region CREATE PARAMETERS


                    string commandText = dict.Aggregate(@"INSERT INTO calls (", (current, d) => current + (d.Key + ","));
                    commandText = commandText.Substring(0, commandText.Length - 1) + ")";
                    commandText += " VALUES(";
                    commandText = dict.Aggregate(commandText, (current, d) => current + ("@" + d.Key + ","));
                    commandText = commandText.Substring(0, commandText.Length - 1) + ")";

                    // создаем параметры
                    var iParam = 0;
                    var command = new SQLiteCommand(commandText, mDbConnection);
                    SQLiteParameter param;
                    foreach (var d in dict)
                    {
                        if (d.Value == "DATETIME")
                        {
                            param = new SQLiteParameter("@" + d.Key, DbType.DateTime);
                        }
                        else if (d.Value == "INT")
                        {
                            param = new SQLiteParameter("@" + d.Key, DbType.Int32);
                        }
                        else
                        {
                            param = new SQLiteParameter("@" + d.Key, DbType.String);
                        }
                        //param.Value = view.GetRowCellValue(i, set.Value)
                        command.Parameters.Add(param);
                        iParam++;
                    }
                    param = new SQLiteParameter("@id", DbType.Int32);
                    command.Parameters.Add(param);
                    param = new SQLiteParameter("@file_type", DbType.String);
                    command.Parameters.Add(param);
                    param = new SQLiteParameter("@file_name", DbType.String);
                    command.Parameters.Add(param);

                    #endregion

                    #region FILL DATA

                    // начинаем заливать данные
                    // Initializing progress bar properties
                    progressBarControl2.Properties.Step = 1;
                    progressBarControl2.Properties.PercentView = true;
                    progressBarControl2.Properties.Maximum = product.GetTablesFromExcel()[0].GetLength(0)
                        * product.GetTablesFromExcel()[0].GetLength(1);
                    progressBarControl2.Properties.Minimum = 0;

                    for (int i = 0; i < product.GetTablesFromExcel()[0].GetLength(0); i++)
                    {
                        for (int j = 0; j < product.GetTablesFromExcel()[0].GetLength(1); j++)
                        {
                            try
                            {
                                progressBarControl2.PerformStep();
                                progressBarControl2.Update();

                                var p = set.GetColumns().Values.ElementAt(j);
                                if (p == "")
                                {
                                    continue;
                                }
                                if (p == "call_date_time")
                                {
                                    var d = DateTime.FromOADate(double.Parse(product.GetTablesFromExcel()[0][i, j]));
                                    command.Parameters["@" + p].Value = d;
                                }
                                else
                                {
                                    command.Parameters["@" + p].Value = product.GetTablesFromExcel()[0][i, j];
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message + "\n" + @"Возможно конфигурационный файл не соответствует требованиям");
                                siStatus.Caption = string.Format("Прервано в результате ошибки");
                                progressBarControl1.Visible = false;
                                progressBarControl2.Visible = false;
                                progressBarControl1.Reset();
                                progressBarControl2.Reset();
                                return;
                            }

                        }
                        command.Parameters["@id"].Value = i + 1;
                        command.Parameters["@file_type"].Value = set.GetFileType();
                        command.Parameters["@file_name"].Value = filepath;
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            siStatus.Caption = string.Format("Прервано в результате ошибки");
                            progressBarControl1.Reset();
                            progressBarControl2.Reset();
                            progressBarControl1.Visible = false;
                            progressBarControl2.Visible = false;
                            return;
                        }
                    }
                    progressBarControl2.Reset();

                    #endregion

                    #region DATA CORRECTION

                    try
                    {
                        using (var reader = new StreamReader(set.GetSqlPath(), Encoding.GetEncoding(1251)))
                        {
                            sqlcommand = reader.ReadToEnd();
                        }

                        using (var cmd = new SQLiteCommand(sqlcommand))
                        {
                            cmd.Connection = mDbConnection;

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                var dialogResult = MessageBox.Show(string.Format(@"Произошла ошибка при выполнении коррекционного скрипта: {0}", set.GetSqlPath()), @"Ошибка", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.No)
                                {
                                    siStatus.Caption = string.Format("Прервано в результате ошибки");
                                    progressBarControl1.Visible = false;
                                    progressBarControl2.Visible = false;
                                    return;
                                }
                            }
                            finally
                            {
                                cmd.Dispose();
                            }
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        var dialogResult = MessageBox.Show(string.Format("Коррекционный файл: {0} не доступен, продолжить?", set.GetSqlPath()), "Ошибка", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            siStatus.Caption = string.Format("Прервано пользователем");
                            progressBarControl1.Visible = false;
                            progressBarControl2.Visible = false;
                            return;
                        }
                    }

                    #endregion

                    #region SQLITE TO GRIDVIEW

                    var config = new ConfigProduct();
                    var dataSet = new DataSet();
                    try
                    {
                        var dataAdapter = new SQLiteDataAdapter("select * from calls", mDbConnection);//(, "Data Source=:memory:");
                        dataAdapter.Fill(dataSet);
                        dataAdapter.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, @"Не могу импортировать данные",
                            MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    try
                    {
                        config.LoadSettings("config\\columns.txt");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Application.Exit();
                    }

                    try
                    {
                        foreach (var s in config.GetSettings())
                        {
                            if (dataSet.Tables[0].Columns.Contains(s.Key))
                            {
                                dataSet.Tables[0].Columns[s.Key].ColumnName = s.Value;
                            }
                        }
                        GSMform.gridControl1.DataSource = dataSet.Tables[0];
                    }
                    catch (DuplicateNameException ex)
                    {
                        MessageBox.Show(ex.Message, "Дублирование столбцов!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }

                    #endregion
                        
                    num++;
                    numdocumload++;
                    GSMform.labelControl1.Text = GSMform.GetGridView().DataRowCount.ToString();
                    GSMform.Show();
                }
            }
            label1.Text = string.Format("Загружен(о) {0} документ(а)", numdocumload);
            progressBarControl1.Reset();
            progressBarControl2.Reset();
            progressBarControl1.Visible = false;
            progressBarControl2.Visible = false;
        }

        private void iOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            logger.Info("Открыть документ");
            var form = new GSMEyeForm();
            if (form.FileShowDialog() == DialogResult.OK)
            {
                form.MdiParent = this;
                form.Show();
            }
        }

        private void iSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (documentManager1.View.Documents.Count == 0)
            {
                return;
            }
            logger.Info("Сохранить документ");
            splashScreenManager2.ShowWaitForm();
            var eye = documentManager1.View.ActiveDocument.Form as GSMEyeForm;
            if (eye != null)
            {
                string currentfile = eye.Text;
                if (File.Exists(currentfile))
                {
                    logger.Info("Файл существует - перезаписываем");
                    // просто перезаписываем
                    logger.Info("Grid to sqlite: " + currentfile);
                    Utils.GridView2Sqlite(eye.GetGridView(), currentfile);

                    logger.Info("Sqlite " + currentfile + " to gridwiew");
                    Utils.Sqlite2GridView(ref eye.gridControl1, currentfile);
                }
                else
                {
                    logger.Info("Файл не существует - SaveFileDialog");
                    var saveFileDialog1 = new SaveFileDialog();

                    //saveFileDialog1.Filter = "sqlite files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        logger.Info("saveFileDialog1.ShowDialog() == DialogResult.OK");
                        // просто перезаписываем
                        logger.Info("Grid to sqlite: " + saveFileDialog1.FileName);
                        Utils.GridView2Sqlite(eye.GetGridView(), saveFileDialog1.FileName);
                        logger.Info("Sqlite " + saveFileDialog1.FileName + " to gridwiew");
                        Utils.Sqlite2GridView(ref eye.gridControl1, saveFileDialog1.FileName);
                    }
                }
            }
            splashScreenManager2.CloseWaitForm();
        }

        private void iClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            logger.Info("Закрыть активный документ");
            if (documentManager1.View.ActiveDocument != null)
                documentManager1.View.ActiveDocument.Form.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            logger.Info("Application.Exit()");
            Application.Exit();
        }

        private void UnionSetsButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            logger.Info("Объединение документов");
            var index = 0;
            splashScreenManager2.ShowWaitForm();
            var docs = new List<string>();
            //1 сохранение в памяти всех активынх документов
            logger.Info("Сохраняем все активнвые документы в temp\\");
            foreach (var document in documentManager1.View.Documents)
            {
                var eye = document.Form as GSMEyeForm;
                var baseName = string.Format(@"temp\base_{0}.sqlite", index);
                logger.Info(baseName);
                index++;
                if (eye != null) Utils.GridView2Sqlite(eye.GetGridView(), baseName);
                docs.Add(baseName);
            }
            //2,3 ATTACH сохраненных баз (активных документов) и вывод в грид
            ImportData(docs, "union all");

            splashScreenManager2.CloseWaitForm();
        }

        private void ImportData(IEnumerable<string> fileLoc, string query)
        {
            //const string baseName = "base.sqlite";
            var tables = new List<string>();
            string sql = string.Empty;
            logger.Info("Создаем в памяти Connection sqlite");
            var mDbConnection = new SQLiteConnection("Data Source=:memory:");
            mDbConnection.Open();
            logger.Info("Connection.Open()");
            logger.Info("foreach по всем активным документам");
            foreach (var filepath in fileLoc.Select((x, i) => new { Value = x, Index = i }))
            {
                sql = "ATTACH '" + filepath.Value + "' AS TOMERGE_" + filepath.Index;
                logger.Info(sql);
                tables.Add("TOMERGE_" + filepath.Index);
                var cmd = new SQLiteCommand(sql) { Connection = mDbConnection };
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    MessageBox.Show(@"An error occurred, your import was not completed.");
                }
                finally
                {
                    cmd.Dispose();
                }
            }

            string SQL = "create table calls as ";
            foreach (var table in tables.Select((x, i) => new { Value = x, Index = i }))
            {
                if (table.Index == 0)
                {
                    SQL += string.Format("select * from {0}.calls ", table.Value);
                }
                else
                {
                    SQL += string.Format("{0} select * from {1}.calls",query, table.Value);
                }
            }
            logger.Info(SQL);
            //string SQL = "create table cal as " +
            //      "select * from TOMERGE.calls";
            var cmd_ = new SQLiteCommand(SQL);
            cmd_.Connection = mDbConnection;
            try
            {
                cmd_.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred, your import was not completed. " + ex.Message);
            }
            finally
            {
                cmd_.Dispose();
            }

            const string baseName = @"temp\mainbase.sqlite";
            logger.Info("CreateFile " + baseName);
            SQLiteConnection.CreateFile(baseName);
            var source = new SQLiteConnection("Data Source=" + baseName);
            source.Open();logger.Info("SaveDataBase in memory");
            // save memory db to file
            mDbConnection.BackupDatabase(source, "main", "main", -1, null, 0);
            source.Close();
            mDbConnection.Close();

            var form = new GSMEyeForm();
            form.Text = @"непонялкакназвать.sqlite";
            documentManager1.View.AddDocument(form);
            logger.Info("sqlite " + baseName + " to GridView");
            if (form.gridControl1 != null) Utils.Sqlite2GridView(ref form.gridControl1, baseName);
            form.labelControl1.Text = form.GetGridView().DataRowCount.ToString();
        }

        private void IntersectButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            logger.Info("Пересечение документов");
            int index = 0;
            splashScreenManager2.ShowWaitForm();
            var docs = new List<string>();
            //1 сохранение в памяти всех активынх документов
            logger.Info("Сохраняем все активнвые документы в temp\\");
            foreach (var document in documentManager1.View.Documents)
            {
                var eye = document.Form as GSMEyeForm;
                string baseName = string.Format(@"temp\base_{0}.sqlite", index);
                logger.Info(baseName);
                index++;
                if (eye != null) Utils.GridView2Sqlite(eye.GetGridView(), baseName);
                docs.Add(baseName);
            }
            //2,3 ATTACH сохраненных баз (активных документов) и вывод в грид
            ImportData(docs, "intersect");

            splashScreenManager2.CloseWaitForm();
        }

        private void iSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (documentManager1.View.Documents.Count == 0)
            {
                return;
            }
            logger.Info("Сохранить как");
            splashScreenManager2.ShowWaitForm();
            var eye = documentManager1.View.ActiveDocument.Form as GSMEyeForm;
            var saveFileDialog1 = new SaveFileDialog();

            //saveFileDialog1.Filter = "sqlite files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                logger.Info("saveFileDialog1.ShowDialog() == DialogResult.OK");
                logger.Info("Grid to sqlite: " + saveFileDialog1.FileName);
                Utils.GridView2Sqlite(eye.GetGridView(), saveFileDialog1.FileName);
                logger.Info("Sqlite " + saveFileDialog1.FileName + " to gridwiew");
                Utils.Sqlite2GridView(ref eye.gridControl1, saveFileDialog1.FileName);
            }
            splashScreenManager2.CloseWaitForm();
        }

        private void NewDocumentsThread_DoWork(object sender, DoWorkEventArgs e)
        {
            }

    }
}
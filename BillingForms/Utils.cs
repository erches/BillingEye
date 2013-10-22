
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Billing.Pattern.AbstractFactory;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace BillingForms
{
    public static class Utils
    {
        public static void GridView2Sqlite(GridView view, string baseName)
        {
            //const string baseName = "base.sqlite";
            const string tableName = "calls";
            SQLiteConnection.CreateFile(baseName);
            var parameters = new List<string>();

            var mDbConnection = new SQLiteConnection("Data Source=" + baseName);
            mDbConnection.Open();

            string columns = string.Empty;
            var config = new ConfigProduct();
            try
            {
                config.LoadSettings(@"config\columns.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            columns = config.GetSettings().Aggregate(columns, (current, set) => current + (set.Key + ","));

            string sql = "create table " + tableName + "(" + columns.Substring(0, columns.Length - 1) + ")";
            var command = new SQLiteCommand(sql, mDbConnection);
            command.ExecuteNonQuery();

            string commandText = config.GetSettings().Aggregate("INSERT INTO " + tableName + " (", (current, set) => current + (set.Key + ","));
            commandText = commandText.Substring(0, commandText.Length - 1) + ")";
            commandText += " VALUES(";
            foreach (var set in config.GetSettings())
            {
                commandText += "@" + set.Key + ",";
                parameters.Add("@" + set.Key);
            }
            commandText = commandText.Substring(0, commandText.Length - 1) + ")";

            command = new SQLiteCommand(commandText, mDbConnection);

            for (int i = 0; i < view.RowCount; i++)
            {
                int iParam = 0;
                foreach (var set in config.GetSettings())
                {
                    var param = new SQLiteParameter(parameters[iParam], DbType.String);
                    param.Value = view.GetRowCellValue(i, set.Value);
                    command.Parameters.Add(param);
                    iParam++;
                }
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                command.Parameters.Clear();
            }
            command.Dispose();
            mDbConnection.Close();
        }

        public static void Sqlite2GridView(ref GridControl control, string dataSource)
        {
            var config = new ConfigProduct();
            string commandString = String.Empty;
            try
            {
                commandString = config.LoadSqlQuery("config\\query.sql");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            var dataSet = new DataSet();
            try
            {
                var dataAdapter = new SQLiteDataAdapter(commandString,
                                                                       string.Format("data source={0}", dataSource));
                dataAdapter.Fill(dataSet);
                dataAdapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Не могу подключиться к БД",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            try
            {
                config.LoadSettings(@"config\columns.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            foreach (var set in config.GetSettings())
            {
                if (dataSet.Tables[0].Columns.Contains(set.Key))
                {
                    dataSet.Tables[0].Columns[set.Key].ColumnName = set.Value;
                }
            }
            control.DataSource = dataSet.Tables[0];
        }

        /// <summary>
        /// создать новый sqlite в памяти, применить к нему sql_file_name, поместить все в grid view 
        /// </summary>
        public static void NewSqlite2GridView(ref GridControl control, IniSet set, List<string[,]> table, string FileName)
        {
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
            for (int i = 0; i < table[0].GetLength(0); i++)
            {
                for (int j = 0; j < table[0].GetLength(1); j++)
                {
                    try
                    { 
                        var p = set.GetColumns().Values.ElementAt(j);
                        if (p == "call_date_time")
                        {
                            var d = DateTime.FromOADate(double.Parse(table[0][i, j]));
                            //var _provider = new CultureInfo(1);
                            // var _d2 = new DateTime();
                            //_d2 = DateTime.ParseExact(table[0][i, j], "dd.MM.yyyy HH:mm", _provider);
                            command.Parameters["@" + p].Value = d;
                        }
                        else
                        {
                            command.Parameters["@" + p].Value = table[0][i, j];
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                   
                }
                command.Parameters["@id"].Value = i + 1;
                command.Parameters["@file_type"].Value = set.GetFileType();
                command.Parameters["@file_name"].Value = FileName;
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

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
                        if (dialogResult == DialogResult.No) return;
                    }
                    finally
                    {
                        cmd.Dispose();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                var dialogResult = MessageBox.Show(string.Format("Коррекционный файл: {0} не доступен, продолжить?", set.GetSqlPath()),"Ошибка" ,MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No) return;
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
                control.DataSource = dataSet.Tables[0];
            }
            catch (DuplicateNameException ex)
            {
                MessageBox.Show(ex.Message, "Дублирование столбцов!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            
            #endregion

        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            var idx = 0;
            foreach (T item in enumerable)
                handler(item, idx++);
        }

        public static void WalkDirectoryTree(System.IO.DirectoryInfo root, string mask, ref List<string> filePathList)
        {
            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;
    
            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles(mask);
                filePathList.AddRange(files.Select(file => file.FullName));
            }
                // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                //log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    //Console.WriteLine(fi.FullName);
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, mask, ref filePathList);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public static void Excel2Grid(ref GridControl control)
        {
            
        }
    }
}

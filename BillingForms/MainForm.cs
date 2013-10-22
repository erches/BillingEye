using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Billing.Pattern.AbstractFactory;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace BillingForms
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Новый документ
        /// </summary>
        private void newDocumentButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var form = new GSMEyeForm();
            //form.MdiParent = this;
            //form.Name = "New";
            xrTabbedMdiManager1.View.AddDocument(form);
        }

        /// <summary>
        /// Открыть sqlite
        /// </summary>
        private void openDocumentButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var form = new GSMEyeForm();
            if (form.FileShowDialog() == DialogResult.OK)
            {
                xrTabbedMdiManager1.View.AddDocument(form);
            }
        }

        /// <summary>
        /// Выход из программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// закрыть текущий активный документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeVisibleDocument_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(xrTabbedMdiManager1.View.ActiveDocument != null)
                xrTabbedMdiManager1.View.ActiveDocument.Form.Close();
        }

        /// <summary>
        /// объединение активных документов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int index = 0;
            splashScreenManager1.ShowWaitForm();
            var docs = new List<string>();
            //1 сохранение в памяти всех активынх документов
            foreach (var document in xrTabbedMdiManager1.View.Documents)
            {
                var eye = document.Form as GSMEyeForm;
                string baseName = string.Format(@"temp\base_{0}.sqlite", index);
                index++;
                if (eye != null) Utils.GridView2Sqlite(eye.GetGridView(), baseName);
                docs.Add(baseName);
            }
            //2,3 ATTACH сохраненных баз (активных документов) и вывод в грид
            ImportData(docs);
            
            splashScreenManager1.CloseWaitForm();
        }

        private void ImportData(IEnumerable<string> fileLoc)
        {
            //const string baseName = "base.sqlite";
            var tables = new List<string>();
            string sql = string.Empty;
            var mDbConnection = new SQLiteConnection("Data Source=:memory:");
            mDbConnection.Open();
            foreach (var filepath in fileLoc.Select((x,i) => new { Value = x, Index=i }))
            {
                sql = "ATTACH '" + filepath.Value + "' AS TOMERGE_" + filepath.Index;
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
            foreach (var table in tables.Select((x,i) => new { Value = x, Index=i }))
            {
                if (table.Index == 0)
                {
                    SQL += string.Format("select * from {0}.calls ", table.Value);
                }
                else
                {
                    SQL += string.Format("union all select * from {0}.calls", table.Value);
                }
            }
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

            const string baseName = @"temp\mainbase.slqite";
            SQLiteConnection.CreateFile(baseName);
            var source = new SQLiteConnection("Data Source=" + baseName);
            source.Open();

            // save memory db to file
            mDbConnection.BackupDatabase(source, "main", "main", -1, null, 0);
            source.Close();
            mDbConnection.Close();

            var form = new GSMEyeForm();
            form.Text = @"непонялкакназвать";
            xrTabbedMdiManager1.View.AddDocument(form);
            if (form.gridControl1 != null) Utils.Sqlite2GridView(ref form.gridControl1, baseName);
            form.labelControl1.Text = form.GetGridView().DataRowCount.ToString();
        }

        /// <summary>
        /// кнопка сохранить документ. 
        /// Для начала нужно проверить, есть ли такой документ в системе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveDocumentButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xrTabbedMdiManager1.View.Documents.Count == 0)
            {
                return;
            }
            var eye = xrTabbedMdiManager1.View.ActiveDocument.Form as GSMEyeForm;
            string currentfile = eye.Text;
            if (File.Exists(currentfile))
            {
                // просто перезаписываем
                Utils.GridView2Sqlite(eye.GetGridView(), currentfile);
                Utils.Sqlite2GridView(ref eye.gridControl1, currentfile);
            }
        }
    }
}
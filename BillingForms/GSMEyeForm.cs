using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Billing.Pattern.AbstractFactory;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace BillingForms
{
    public partial class GSMEyeForm : DevExpress.XtraEditors.XtraForm
    {
        public GSMEyeForm()
        {
            InitializeComponent();

            gridView1.PopupMenuShowing += new PopupMenuShowingEventHandler(gridView1_PopupMenuShowing);
            gridView1.ShowGridMenu += new GridMenuEventHandler(this.gridView1_ShowGridMenu1);

            // добавим GroupSummary
            //GridGroupSummaryItem item = new GridGroupSummaryItem();
            //item.FieldName = "Count Discinct";
            //item.SummaryType = SummaryItemType.Custom;
            //item.DisplayFormat = "(Discinct items = {0})";
            //item.Tag = 2;
            //gridView1.GroupSummary.Add(item);
        }

        private SQLiteDataAdapter _dataAdapter;
        private SQLiteConnection _sqLiteConnection;
        private SQLiteCommand _command;

        /// <summary>
        /// Нажали кнопку открыть файл БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public DialogResult FileShowDialog()
        {
            openFileDialog1.FileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1_FileOk();
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        public GridView GetGridView()
        {
            return gridView1;
        }

        public GridControl GetGridControl()
        {
            return gridControl1;
        }
 
        /// <summary>
        /// Открыли файл в OpenFileDialog
        /// </summary>
        private void openFileDialog1_FileOk()
        {
            this.Text = openFileDialog1.FileName;
            gridControl1.DataSource = null;
            Utils.Sqlite2GridView(ref gridControl1, openFileDialog1.FileName);
            splashScreenManager1.ShowWaitForm();
            var col = new GridColumn()
                {
                    FieldName = "selected",
                    Name = "selected"
                };

            gridView1.Columns.Add(col);

            gridView1.Columns["selected"].Visible = false;
            labelControl1.Text = gridView1.DataRowCount.ToString();

            gridView1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            gridView1.OptionsMenu.EnableGroupPanelMenu = true;
            gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
            gridView1.OptionsMenu.EnableGroupPanelMenu = true;

            gridView1.ExpandAllGroups();
            gridView1.BestFitColumns();
            splashScreenManager1.CloseWaitForm();
        }

        /// <summary>
        /// показать контекстное меню после нажатия правой кнопкой мыши по строкам
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_ShowGridMenu1(object sender, GridMenuEventArgs e)
        {
            var view = sender as GridView;
            GridHitInfo hitInfo = view.CalcHitInfo(e.Point);
            if (hitInfo.InRow)
            {
                view.FocusedRowHandle = hitInfo.RowHandle;
                contextMenuStrip1.Show(view.GridControl, e.Point);
            }
        }

        /// <summary>
        /// агрегирующая функция под столбцом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != GridMenuType.Summary) return;

            DevExpress.XtraGrid.Menu.GridViewFooterMenu footerMenu = e.Menu as DevExpress.XtraGrid.Menu.GridViewFooterMenu;
            bool check = e.HitInfo.Column.SummaryItem.SummaryType == SummaryItemType.Custom && Equals("Count", e.HitInfo.Column.SummaryItem.Tag);
            DevExpress.Utils.Menu.DXMenuItem menuItem = new DevExpress.Utils.Menu.DXMenuCheckItem("Число уникальных", check, null, new EventHandler(MyMenuItem));
            menuItem.Tag = e.HitInfo.Column;
            foreach (DevExpress.Utils.Menu.DXMenuItem item in footerMenu.Items)
                item.Enabled = true;
            footerMenu.Items.Add(menuItem);
        }

        private void MyMenuItem(Object sender, EventArgs e)
        {
            DevExpress.Utils.Menu.DXMenuItem Item = sender as DevExpress.Utils.Menu.DXMenuItem;
            GridColumn col = Item.Tag as GridColumn;
            col.SummaryItem.Tag = "Discinct";
            col.SummaryItem.SetSummary(SummaryItemType.Custom, string.Empty);
        }

        int _discinctRows = 0;
        List<object> _checkRows = new List<object>();

        private void gridView1_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            GridColumnSummaryItem item = e.Item as GridColumnSummaryItem;
            GridView view = sender as GridView;
            if (item != null && Equals("Discinct", item.Tag))
            {
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    object field = e.FieldValue;
                    object rowValue = view.GetRowCellValue(e.RowHandle, item.FieldName);
                    if (!_checkRows.Contains(rowValue))
                    {
                        _checkRows.Add(rowValue);
                        _discinctRows++;
                    }
                }
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    e.TotalValue = _discinctRows;
                    _discinctRows = 0;
                    _checkRows.Clear();
                }
            }
        }

        /// <summary>
        /// установка фильтра, для того, чтобы выгрузить в excel нужные мне строки
        /// </summary>
        private void SetFilterValue()
        {
            int[] rows = gridView1.GetSelectedRows();
            for (int i = 0; i < gridView1.DataRowCount; i++)
            {
                if (rows.Contains(i))
                {
                    gridView1.SetRowCellValue(i, gridView1.Columns["selected"], "true");
                }
                else
                {
                    gridView1.SetRowCellValue(i, gridView1.Columns["selected"], "false");
                }
            }
        }


        /// <summary>
        /// В случае сортировки по строке, появлялась надпись: "Записей - 2, например"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            GridGroupRowInfo info = e.Info as GridGroupRowInfo;
            //string caption = info.Column.Caption;
            //if (info.Column.Caption == string.Empty)
            //    caption = info.Column.ToString();
            //info.GroupText = string.Format("{0} : {1} - Записей {2}. {3}", caption, info.GroupValueText,
            //                               view.GetChildRowCount(e.RowHandle), "");
            if (info != null)
                info.GroupText = info.GroupText + string.Format(" : Записей {0}", view.GetChildRowCount(e.RowHandle));
        }

        /// <summary>
        /// Выделение всех строк и подбор ширины всех колонок. Best Fit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_ColumnWidthChanged(object sender, ColumnEventArgs e)
        {
            if (gridView1.GetSelectedRows().Count() == gridView1.RowCount)
            {
                gridView1.OptionsView.ColumnAutoWidth = false;
                gridView1.BestFitColumns();
            }
        }

        private void экспортироватьВExcelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetFilterValue();
            ColumnFilterInfo filterInfo = new ColumnFilterInfo("selected = true");
            ViewColumnFilterInfo filter = new ViewColumnFilterInfo(gridView1.Columns["selected"], filterInfo);
            gridView1.ActiveFilter.Add(filter);
            saveFileDialog1.Filter = "Excel (2003) (.xls) | *.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                gridView1.ActiveFilter.Remove(gridView1.Columns["selected"]);
            }
        }

        /// <summary>
        /// экспорт в эксел, если файл выбрали и с ним все нормально
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            gridView1.RefreshData();
            try
            {
                //gridView1.ShowPrintPreview();
                gridView1.OptionsPrint.AutoWidth = false;
                gridView1.BestFitColumns();
                gridView1.ExportToXls(saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            gridView1.ActiveFilter.Remove(gridView1.Columns["selected"]);
            Process.Start(saveFileDialog1.FileName);
        }

        private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            labelControl2.Text = gridView1.GetSelectedRows().Count().ToString();
        }

        private void удалитьДупликатыСтрокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var total = new Dictionary<int, string>();
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string hash = string.Empty;
                foreach (GridColumn col in gridView1.VisibleColumns)
                {
                    hash += gridView1.GetRowCellValue(i, col);
                }
                total.Add(i, hash);
            }
            // номера строк, которые будем удалять
            var deleteRows = new List<int>();

            for (int i = 0; i < total.Count - 1; i++)
            {
                for (int j = i + 1; j < total.Count; j++)
                {
                    if (total[i] == total[j])
                    {
                        if (!deleteRows.Contains(j))
                            deleteRows.Add(j);
                    }
                }
            }
            deleteRows.Sort();
            deleteRows.Reverse();
            foreach (var deleteRow in deleteRows)
            {
                gridView1.DeleteRow(deleteRow);
                gridView1.RefreshData();
            }
            labelControl1.Text = gridView1.DataRowCount.ToString();
        }

        /// <summary>
        /// выгрузка базы в sqlite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void вызрузкаВSqliteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            const string baseName = "base.sqlite";
            const string tableName = "calls";
            SQLiteConnection.CreateFile(baseName);
            var parameters = new List<string>();

            var mDbConnection = new SQLiteConnection("Data Source=" + baseName);
            mDbConnection.Open();

            string columns = string.Empty;
            var config = new ConfigProduct();
            try
            {
                config.LoadSettings("columns.txt");
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

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                int iParam = 0;
                foreach (var set in config.GetSettings())
                {
                    var param = new SQLiteParameter(parameters[iParam], DbType.String);
                    param.Value = gridView1.GetRowCellValue(i, set.Value);
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
            splashScreenManager1.CloseWaitForm();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Billing.Pattern.AbstractFactory;
using DevExpress.Data;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using DevExpress.XtraTabbedMdi;

namespace BillingForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Работа с best fit
            //gridView1.RowClick += 
            //gridView1.ColumnWidthChanged += new ColumnEventHandler(gridView1_ColumnWidthChanged);
            //gridView1.CellValueChanged += new CellValueChangedEventHandler(gridView1_CellValueChanged);
            //gridView1.OptionsView.ColumnAutoWidth = false;
            //OnFirstLoad();
            gridView1.PopupMenuShowing += new PopupMenuShowingEventHandler(gridView1_PopupMenuShowing);
            
            // реализуем MDI приложение
            BarManager barManager = new BarManager();
            barManager.Form = this;
            barManager.BeginUpdate();
            Bar bar = new Bar(barManager, "My Bar");
            bar.DockStyle = BarDockStyle.Top;
            barManager.MainMenu = bar;
            BarItem barItem = new BarButtonItem(barManager, "Новый документ");
            barItem.ItemClick += new ItemClickEventHandler(barItem_ItemClick);
            bar.ItemLinks.Add(barItem);
            barManager.EndUpdate();
        }

        private int ctr = 0;
        void barItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form1 f = new Form1();
            //f.MdiParent = this;
            f.Show();
        }

        private SQLiteDataAdapter _dataAdapter;
        private SQLiteConnection _sqLiteConnection;
        private SQLiteCommand _command;


        /// <summary>
        /// Нажали кнопку открыть файл БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEdit1_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

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
            if (Equals("Discinct", item.Tag))
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
        /// Открыли файл в OpenFileDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            gridControl1.DataSource = null;

            ConfigProduct config = new ConfigProduct();
            string CommandString = String.Empty;
            try
            {
                CommandString = config.LoadSqlQuery("query.sql");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            _filename.Text = String.Empty;
            _dataSet = new DataSet();
            try
            {
                _dataAdapter = new SQLiteDataAdapter(CommandString, 
                    string.Format("data source={0}", openFileDialog1.FileName));
                _dataAdapter.Fill(_dataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не могу подключиться к БД", 
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                e.Cancel = true;
                return;
            }
            _filename.Text = openFileDialog1.FileName;

            DataColumn column = new DataColumn("selected");
            _dataSet.Tables[0].Columns.Add(column);
            // передачу данных нужно передать в отдельный поток.

            try
            {
                config.LoadSettings("columns.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }

            foreach (var set in config.GetSettings())
            {
                if (_dataSet.Tables[0].Columns.Contains(set.Key))
                {
                    _dataSet.Tables[0].Columns[set.Key].ColumnName = set.Value;
                }
            }

            gridControl1.DataSource = _dataSet.Tables[0];

            gridView1.Columns["selected"].Visible = false;
            labelControl1.Text = gridView1.DataRowCount.ToString();

            gridView1.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            gridView1.OptionsMenu.EnableGroupPanelMenu = true;
            gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
            gridView1.OptionsMenu.EnableGroupPanelMenu = true;

            gridView1.ExpandAllGroups();
            gridView1.BestFitColumns();
        }
        
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Открыть базу в менюшке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void открытьSqliteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        /// <summary>
        /// Экспорт в Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void экспортироватьВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetFilterValue();
            ColumnFilterInfo filterInfo = new ColumnFilterInfo("selected = true");
            ViewColumnFilterInfo filter = new ViewColumnFilterInfo(gridView1.Columns["selected"], filterInfo);
            gridView1.ActiveFilter.Add(filter);
            saveFileDialog1.Filter = "Excel (2003) (.xls) | *.xls";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            gridView1.RefreshData();
            try
            {
                gridView1.ExportToXls(saveFileDialog1.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            gridView1.ActiveFilter.Remove(gridView1.Columns["selected"]);
            Process.Start(saveFileDialog1.FileName);
        }

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

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            labelControl2.Text = gridView1.GetSelectedRows().Count().ToString();
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
            string caption = info.Column.Caption;
            if (info.Column.Caption == string.Empty)
                caption = info.Column.ToString();
            //info.GroupText = string.Format("{0} : {1} - Записей {2}. {3}", caption, info.GroupValueText,
            //                               view.GetChildRowCount(e.RowHandle), "");
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
    }
}

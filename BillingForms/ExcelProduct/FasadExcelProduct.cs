using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace Billing.Pattern.AbstractFactory
{
    public abstract class FasadExcelProduct
    {
        private Excel.Application excelapp;
        private Excel.Workbook workbook_current;
        private Excel.Worksheet worksheet_current;
        protected Logger logger;
        public FasadExcelProduct()
        {
            excelapp = new Excel.Application();
            //excelapp.Visible = true;
        }

        public void OpenExcelApp(string path)
        {
            // проверим наличие файла на диске
            // в случае, если файла не существует, вызываем Exception
            if (!File.Exists(path))
            {
                throw new COMException();
            }
            // будем открывать только для чтения 
            excelapp.Workbooks.Open(  path,
                                        Type.Missing, Type.Missing, true, Type.Missing,
                                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing);
            
        }

        public void CloseExcelApp()
        {
            releaseObject(excelapp);
            releaseObject(workbook_current);
            releaseObject(worksheet_current);
        }

        protected void SetCurrentWorkbook(int Number)
        {
            workbook_current = excelapp.Workbooks.get_Item(Number);
        }

        protected void CloseCurrentWorkbook()
        {
            workbook_current.Close(true, null, null);
        }

        protected int CountColumnsWorksheet
        {
            get { return worksheet_current.Columns.Count; }
        }

        protected int CountRowsWorksheet 
        { 
            get { return worksheet_current.Rows.Count; }
        }

        protected int CountSheets
        {
            get { return workbook_current.Sheets.Count; }
        }

        protected void SetCurrentWorksheet(int Number)
        {
            worksheet_current = workbook_current.Worksheets.get_Item(Number);
        }

        protected object GetElementFromWorkSheet(int row, int column)
        {
            object result = worksheet_current.Cells[row, column].Value2;
            return result;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        public int GetCountColumns(int numworksheet = 1)
        {
            SetCurrentWorkbook(1);
            SetCurrentWorksheet(numworksheet);
            return worksheet_current.UsedRange.Columns.Count;
        }
    }
}

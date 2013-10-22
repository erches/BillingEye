using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Billing.Pattern.AbstractFactory
{
    public class ExcelProduct : FasadExcelProduct, IExcelProduct
    {
        // число колонок берется из файла с настройками соответствия
        private int CountColumns { get; set; }

        // придумаем алгоритм подсчета этого значения
        private int CurrentCountRows { get; set; }

        // таблица с данными
        private List<string[,]> LTable { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="path">Открытие excel файла</param>
        public ExcelProduct(Logger log)
        {
            this.logger = log;
            //this.CountColumns = countcolumns;
            //OpenExcelApp(path);
            LTable = new List<string[,]>();
        }

        private void SetCountColumns(int numworksheet = 1, int countcolumns = -1)
        {
            if (countcolumns == -1)
            {
                this.CountColumns = GetCountColumns(numworksheet);
                return;
            }
            this.CountColumns = countcolumns;
        }

        public void MigateDate()
        {
            SetCurrentWorkbook(1);
            for (int i = 1; i <= CountSheets; i++)
            {
                try{
                    // считаем количество строк у текущего листа
                    SetCountColumns();
                    GetCountRows(i);
                }
                catch (NullReferenceException) { return; }
                

                // -1, потому что не берем первую строку с названиями столбцов
                var cTable = new string[CurrentCountRows-1,CountColumns];
                
                for (int j = 2; j <= CurrentCountRows; j++)
                {
                    for (int k = 1; k <= CountColumns; k++)
                    {
                        object CurrentData = GetElementFromWorkSheet(j, k);
                        if (CurrentData != null)
                            cTable[j - 2, k - 1] = CurrentData.ToString();
                    }
                }
                LTable.Add(cTable);
            }
            CloseCurrentWorkbook();
        }

        public List<string[,]> GetTablesFromExcel()
        {
            return this.LTable;
        }

        public List<string> GetColumns(int numWorkSheet)
        {
            SetCurrentWorkbook(1);
            SetCurrentWorksheet(numWorkSheet);
            SetCountColumns(numWorkSheet);
            var columns = new List<string>();
            for (int k = 1; k <= CountColumns; k++)
            {
                object CurrentData = GetElementFromWorkSheet(1, k);
                columns.Add(CurrentData != null ? CurrentData.ToString() : null);
            }
            return columns;
        }

        /// <summary>
        /// подсчет количества строк в текущем листе
        /// </summary>
        /// <param name="numWorkSheet"></param>
        private void GetCountRows(int numWorkSheet)
        {
            // если в строке все значения пусты => останов
            SetCurrentWorksheet(numWorkSheet);

            if (GetElementFromWorkSheet(1, 1) == null)
                throw new NullReferenceException();

            object CurrentData = String.Empty;
            int i = 1; // номер текущей строки
            while(true)
            {
                for (int j = 1; j <= CountColumns; j++)
                {
                    CurrentData = GetElementFromWorkSheet(i, j);
                    if (CurrentData != null)
                        break;
                    if (j == CountColumns)
                    {
                        CurrentCountRows = i;
                        i = -1;
                    }
                }
                if (i < 0)
                {
                    CurrentCountRows = CurrentCountRows - 1;
                    break;
                }
                i++;
            }
        }
    }
}

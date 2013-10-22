using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Pattern.AbstractFactory
{
    public interface IExcelProduct
    {
        /// <summary>
        /// Открытие Excel файла
        /// </summary>
        /// <param name="path"></param>
        void OpenExcelApp(string path);
        
        /// <summary>
        /// Закрытие Excel
        /// </summary>
        void CloseExcelApp();

        /// <summary>
        /// Заполнение таблицы из Excel
        /// </summary>
        void MigateDate();

        /// <summary>
        /// получение списка всех данных в страницах
        /// </summary>
        /// <returns></returns>
        List<string[,]> GetTablesFromExcel();

        List<string> GetColumns(int numWorkSheet);
    }
}

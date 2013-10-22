using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Billing.Pattern.AbstractFactory
{
    public class ConfigProduct
    {
        private Logger logger { get; set; }
        private Dictionary<string, string> Settings { get; set; }

        public ConfigProduct()
        {
            Settings = new Dictionary<string, string>();
        }
        public ConfigProduct(Logger log)
        {
            this.logger = log;
            Settings = new Dictionary<string, string>();
        }

        public void LoadSettings(string path, string section)
        {
            // в случае, если файла не существует, вызываем Exception
            if (!File.Exists(path))
            {
                throw new FileLoadException();
            }
            using (var sr = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                while (sr.Peek() >= 0)
                {
                    // берем первую строку в конфиг файле
                    string line = sr.ReadLine();

                    char[] charsToTrim = { '\t', '\n' };
                    line = line.Trim(charsToTrim);

                    if (line == String.Empty)
                    {
                        continue;
                    }

                    // находим нашу секцию [section]
                    int pos = line.IndexOf(string.Format("[{0}]", section), System.StringComparison.Ordinal);
                    if (pos == -1)
                    {
                        continue;
                    }

                    // находим знак ">" и добавляем в словарь
                    pos = line.IndexOf('>');
                    Settings.Add(line.Substring(0, pos).Trim(),
                        line.Substring(pos + 1, line.Length - pos - 1).Trim());
                }
            }
        }

        public void LoadSettings(string path)
        {
            // в случае, если файла не существует, вызываем Exception
            if (!File.Exists(path))
            {
                throw new FileLoadException();
            }
            using (var sr = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                while (sr.Peek() >= 0)
                {
                    // берем первую строку в конфиг файле
                    string line = sr.ReadLine();

                    char[] charsToTrim = { '\t', '\n' };
                    line = line.Trim(charsToTrim);

                    if (line == String.Empty)
                    {
                        continue;
                    }

                    // находим знак ">" и добавляем в словарь
                    int pos = line.IndexOf('>');
                    Settings.Add(line.Substring(0, pos).Trim(), 
                        line.Substring(pos + 1, line.Length - pos - 1).Trim());
                }
            }
        }


        public string LoadSqlQuery(string path)
        {
            // в случае, если файла не существует, вызываем Exception
            if (!File.Exists(path))
            {
                throw new FileLoadException();
            }
            using (var sr = new StreamReader(path, Encoding.GetEncoding(1251)))
            {
                while (sr.Peek() >= 0)
                {
                    // берем первую строку в конфиг файле
                    string line = sr.ReadLine();

                    char[] charsToTrim = { '\t', '\n' };
                    line = line.Trim(charsToTrim);

                    if (line == String.Empty)
                    {
                        continue;
                    }

                    return line.Trim();
                }
            }
            return "select * from calls";
        }

        public Dictionary<string, string> GetSettings()
        {
            return Settings;
        }
    }
}

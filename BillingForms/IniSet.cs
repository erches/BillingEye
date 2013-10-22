using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IniParser;
using IniParser.Model;

namespace BillingForms
{
    public class IniSet
    {
        private string _fileExt { get; set; }
        private string _fileType { get; set; }
        private Dictionary<string, string> _columnsList { get; set; }
        private string _sqlPath { get; set; }
        private FileIniDataParser _parser { get; set; }
        private IniData _parsedData { get; set; }
        private string _file_path_ini { get; set; }

        public IniSet(string filepath)
        {
            this._file_path_ini = filepath;
            //Create an instance of a ini file parser
            this._parser = new FileIniDataParser();
            //Load the INI file which also parses the INI data
            this._parsedData = _parser.ReadFile(filepath, Encoding.GetEncoding(1251));
            this._columnsList = new Dictionary<string, string>();
        }

        public void LoadSettings()
        {
            //Iterate through all the sections
            foreach (SectionData section in _parsedData.Sections)
            {
                if (section.SectionName == "file")
                {
                    this._fileExt = _parsedData["file"]["file_ext"];
                    this._fileType = _parsedData["file"]["file_type"];
                }
                if (section.SectionName == "columns")
                {
                    //Iterate through all the keys in the current section
                    foreach (var key in section.Keys)
                    {
                        _columnsList.Add(key.KeyName, key.Value);
                    }
                }
                if (section.SectionName == "sql")
                {
                    this._sqlPath = _parsedData["sql"]["sql_file_name"];
                }
            }
            if (_fileExt == null || _fileType == null || _columnsList.Count == 0 || _sqlPath == null)
            {
                throw new Exception("Неправильно определен конфигурационный файл " + _file_path_ini);
            }
        }

        public string GetFileExt()
        {
            return _fileExt;
        }

        public string GetFileType()
        {
            return _fileType;
        }

        public string GetSqlPath()
        {
            return this._sqlPath;
        }

        public Dictionary<string, string> GetColumns()
        {
            return _columnsList;
        }
    }
}

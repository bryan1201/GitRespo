using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TransformReport.Configuration
{
    public class ReportElement : ConfigurationElement
    {
        private const string NAME = "name";
        private const string ROWS = "排";
        private const string COLS = "欄位";
        private const string HEADER_COLS = "HeaderCols";
        private const string EXCELFILE = "ExcelFile";

        public ReportElement()
        {
        }

        public ReportElement(string elementName)
        {
            Name = elementName;
        }

        [ConfigurationProperty(NAME, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NAME]; }
            set { this[NAME] = value; }
        }

        [ConfigurationProperty(HEADER_COLS, IsRequired = true)]
        public int HeaderCols
        {
            get { return (int)this[HEADER_COLS]; }
            set { this[HEADER_COLS] = value; }
        }

        [ConfigurationProperty(EXCELFILE, IsRequired = true)]
        public string ExcelFile
        {
            get { return (string)this[EXCELFILE]; }
            set { this[EXCELFILE] = value; }
        }

        [ConfigurationProperty(ROWS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection))]
        public NameValueConfigurationCollection Rows
        {
            get { return (NameValueConfigurationCollection)this[ROWS]; }
        }

        [ConfigurationProperty(COLS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection))]
        public NameValueConfigurationCollection Cols
        {
            get { return (NameValueConfigurationCollection)this[COLS]; }
        }
    }
}

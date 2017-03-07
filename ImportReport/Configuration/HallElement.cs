using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ImportReport.Configuration
{
    public class HallElement : ConfigurationElement
    {
        private const string NAME = "name";

        private const string SOURCE_TOP_HEADER_ROW = "sourceTopHeaderRow";
        private const string SOURCE_TOP_SUBHEADER_ROW = "sourceTopSubHeaderRow";
        private const string SOURCE_TOP_HEADER_COL = "sourceTopHeaderCol";

        private const string SOURCE_LEFT_HEADER_ROW = "sourceLeftHeaderRow";
        private const string SOURCE_LEFT_HEADER_COL = "sourceLeftHeaderCol";
        private const string SOURCE_LEFT_SUBHEADER_COL = "sourceLeftSubHeaderCol";

        private const string ROWS = "小區";

        public HallElement()
        {
        }

        public HallElement(string elementName)
        {
            Name = elementName;
        }

        [ConfigurationProperty(NAME, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NAME]; }
            set { this[NAME] = value; }
        }

        [ConfigurationProperty(SOURCE_TOP_HEADER_ROW, DefaultValue = 1)]
        public int SourceTopHeaderRow
        {
            get { return (int)this[SOURCE_TOP_HEADER_ROW]; }
            set { this[SOURCE_TOP_HEADER_ROW] = value; }
        }

        [ConfigurationProperty(SOURCE_TOP_SUBHEADER_ROW, DefaultValue = 2)]
        public int SourceTopSubHeaderRow
        {
            get { return (int)this[SOURCE_TOP_SUBHEADER_ROW]; }
            set { this[SOURCE_TOP_SUBHEADER_ROW] = value; }
        }

        [ConfigurationProperty(SOURCE_TOP_HEADER_COL, DefaultValue = 12)]
        public int SourceTopHeaderCol
        {
            get { return (int)this[SOURCE_TOP_HEADER_COL]; }
            set { this[SOURCE_TOP_HEADER_COL] = value; }
        }

        [ConfigurationProperty(SOURCE_LEFT_HEADER_ROW, DefaultValue = 3)]
        public int SourceLeftHeaderRow
        {
            get { return (int)this[SOURCE_LEFT_HEADER_ROW]; }
            set { this[SOURCE_LEFT_HEADER_ROW] = value; }
        }

        [ConfigurationProperty(SOURCE_LEFT_HEADER_COL, DefaultValue = 1)]
        public int SourceLeftHeaderCol
        {
            get { return (int)this[SOURCE_LEFT_HEADER_COL]; }
            set { this[SOURCE_LEFT_HEADER_COL] = value; }
        }

        [ConfigurationProperty(SOURCE_LEFT_SUBHEADER_COL, DefaultValue = 2)]
        public int SourceLeftSubHeaderCol
        {
            get { return (int)this[SOURCE_LEFT_SUBHEADER_COL]; }
            set { this[SOURCE_LEFT_SUBHEADER_COL] = value; }
        }

        [ConfigurationProperty(ROWS, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection))]
        public NameValueConfigurationCollection Rows
        {
            get { return (NameValueConfigurationCollection)this[ROWS]; }
        }
    }
}

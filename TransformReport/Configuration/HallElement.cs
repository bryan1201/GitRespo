using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TransformReport.Configuration
{
    public class HallElement : ConfigurationElement
    {
        private const string NAME = "name";

        private const string TARGET_TOP_HEADER_ROW = "targetTopHeaderRow";
        //private const string TARGET_TOP_SUBHEADER_ROW = "targetTopSubHeaderRow";
        private const string TARGET_TOP_HEADER_COL = "targetTopHeaderCol";

        private const string TARGET_LEFT_HEADER_ROW = "targetLeftHeaderRow";
        private const string TARGET_LEFT_HEADER_COL = "targetLeftHeaderCol";
        //private const string TARGET_LEFT_SUBHEADER_COL = "targetLeftSubHeaderCol";

        private const string ROWS = "小區";
        private const string REPORT = "報表";

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

        [ConfigurationProperty(TARGET_TOP_HEADER_ROW, DefaultValue = 1)]
        public int TargetTopHeaderRow
        {
            get { return (int)this[TARGET_TOP_HEADER_ROW]; }
            set { this[TARGET_TOP_HEADER_ROW] = value; }
        }

        /*
        [ConfigurationProperty(TARGET_TOP_SUBHEADER_ROW, DefaultValue = 2)]
        public int TargetTopSubHeaderRow
        {
            get { return (int)this[TARGET_TOP_SUBHEADER_ROW]; }
            set { this[TARGET_TOP_SUBHEADER_ROW] = value; }
        }
        */

        [ConfigurationProperty(TARGET_TOP_HEADER_COL, DefaultValue = 12)]
        public int TargetTopHeaderCol
        {
            get { return (int)this[TARGET_TOP_HEADER_COL]; }
            set { this[TARGET_TOP_HEADER_COL] = value; }
        }

        [ConfigurationProperty(TARGET_LEFT_HEADER_ROW, DefaultValue = 3)]
        public int TargetLeftHeaderRow
        {
            get { return (int)this[TARGET_LEFT_HEADER_ROW]; }
            set { this[TARGET_LEFT_HEADER_ROW] = value; }
        }

        [ConfigurationProperty(TARGET_LEFT_HEADER_COL, DefaultValue = 1)]
        public int TargetLeftHeaderCol
        {
            get { return (int)this[TARGET_LEFT_HEADER_COL]; }
            set { this[TARGET_LEFT_HEADER_COL] = value; }
        }

        /*
        [ConfigurationProperty(TARGET_LEFT_SUBHEADER_COL, DefaultValue = 2)]
        public int TargetLeftSubHeaderCol
        {
            get { return (int)this[TARGET_LEFT_SUBHEADER_COL]; }
            set { this[TARGET_LEFT_SUBHEADER_COL] = value; }
        }
        */

        [ConfigurationProperty(ROWS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection))]
        public NameValueConfigurationCollection Rows
        {
            get { return (NameValueConfigurationCollection)this[ROWS]; }
        }

        [ConfigurationProperty(REPORT, IsDefaultCollection=false)]
        [ConfigurationCollection(typeof(ReportCollection), AddItemName="add", RemoveItemName="remove", ClearItemsName="clear")]
        public ReportCollection Reports
        {
            get { return (ReportCollection)this[REPORT];}
        }
    }
}

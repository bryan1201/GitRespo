using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TransformReport.Configuration
{
    public sealed class TransformSection : ConfigurationSection
    {
        public const string SECTION_NAME = "transformConfiguration";

        private const string SOURCE_TOP_HEADER_ROW = "sourceTopHeaderRow";
        private const string SOURCE_TOP_HEADER_COL = "sourceTopHeaderCol";

        private const string SOURCE_LEFT_HEADER_ROW = "sourceLeftHeaderRow";
        private const string SOURCE_LEFT_HEADER_COL = "sourceLeftHeaderCol";

        private const string SEPERATOR = "seperator";
        private const string TEMPLATE_EXCEL_FILE = "templateExcelFile";

        private const string HALL = @"會所";

        [ConfigurationProperty(SOURCE_TOP_HEADER_ROW, DefaultValue = 0)]
        public int SourceTopHeaderRow
        {
            get { return (int)this[SOURCE_TOP_HEADER_ROW]; }
            set { this[SOURCE_TOP_HEADER_ROW] = value; }
        }

        [ConfigurationProperty(SOURCE_TOP_HEADER_COL, DefaultValue = 3)]
        public int SourceTopHeaderCol
        {
            get { return (int)this[SOURCE_TOP_HEADER_COL]; }
            set { this[SOURCE_TOP_HEADER_COL] = value; }
        }

        [ConfigurationProperty(SOURCE_LEFT_HEADER_ROW, DefaultValue = 2)]
        public int SourceLeftHeaderRow
        {
            get { return (int)this[SOURCE_LEFT_HEADER_ROW]; }
            set { this[SOURCE_LEFT_HEADER_ROW] = value; }
        }

        [ConfigurationProperty(SOURCE_LEFT_HEADER_COL, DefaultValue = 2)]
        public int SourceLeftHeaderCol
        {
            get { return (int)this[SOURCE_LEFT_HEADER_COL]; }
            set { this[SOURCE_LEFT_HEADER_COL] = value; }
        }

        [ConfigurationProperty(SEPERATOR, DefaultValue = "@")]
        public string Seperator
        {
            get { return (string)this[SEPERATOR]; }
            set { this[SEPERATOR] = value; }
        }

        [ConfigurationProperty(TEMPLATE_EXCEL_FILE, IsRequired = true)]
        public string TemplateExcelFile
        {
            get { return (string)this[TEMPLATE_EXCEL_FILE]; }
            set { this[TEMPLATE_EXCEL_FILE] = value; }
        }

        [ConfigurationProperty(HALL, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HallCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public HallCollection Halls
        {
            get { return (HallCollection)this[HALL]; }
        }
    }
}

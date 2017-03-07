using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ImportReport.Configuration
{
    public sealed class ImportSection : ConfigurationSection
    {
        public const string SECTION_NAME = "importConfiguration";

        private const string SEPERATOR = "seperator";

        private const string REPORT = "報表";
        private const string HALL = "會所";

        [ConfigurationProperty(SEPERATOR, DefaultValue = "@")]
        public string Seperator
        {
            get { return (string)this[SEPERATOR]; }
            set { this[SEPERATOR] = value; }
        }

        [ConfigurationProperty(REPORT, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(NameValueConfigurationCollection))]
        public NameValueConfigurationCollection Cols
        {
            get { return (NameValueConfigurationCollection)this[REPORT]; }
        }

        [ConfigurationProperty(HALL, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HallCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public HallCollection Halls
        {
            get { return (HallCollection)this[HALL]; }
        }
    }
}

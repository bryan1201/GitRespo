using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TransformReport.Configuration
{
    public class ReportCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public ReportElement this[int index]
        {
            get { return (ReportElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public ReportElement this[string name]
        {
            get { return (ReportElement)BaseGet(name); }
        }

        public int IndexOf(ReportElement reportElement)
        {
            return BaseIndexOf(reportElement);
        }

        public void Add(ReportElement reportElement)
        {
            BaseAdd(reportElement);
        }

        public void Remove(ReportElement reportElement)
        {
            if (BaseIndexOf(reportElement) > 0)
                BaseRemove(reportElement.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public new string AddElementName
        {
            get { return base.AddElementName; }
            set { base.AddElementName = value; }
        }

        public new string RemoveElementName
        {
            get { return base.RemoveElementName; }
            set { base.RemoveElementName = value; }
        }

        public new string ClearElementName
        {
            get { return base.ClearElementName; }
            set { base.ClearElementName = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ReportElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new ReportElement(elementName);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ReportElement).Name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TransformReport.Configuration
{
    public class HallCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        public HallElement this[int index]
        {
            get { return (HallElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public HallElement this[string name]
        {
            get { return (HallElement)BaseGet(name); }
        }

        public int IndexOf(HallElement hallElement)
        {
            return BaseIndexOf(hallElement);
        }

        public void Add(HallElement hallElement)
        {
            BaseAdd(hallElement);
        }

        public void Remove(HallElement hallElement)
        {
            if (BaseIndexOf(hallElement) > 0)
                BaseRemove(hallElement.Name);
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
            return new HallElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new HallElement(elementName);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as HallElement).Name;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class GenericLibraryItemCollection<TItemType>
        : System.Collections.Generic.IEnumerable<TItemType>,
        System.Collections.IEnumerable
        where TItemType : GenericLibraryItem, new()
    {
        private List<TItemType> items;
        private ArcadiaLibrary parentGameLibrary;

        public GenericLibraryItemCollection(ArcadiaLibrary parentGameLibrary)
        {
            this.parentGameLibrary = parentGameLibrary;
            items = new List<TItemType>();
        }
        public GenericLibraryItemCollection()
            : this(null)
        {
        }


        #region XML code

        #region writing
        public void WriteToXml(XmlWriter writer, string collectionNodeName)
        {
            this.Sort();

            writer.WriteStartElement(collectionNodeName);

            writeToXmlExtension(writer);

            foreach (TItemType item in this)
                item.WriteToXml(writer);

            writer.WriteEndElement();
        }
        public void WriteToXml(XmlWriter writer)
        {
            WriteToXml(writer, DefaultXmlNodeName);
        }
        protected virtual void writeToXmlExtension(XmlWriter writer)
        {

        }
        #endregion

        #region reading
        public void ReadFromXml(XmlNode node, string libraryItemNodesName)
        {
            if (node == null)
                return;

            readFromXmlExtension(node);

            foreach (XmlNode libraryItemNode in node.SelectNodes(libraryItemNodesName))
            {
                TItemType item = new TItemType();
                item.ParentGameLibrary = ParentGameLibrary;
                item.ReadFromXml(libraryItemNode);
                Add(item);
            }
        }
        public virtual void ReadFromXml(XmlNode node)
        {
            ReadFromXml(node, DefaultXmlChildNodeName);
        }
        protected virtual void readFromXmlExtension(XmlNode node)
        {

        }
        #endregion

        #region nameing
        public string DefaultXmlNodeName
        {
            get
            {
                string fullName = this.GetType().FullName;
                string lastPart = fullName.Substring(fullName.LastIndexOf(".") + 1);
                return lastPart.Substring(0, 1).ToLower() + lastPart.Substring(1);
            }
        }
        public string DefaultXmlChildNodeName
        {
            get
            {
                return DefaultXmlNodeName.Replace("Collection", "");
            }
        }
        #endregion

        #endregion

        public List<TItemType> GetByName(string name, StringComparison comparison)
        {
            List<TItemType> toReturn = new List<TItemType>();

            foreach (TItemType item in this)
                if (item.NameMatches(name, comparison))
                    toReturn.Add(item);

            return toReturn;
        }

        public List<TItemType> GetByName(string name)
        {
            return GetByName(name, StringComparison.InvariantCulture);
        }

        public TItemType GetById(string id)
        {
            foreach (TItemType item in this)
                if (item.Id == id)
                    return item;
            throw new Exception("could not find item with id" + id);
        }

        public List<TItemType> GetValues()
        {
            List<TItemType> result = new List<TItemType>(items.Count);
            foreach (TItemType item in items)
                result.Add(item);
            return result;
        }

        public ArcadiaLibrary ParentGameLibrary
        {
            get
            {
                return parentGameLibrary;
            }
        }

        public void Sort()
        {
            items.Sort();
        }

        #region basic collection stuff

        public TItemType this[int index]
        {
            get
            {
                return items[index];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.Generic.IEnumerator<TItemType> System.Collections.Generic.IEnumerable<TItemType>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public virtual void Add(TItemType item)
        {
            item.ParentGameLibrary = ParentGameLibrary;
            items.Add(item);
        }

        public bool Contains(TItemType item)
        {
            return items.Contains(item);
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public virtual void Clear()
        {
            while (items.Count > 0)
                RemoveAt(items.Count - 1);
        }

        public virtual bool Remove(TItemType item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Remove(items[index]);
        }

        #endregion
    }
}

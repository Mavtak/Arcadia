using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class GenericLibraryItemCollection<TItemType> : ICollection<TItemType>
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

        public TItemType this[string id]
        {
            get
            {
                var result = from item in items
                             where item.Id == id
                             select item;

                if (result.Any())
                {
                    return result.FirstOrDefault();
                }

                throw new KeyNotFoundException();
            }
        }

        public System.Collections.Generic.IEnumerator<TItemType> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.Generic.IEnumerator<TItemType> System.Collections.Generic.IEnumerable<TItemType>.GetEnumerator()
        {
            return GetEnumerator();
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

        #region ICollection<TItemType> implementation

        void ICollection<TItemType>.Add(TItemType item)
        {
            Add(item);
        }

        void ICollection<TItemType>.Clear()
        {
            Clear();
        }

        bool ICollection<TItemType>.Contains(TItemType item)
        {
            return Contains(item);
        }

        void ICollection<TItemType>.CopyTo(TItemType[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        int ICollection<TItemType>.Count
        {
            get
            {
                return Count;
            }
        }

        bool ICollection<TItemType>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection<TItemType>.Remove(TItemType item)
        {
            return Remove(item);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class GenericLibraryItem : IComparable
    {
        //private GenericLibraryItemCollection<GenericLibraryItem> parentCollection;
        private ArcadiaLibrary parentGameLibrary;
        private string name = null;
        private List<string> otherNames = new List<string>();
        private string id = null;

        public GenericLibraryItem(string name, ArcadiaLibrary parentGameLibrary)
        {
            otherNames = new List<string>();
            this.name = name;
            this.parentGameLibrary = parentGameLibrary;
        }

        public GenericLibraryItem(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        {
        }

        #region object overrides

        /*
        public override bool Equals(object obj)
        {
            GenericLibraryItem<T> that = (GenericLibraryItem<T>)obj;

            return this.Name == that.Name
                && this.GetType() == that.GetType();
        }*/

        public override string ToString()
        {
            return Name;
        }

        public override int GetHashCode()
        {
            if(NameIsSet)
                return Name.GetHashCode();
            return base.GetHashCode();
        }

        #endregion

        #region IComparable implimentations
        int System.IComparable.CompareTo(object obj)
        {
            return this.name.CompareTo(((GenericLibraryItem)obj).name);
        }
        #endregion

        #region xml

        #region writing
        public void WriteToXml(XmlWriter writer, string nodeName)
        {
            writer.WriteStartElement(nodeName);

            if (NameIsSet)
                writer.WriteAttributeString("name", Name);

            writer.WriteAttributeString("id", Id);

            writeToXmlExtension(writer);

            if (OtherNamesAreSet)
                Common.WriteListOfStringsToXml(writer, otherNames, "otherNames", "name");

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
        public void ReadFromXml(XmlNode node)
        {
            if (node.Attributes["name"] != null)
                Name = node.Attributes["name"].Value;

            if (node.Attributes["id"] != null)
                id = node.Attributes["id"].Value;

            Common.ReadListOfStringsFromXml(node.SelectSingleNode("otherNames"), "name", OtherNames);

            readFromXmlExtension(node);
        }
        protected virtual void readFromXmlExtension(XmlNode node)
        {

        }
        #endregion

        #region naming

        public string DefaultXmlNodeName
        {
            get
            {
                string fullName = this.GetType().FullName;
                string lastPart = fullName.Substring(fullName.LastIndexOf(".") + 1);
                return lastPart.Substring(0, 1).ToLower() + lastPart.Substring(1);
            }
        }

        #endregion

        #endregion

        #region basic stuff

        public ArcadiaLibrary ParentGameLibrary
        {
            get
            {
                return parentGameLibrary;
            }
            set
            {
                //TODO: remove from old library
                parentGameLibrary = value;
            }
        }

        public bool NameIsSet
        {
            get
            {
                return !String.IsNullOrEmpty(name);
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public bool OtherNamesAreSet
        {
            get
            {
                return otherNames != null && otherNames.Count > 0;
            }
        }

        public List<string> OtherNames
        {
            get
            {
                if (otherNames == null)
                    otherNames = new List<string>();
                return otherNames;
            }
            set
            {
                otherNames = value;
            }
        }

        public bool NameMatches(string value, StringComparison comparisonType)
        {
            if (value.Equals(Name, comparisonType))
                return true;

            foreach (string otherName in OtherNames)
                if (value.Equals(otherName, comparisonType))
                    return true;

            return false;
        }

        public bool NameMatches(string value)
        {
            return NameMatches(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool IdIsSet
        {
            get
            {
                return id != null;
            }
        }

        public string Id
        {
            get
            {
                if (!IdIsSet)
                    id = Common.IdGenerator.NextString();
                return id;
            }
        }
        #endregion


    }
}

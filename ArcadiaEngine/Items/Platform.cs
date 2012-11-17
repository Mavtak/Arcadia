using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class Platform : GenericLibraryItem, IFilterItem
    {
        public List<string> UniqueFileExtensions { get; private set; }
        public ICollection<Regex> NamePatterns { get; private set; }

        #region constructors

        public Platform(string name, ArcadiaLibrary parentGameLibrary)
            : base(name, parentGameLibrary)
        {
            NamePatterns = new LinkedList<Regex>();
            UniqueFileExtensions = new List<string>();
        }

        public Platform(string name)
            : this(name, null)
        { }

        public Platform(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        { }

        public Platform()
            : this(null, null)
        { }

        #endregion

        bool IFilterItem.Matches(Game game)
        {
            return game.Platform.Equals(this);
        }

        #region XML

        protected override void writeToXmlExtension(System.Xml.XmlWriter writer)
        {
            if (UniqueFileExtensionsAreSet)
            {
                Common.WriteListOfStringsToXml(writer, UniqueFileExtensions, "uniqueFileExtensions", "extension");
            }

            if (NamePatterns.Count > 0)
            {
                var data = NamePatterns.Select(pattern => pattern.ToString());

                Common.WriteListOfStringsToXml(writer, data, "namePatterns", "regularExpression");
            }
        }

        protected override void readFromXmlExtension(System.Xml.XmlNode node)
        {
            Common.ReadListOfStringsFromXml(node.SelectSingleNode("uniqueFileExtensions"), "extension", UniqueFileExtensions);

            var data = new LinkedList<string>();
            Common.ReadListOfStringsFromXml(node.SelectSingleNode("namePatterns"), "regularExpression", data);
            foreach (var pattern in data)
            {
                NamePatterns.Add(new Regex(pattern, RegexOptions.IgnoreCase));
            }
        }

        #endregion

        #region Extensions code
        public bool UniqueFileExtensionsAreSet
        {
            get
            {
                return UniqueFileExtensions != null && UniqueFileExtensions.Count > 0;
            }
        }

        public bool ExtensionMatches(string extensionToCheck)
        {
            if (!UniqueFileExtensionsAreSet)
            {
                return false;
            }

            var result = UniqueFileExtensions.Any(extension => extensionToCheck.Equals(extension, StringComparison.InvariantCultureIgnoreCase));

            return result;
        }

        public override bool NameMatches(string value, StringComparison comparisonType)
        {
            if (base.NameMatches(value, comparisonType))
            {
                return true;
            }

            if (NamePatterns.Any(pattern => pattern.IsMatch(value)))
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}

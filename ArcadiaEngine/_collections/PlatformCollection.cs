using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class PlatformCollection : GenericLibraryItemCollection<Platform>
    {
        private List<string> commonFileExtensions;

        public PlatformCollection(ArcadiaLibrary parentGameLibrary)
            : base(parentGameLibrary)
        {
            commonFileExtensions = new List<string>();
        }

        #region xml
        protected override void writeToXmlExtension(XmlWriter writer)
        {
            if (CommonFileExtensionsAreSet)
                Common.WriteListOfStringsToXml(writer, CommonFileExtensions, "commonFileExtensions", "extension");
        }

        protected override void readFromXmlExtension(XmlNode node)
        {
            Common.ReadListOfStringsFromXml(node.SelectSingleNode("commonFileExtensions"), "extension", commonFileExtensions);
        }
        #endregion

        #region Extensions
        public bool CommonFileExtensionsAreSet
        {
            get
            {
                return commonFileExtensions != null && commonFileExtensions.Count > 0;
            }
        }
        public List<string> CommonFileExtensions
        {
            get
            {
                if (commonFileExtensions == null)
                    commonFileExtensions = new List<string>();
                return commonFileExtensions;
            }
        }
        public bool ExtensionMatchesGeneric(string extensionToCheck)
        {
            if (!CommonFileExtensionsAreSet)
                return false;
            foreach (string otherMatch in CommonFileExtensions)
                if (extensionToCheck.Equals(otherMatch, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            return false;
        }

        public IEnumerable<Platform> FindPlatformsByExtension(string extension)
        {
            if (extension.StartsWith("."))
            {
                extension = extension.Substring(1);
            }

            var result = from platform in this
                         where platform.ExtensionMatches(extension)
                         select platform;

            return result;
        }
        public bool CheckIfIsRomExtension(string extension)
        {
            if (ExtensionMatchesGeneric(extension))
                return true;
            foreach (Platform platform in this)
                if (platform.ExtensionMatches(extension))
                    return true;
            return false;

        }
        #endregion

        public IEnumerable<Platform> FindPlatformsByFileDirectory(string path)
        {
            HashSet<Platform> alreadyReturned = new HashSet<Platform>();

            foreach(string pathPart in path.Split(System.IO.Path.DirectorySeparatorChar))
            {
                var moreMatches = ParentGameLibrary.Platforms.GetByName(pathPart);

                //merge
                foreach (Platform match in moreMatches)
                {
                    if (!alreadyReturned.Contains(match))
                    {
                        alreadyReturned.Add(match);
                        yield return match;
                    }
                }
            }
        }

        public IEnumerable<Platform> FindPlatforms(string filePath)
        {
            var matches = FindPlatformsByExtension(System.IO.Path.GetExtension(filePath));

            var moreMatches = FindPlatformsByFileDirectory(System.IO.Path.GetDirectoryName(filePath));

            var results = matches.Union(moreMatches);

            return results;
        }

        public override bool Remove(Platform item)
        {
            var games = from game in ParentGameLibrary.Games
                        where game.Platform == item
                        select game;

            foreach (var game in games)
            {
                game.Platform = null;
            }

            return base.Remove(item);
        }
        public override void Clear()
        {
            if (ParentGameLibrary != null)
            {
                foreach (Game game in ParentGameLibrary.Games)
                    game.Platform = null;
            }
            base.Clear();
        }
    }
}

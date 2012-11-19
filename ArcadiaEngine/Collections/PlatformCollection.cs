using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class PlatformCollection : GenericLibraryItemCollection<Platform>
    {
        public List<string> CommonFileExtensions { get; private set; }

        public PlatformCollection(ArcadiaLibrary parentGameLibrary)
            : base(parentGameLibrary)
        {
            CommonFileExtensions = new List<string>();
        }

        #region xml
        protected override void writeToXmlExtension(XmlWriter writer)
        {
            if (CommonFileExtensions.Count > 0)
            {
                Common.WriteListOfStringsToXml(writer, CommonFileExtensions, "commonFileExtensions", "extension");
            }
        }

        protected override void readFromXmlExtension(XmlNode node)
        {
            Common.ReadListOfStringsFromXml(node.SelectSingleNode("commonFileExtensions"), "extension", CommonFileExtensions);
        }
        #endregion

        #region Extensions

        public bool ExtensionMatchesGeneric(string extensionToCheck)
        {
            var matches = from extension in CommonFileExtensions
                          where extension.Equals(extensionToCheck, StringComparison.InvariantCultureIgnoreCase)
                          select extensionToCheck;

            var result = matches.Any();

            return result;
        }

        public IEnumerable<Platform> FindPlatformsByExtension(string extension)
        {
            if (extension != null && extension.StartsWith("."))
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
            {
                return true;
            }

            var matches = from platform in this
                          where platform.ExtensionMatches(extension)
                          select platform;

            var result = matches.Any();

            return result;
        }

        #endregion

        public IEnumerable<Platform> FindPlatformsByFileDirectory(string path)
        {
            var alreadyReturned = new HashSet<Platform>();

            foreach(string pathPart in path.Split(System.IO.Path.DirectorySeparatorChar))
            {
                var matches = ParentGameLibrary.Platforms.GetByName(pathPart);

                foreach (var match in matches)
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
                foreach (var game in ParentGameLibrary.Games)
                {
                    game.Platform = null;
                }
            }
            base.Clear();
        }
    }
}

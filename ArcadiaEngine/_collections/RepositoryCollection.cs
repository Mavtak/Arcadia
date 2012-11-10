using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class RepositoryCollection : GenericLibraryItemCollection<Repository>
    {
        public string RootPath { get; set; }

        public RepositoryCollection(ArcadiaLibrary parentLibrary)
            : base(parentLibrary)
        { }


        #region xml

        protected override void writeToXmlExtension(System.Xml.XmlWriter writer)
        {
            if (RootPathIsSet)
            {
                writer.WriteAttributeString("rootPath", RootPath);
            }
        }

        protected override void readFromXmlExtension(System.Xml.XmlNode node)
        {
            if (node.Attributes["rootPath"] != null)
            {
                RootPath = node.Attributes["rootPath"].Value;
            }
        }

        #endregion

        #region root path

        public bool RootPathIsSet
        {
            get
            {
                return !String.IsNullOrEmpty(RootPath);
            }
        }

        public RepositoryType Type
        {
            get
            {
                if (!RootPathIsSet)
                {
                    return RepositoryType.SimpleList;
                }

                if (RootPath.StartsWith("http:\\", StringComparison.InvariantCultureIgnoreCase))
                {
                    return RepositoryType.HTTP;
                }

                if (RootPath.Substring(1, 1) == ":")
                {
                    return RepositoryType.SystemIO;
                }

                return RepositoryType.Unknown;
            }
        }

        public bool RootPathExists
        {
            get
            {
                switch (Type)
                {
                    case RepositoryType.HTTP:
                        throw new Exception("Not supported funcionality");

                    case RepositoryType.SimpleList:
                        return true;

                    case RepositoryType.SystemIO:
                        return System.IO.Directory.Exists(RootPath);

                    case RepositoryType.Unknown:
                        return false;

                    default:
                        throw new Exception("Unknown Error");
                }
            }
        }

        #endregion

        public override bool Remove(Repository item)
        {
            lock (ParentGameLibrary)
            {
                var games = from game in ParentGameLibrary.Games
                              where game.Repository == item
                              select game;

                //TODO: avoid calling ToList()
                foreach (var game in games.ToList())
                {
                    ParentGameLibrary.Remove(game);
                }

                return base.Remove(item);
            }
        }

        public override void Clear()
        {
            ParentGameLibrary.Games.Clear();
            base.Clear();
        }
    }
}

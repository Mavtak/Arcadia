using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class RepositoryCollection : GenericLibraryItemCollection<Repository>
    {
        private string rootPath;

        public RepositoryCollection(ArcadiaLibrary parentLibrary)
            : base(parentLibrary)
        {
        }


        #region xml

        protected override void writeToXmlExtension(System.Xml.XmlWriter writer)
        {
            if (RootPathIsSet)
                writer.WriteAttributeString("rootPath", rootPath);
        }

        protected override void readFromXmlExtension(System.Xml.XmlNode node)
        {
            if (node.Attributes["rootPath"] != null)
                rootPath = node.Attributes["rootPath"].Value;
        }

        #endregion

        #region root path

        public bool RootPathIsSet
        {
            get
            {
                return !String.IsNullOrEmpty(rootPath);
            }
        }

        public string RootPath
        {
            get
            {
                return rootPath;
            }
            set
            {
                rootPath = value;
            }
        }

        public RepositoryType Type
        {
            get
            {
                if (!RootPathIsSet)
                    return RepositoryType.SimpleList;
                if (rootPath.StartsWith("http:\\", StringComparison.InvariantCultureIgnoreCase))
                    return RepositoryType.HTTP;
                if (rootPath.Substring(1, 1) == ":")
                    return RepositoryType.SystemIO;
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
                        return System.IO.Directory.Exists(rootPath);

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
                for (int x = 0; x < ParentGameLibrary.Games.Count; x++)
                {
                    Game game = ParentGameLibrary.Games[x];
                    if (game.Repository == item)
                    {
                        ParentGameLibrary.Remove(game);
                        x--;
                    }
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

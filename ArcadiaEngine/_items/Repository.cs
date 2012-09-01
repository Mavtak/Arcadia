using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class Repository : GenericLibraryItem
    {
        private string rootPath;

        #region constructors

        public Repository(string name, ArcadiaLibrary parentGameLibrary)
            : base(name, parentGameLibrary)
        { }
        public Repository(string name)
            : this(name, null)
        { }
        public Repository(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        { }
        public Repository()
            : this(null, null)
        { }

        #endregion

        #region xml
        protected override void writeToXmlExtension(System.Xml.XmlWriter writer)
        {
            if (RootPathIsSet)
                writer.WriteAttributeString("rootPath", RootPath);
        }

        protected override void readFromXmlExtension(System.Xml.XmlNode node)
        {
            if (node.Attributes["rootPath"] != null)
                RootPath = node.Attributes["rootPath"].Value;
        }


        #endregion

        #region path

        public bool RootPathIsSet
        {
            get
            {
                return !string.IsNullOrEmpty(rootPath);
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
                if (rootPath.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                    return RepositoryType.HTTP;
                //if (rootPath.Substring(1, 1).Equals(":") || rootPath.StartsWith(@"\\"))
                    return RepositoryType.SystemIO;
                //return RepositoryType.Unknown;
            }
        }

        #endregion

        public int GameCount
        {
            get
            {
                int result = 0;
                foreach (Game game in ParentGameLibrary.Games)
                    if (game.Repository == this)
                        result++;
                return result;
            }

        }

        #region discovery

        public bool CanScanForNewGames
        {
            get
            {
                return Type == RepositoryType.SystemIO;
            }
        }

        public List<Platform> PossiblePlatforms
        {
            get
            {
                return ParentGameLibrary.Platforms.FindPlatformsByFileDirectory(RootPath);
            }
        }

        public List<Game> ScanForNewGames()
        {
            //TODO: Speed this up!


            if (!CanScanForNewGames)
                throw new Exception("Can't scan for games in the \"" + Name + "\" repository.");

            List<Game> result = new List<Game>();

            foreach (String filePath in System.IO.Directory.GetFiles(RootPath, "*", System.IO.SearchOption.AllDirectories))
            {
                string extension = System.IO.Path.GetExtension(filePath).Replace(".", "");
                if (ParentGameLibrary.Platforms.CheckIfIsRomExtension(extension) && !ParentGameLibrary.Games.ContainsPath(filePath))
                {
                    Game addedGame = AddGame(filePath);
                    result.Add(addedGame);
                }
            }

            return result;
        }

        public Game AddGame(string gamePath)
        {
            if (!gamePath.StartsWith(RootPath, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception("Game " + gamePath + " is not in the domain of this repository.");

            Game game = new Game(System.IO.Path.GetFileName(gamePath), ParentGameLibrary);
            game.Repository = this;
            game.InnerPath = gamePath.Substring(RootPath.Length);

            game.FillInInformation(true);

            ParentGameLibrary.Add(game);

            return game;
        }

        

        #endregion

    }
}

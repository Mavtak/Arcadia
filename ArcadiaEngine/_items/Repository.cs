using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                {
                    return RepositoryType.SimpleList;
                }

                if (rootPath.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                {
                    return RepositoryType.HTTP;
                }

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
                var matches = from game in ParentGameLibrary.Games
                              where game.Repository == this
                              select game;

                var result = matches.Count();

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

        public IEnumerable<Platform> PossiblePlatforms
        {
            get
            {
                return ParentGameLibrary.Platforms.FindPlatformsByFileDirectory(RootPath);
            }
        }

        public IEnumerable<Game> ScanForNewGames()
        {
            //TODO: Speed this up!

            if (!CanScanForNewGames)
            {
                //TODO: throw more specific exception
                throw new Exception("Can't scan for games in the \"" + Name + "\" repository.");
            }

            var files = Directory.EnumerateFiles(RootPath, "*", SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                string extension = System.IO.Path.GetExtension(filePath).Replace(".", "");
                if (ParentGameLibrary.Platforms.CheckIfIsRomExtension(extension) && !ParentGameLibrary.Games.ContainsPath(filePath))
                {
                    var addedGame = AddGame(filePath);
                    yield return addedGame;
                }
            }
        }

        public Game AddGame(string gamePath)
        {
            if (!gamePath.StartsWith(RootPath, StringComparison.InvariantCultureIgnoreCase))
            {
                //TODO: throw more specific exception
                throw new Exception("Game " + gamePath + " is not in the domain of this repository.");
            }

            var game = new Game(System.IO.Path.GetFileName(gamePath), ParentGameLibrary)
            {
                Repository = this,
                InnerPath = gamePath.Substring(RootPath.Length)
            };

            game.FillInInformation(true);

            ParentGameLibrary.Add(game);

            return game;
        }

        #endregion

    }
}

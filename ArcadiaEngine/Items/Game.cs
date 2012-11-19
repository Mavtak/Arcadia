using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SomewhatGeeky.Arcadia.Engine.Items
{
    public class Game : GenericLibraryItem
    {
        private Language language;
        private Platform platform;
        private NumberRange players;
        private Repository repository;

        private string innerPath;

        #region constructors

        public Game(string name, ArcadiaLibrary parentGameLibrary)
            : base(name, parentGameLibrary)
        { }

        public Game(string name, Repository repository)
            : base(name, repository.ParentGameLibrary)
        {
            this.repository = repository;
        }

        public Game(string name)
            : this(name, (ArcadiaLibrary)null)
        { }
        public Game(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        { }
        public Game()
            : this(null, (ArcadiaLibrary)null)
        { }

        #endregion

        #region xml

        protected override void writeToXmlExtension(System.Xml.XmlWriter writer)
        {
            if (LanguageIsSet)
                writer.WriteAttributeString("languageId", language.Id);
            if (PlatformIsSet)
                writer.WriteAttributeString("platformId", platform.Id);
            if (PlayersIsSet)
                writer.WriteAttributeString("players", players.ToString());
            if (RepositoryIsSet)
                writer.WriteAttributeString("repositoryId", repository.Id);

            if(!String.IsNullOrEmpty(innerPath))
                writer.WriteAttributeString("innerPath", innerPath);
        }

        protected override void readFromXmlExtension(System.Xml.XmlNode node)
        {
            if (node.Attributes["languageId"] != null)
                Language = ParentGameLibrary.Languages[node.Attributes["languageId"].Value];
            if (node.Attributes["platformId"] != null)
                Platform = ParentGameLibrary.Platforms[node.Attributes["platformId"].Value];
            if (node.Attributes["players"] != null)
                players = new NumberRange(node.Attributes["players"].Value);
            if (node.Attributes["repositoryId"] != null)
                Repository = ParentGameLibrary.Repositories[node.Attributes["repositoryId"].Value];

            if (node.Attributes["innerPath"] != null)
                innerPath = node.Attributes["innerPath"].Value;
        }


        #endregion

        #region platform

        public bool PlatformIsSet
        {
            get
            {
                return platform != null;
            }
        }
        public Platform Platform
        {
            get
            {
                return platform;
            }
            set
            {
                platform = value;
            }
        }

        #endregion

        #region Language

        public bool LanguageIsSet
        {
            get
            {
                return language != null;
            }
        }
        public Language Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
            }
        }

        #endregion

        #region Repository

        public bool RepositoryIsSet
        {
            get
            {
                return repository != null;
            }
        }
        public Repository Repository
        {
            get
            {
                return repository;
            }
            set
            {
                repository = value;
            }
        }

        #endregion

        #region players

        public bool PlayersIsSet
        {
            get
            {
                return players != null && players.IsSet;
            }
        }
        public NumberRange Players
        {
            get
            {
                if (players == null)
                    players = new NumberRange();
                return players;
            }
            set
            {
                players = value;
            }
        }

        #endregion

        #region path

        public bool InnerPathIsSet
        {
            get
            {
                return !String.IsNullOrEmpty(innerPath);
            }
        }
        public string InnerPath
        {
            get
            {
                return innerPath;
            }
            set
            {
                innerPath = value;
            }
        }
        public string Path
        {
            get
            {
                if (InnerPathIsSet && RepositoryIsSet && Repository.RootPathIsSet)
                {
                    return Repository.RootPath + InnerPath;
                }

                return null;
            }
        }
        public string FullPath
        {
            get
            {
                string path = Path;

                if (path == null)
                {
                    return null;
                }

                if (path.StartsWith(@"\\") || path.Substring(1, 1).Equals(":"))
                {
                    return path;
                }

                return System.IO.Path.GetFullPath(path);
            }
        }

        #endregion

        #region flags

        private static Regex fileFlagsRegex;
        
        private static void initializeFileFlagsRegex()
        {
            if (fileFlagsRegex == null)
                fileFlagsRegex = new Regex(@"[{] [\w\s!.]*? [}]  |  [(] [\w\s!.]*? [)]  |  [[] [\w\s!.]*? []]", RegexOptions.IgnorePatternWhitespace);
        }

        public bool FilenameFlagsAreSet
        {
            get
            {
                List<string> results = new List<string>();

                if (!InnerPathIsSet)
                    return false;

                initializeFileFlagsRegex();

                return fileFlagsRegex.IsMatch(innerPath);
            }
        }

        public List<string> FilenameFlags
        {
            get
            {
                List<string> results = new List<string>();

                if (!InnerPathIsSet)
                    return results;

                initializeFileFlagsRegex();

                foreach (Capture capture in fileFlagsRegex.Matches(System.IO.Path.GetFileName(innerPath)))
                    results.Add(capture.Value);

                return results;
            }
        }

        #endregion

        public void FillInInformation(bool overwrite)
        {
            //pretty up the title.
            if (overwrite || !NameIsSet)
            {
                //remove extension from name
                if (Name.Contains('.'))
                    Name = Name.Substring(0, Name.LastIndexOf('.'));

                //remove file flags
                foreach (string filenameFlag in FilenameFlags)
                    Name = Name.Replace(filenameFlag, "");
                while (Name.Contains("  "))
                    Name = Name.Replace("  ", " ");

                //trim trim!
                Name = Name.Trim();
            }

            if (overwrite || !PlatformIsSet)
            {
                var platformPossibilities = ParentGameLibrary.Platforms.FindPlatforms(FullPath).ToList();
                if (platformPossibilities.Count == 1)
                {
                    //TODO: What if there are multiple matches?
                    Platform = platformPossibilities[0];
                }
            }
        }
    }
}

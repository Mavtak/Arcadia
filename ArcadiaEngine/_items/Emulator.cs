using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class Emulator : GenericLibraryItem
    {
        private string path;
        private string argumentPattern;
        private List<Platform> compatablePlatforms;

        #region constructors

        public Emulator(string name, ArcadiaLibrary parentGameLibrary)
            : base(name, parentGameLibrary)
        {
            compatablePlatforms = new List<Platform>();
        }
        public Emulator(string name)
            : this(name, null)
        { }
        public Emulator(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        { }
        public Emulator()
            : this(null, null)
        { }

        #endregion

        #region xml
        protected override void writeToXmlExtension(System.Xml.XmlWriter writer)
        {
            //write path
            if (PathIsSet)
                writer.WriteAttributeString("path", Path);

            //write argument pattern
            if (ArgumentPatternIsSet)
                writer.WriteAttributeString("argumentPattern", ArgumentPattern);

            //write platforms
            if (CompatablePlatforms.Count > 0)
            {
                writer.WriteStartElement("compatablePlatforms");

                foreach (Platform platform in CompatablePlatforms)
                    writer.WriteElementString("platformId", platform.Id);

                writer.WriteEndElement();
            }
        }

        protected override void readFromXmlExtension(System.Xml.XmlNode node)
        {
            //read path
            if (node.Attributes["path"] != null)
                Path = node.Attributes["path"].Value;

            //read argument pattern
            if (node.Attributes["argumentPattern"] != null)
                ArgumentPattern = node.Attributes["argumentPattern"].Value;

            //read platforms
            if (node.SelectSingleNode("compatablePlatforms") != null)
                foreach (XmlNode platformIdNode in node.SelectSingleNode("compatablePlatforms"))
                    compatablePlatforms.Add(ParentGameLibrary.Platforms[platformIdNode.InnerXml]);
        }
        
        #endregion

        #region path

        public bool PathIsSet
        {
            get
            {
                return !string.IsNullOrEmpty(path);
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        public string FullPath
        {
            get
            {
                return System.IO.Path.GetFullPath(Path);
            }
        }
        public bool FileExists
        {
            get
            {
                if (!PathIsSet)
                    return false;
                return System.IO.File.Exists(path);
            }
        }

        #endregion

        #region platform

        public List<Platform> CompatablePlatforms
        {
            get
            {
                return compatablePlatforms;
            }
            set
            {
                compatablePlatforms = value;
            }
        }

        #endregion

        #region play

        #region argument pattern
        public static string ArgumentPatternFileVariable
        {
            get
            {
                return "$(FilePath)";
            }
        }
        public static string DefaultArgumentPattern
        {
            get
            {
                return "\"" + ArgumentPatternFileVariable + "\"";
            }
        }
        public string ArgumentPattern
        {
            get
            {
                return argumentPattern;
            }
            set
            {
                argumentPattern = value;
            }
        }
        public bool ArgumentPatternIsSet
        {
            get
            {
                return !string.IsNullOrEmpty(argumentPattern);
            }
        }

        #endregion

        public void OpenGame(Game game)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = this.Path;

            string pattern;
            if (ArgumentPatternIsSet)
                pattern = ArgumentPattern;
            else
                pattern = DefaultArgumentPattern;

            process.StartInfo.Arguments = pattern.Replace(ArgumentPatternFileVariable, game.FullPath);

            try
            {
                process.Start();
            }
            catch
            {
                throw new Exception("Error starting emulator.");
            }
        }

        #endregion

    }
}

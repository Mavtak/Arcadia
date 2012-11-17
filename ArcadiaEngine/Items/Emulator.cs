using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class Emulator : GenericLibraryItem
    {
        public const string ArgumentPatternFileVariable = "$(FilePath)";
        public const string DefaultArgumentPattern = "\"" + ArgumentPatternFileVariable + "\"";

        public string Path { get; set; }
        public string ArgumentPattern { get; set; }
        public ICollection<Platform> CompatablePlatforms { get; private set; }

        #region constructors

        public Emulator(string name = null, ArcadiaLibrary parentGameLibrary = null)
            : base(name, parentGameLibrary)
        {
            CompatablePlatforms = new List<Platform>();
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

        protected override void writeToXmlExtension(XmlWriter writer)
        {
            //write path
            if (PathIsSet)
            {
                writer.WriteAttributeString("path", Path);
            }

            //write argument pattern
            if (ArgumentPatternIsSet)
            {
                writer.WriteAttributeString("argumentPattern", ArgumentPattern);
            }

            //write platforms
            if (CompatablePlatforms.Count > 0)
            {
                writer.WriteStartElement("compatablePlatforms");

                foreach (var platform in CompatablePlatforms)
                {
                    writer.WriteElementString("platformId", platform.Id);
                }

                writer.WriteEndElement();
            }
        }

        protected override void readFromXmlExtension(XmlNode node)
        {
            //read path
            if (node.Attributes["path"] != null)
            {
                Path = node.Attributes["path"].Value;
            }

            //read argument pattern
            if (node.Attributes["argumentPattern"] != null)
            {
                ArgumentPattern = node.Attributes["argumentPattern"].Value;
            }

            //read platforms
            var platformIds = new LinkedList<string>();
            Common.ReadListOfStringsFromXml(node.SelectSingleNode("compatablePlatforms"), "platformId", platformIds);
            foreach (var platformId in platformIds)
            {
                CompatablePlatforms.Add(ParentGameLibrary.Platforms[platformId]);
            }
        }
        
        #endregion

        #region path

        public bool PathIsSet
        {
            get
            {
                return !string.IsNullOrEmpty(Path);
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
                {
                    return false;
                }

                return File.Exists(Path);
            }
        }

        #endregion

        #region play

        public bool ArgumentPatternIsSet
        {
            get
            {
                return !string.IsNullOrEmpty(ArgumentPattern);
            }
        }

        public void OpenGame(Game game)
        {
            var process = new Process();
            process.StartInfo.FileName = this.Path;

            var pattern = ArgumentPatternIsSet ? ArgumentPattern : DefaultArgumentPattern;

            process.StartInfo.Arguments = pattern.Replace(ArgumentPatternFileVariable, game.FullPath);

            try
            {
                process.Start();
            }
            catch(Exception exception)
            {
                //TODO: throw more specific exception
                throw new Exception("Error starting emulator.", exception);
            }
        }

        #endregion

    }
}

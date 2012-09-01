using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class ArcadiaLibrary //TODO : IEnumerable<GenericLibraryItem>//, IEnumerable
    {
        private EmulatorCollection emulatorCollection;
        private GameCollection gameCollection;
        private LanguageCollection languageCollection;
        private PlatformCollection platformCollection;
        private RepositoryCollection repositoryCollection;
        private LibrarySearcher searcher;
        public ArcadiaLibrary()
        {
            emulatorCollection = new EmulatorCollection(this);
            gameCollection = new GameCollection(this);
            languageCollection = new LanguageCollection(this);
            platformCollection = new PlatformCollection(this);
            repositoryCollection = new RepositoryCollection(this);
            searcher = new LibrarySearcher(this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            //settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            XmlWriter writer = XmlWriter.Create(builder, settings);
            lock (this)
            {
                this.WriteToXml(writer, "ArcadiaLibrary");
            }
            writer.Close();
            return builder.ToString();
        }

        #region xml

        public void WriteToXml(XmlWriter writer, string nodeName)
        {
            writer.WriteStartElement(nodeName);

            platformCollection.WriteToXml(writer);
            languageCollection.WriteToXml(writer);
            emulatorCollection.WriteToXml(writer);
            repositoryCollection.WriteToXml(writer);
            gameCollection.WriteToXml(writer);

            writer.WriteEndElement();
        }

        public void ReadFromXml(XmlNode node)
        {
            if(node == null)
                return;

            platformCollection.ReadFromXml(node.SelectSingleNode("platformCollection"));
            languageCollection.ReadFromXml(node.SelectSingleNode("languageCollection"));
            emulatorCollection.ReadFromXml(node.SelectSingleNode("emulatorCollection"));
            repositoryCollection.ReadFromXml(node.SelectSingleNode("repositoryCollection"));
            gameCollection.ReadFromXml(node.SelectSingleNode("gameCollection"));
        }

        public string DefaultXmlNodeName
        {
            get
            {
                string fullName = this.GetType().FullName;
                string lastPart = fullName.Substring(fullName.LastIndexOf(".") + 1);
                return lastPart.Substring(0, 1).ToLower() + lastPart.Substring(1);
            }
        }

        public void WriteToFile(string path)
        {
            string tempPath = path + ".tmp";

            XmlWriter writer = XmlWriter.Create(tempPath);
            writer.WriteStartDocument();
            WriteToXml(writer, DefaultXmlNodeName);
            writer.WriteEndDocument();
            writer.Close();

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            System.IO.File.Move(tempPath, path);
        }

        public void ReadFromFile(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            ReadFromXml(xmlDocument.LastChild);
        }

        public void WriteDefaultSettings(XmlWriter writer, string nodeName)
        {
            writer.WriteStartElement(nodeName);

            platformCollection.WriteToXml(writer);
            languageCollection.WriteToXml(writer);

            writer.WriteEndElement();
        }
        public void WriteDefaultSettings(XmlWriter writer)
        {
            WriteDefaultSettings(writer, DefaultXmlNodeName);
        }
        public void LoadDefaultSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string settings = Properties.Resources.DefaultSettings;

            xmlDoc.LoadXml(settings);
            XmlNode rootNode = xmlDoc.ChildNodes[xmlDoc.ChildNodes.Count - 1];
            ReadFromXml(rootNode);
        }
        #endregion

        #region accessors

        public PlatformCollection Platforms
        {
            get
            {
                return platformCollection;
            }
        }

        public LanguageCollection Languages
        {
            get
            {
                return languageCollection;
            }
        }

        public EmulatorCollection Emulators
        {
            get
            {
                return emulatorCollection;
            }
        }

        public GameCollection Games
        {
            get
            {
                return gameCollection;
            }
        }

        public RepositoryCollection Repositories
        {
            get
            {
                return repositoryCollection;
            }
        }

        public LibrarySearcher Searcher
        {
            get
            {
                return searcher;
            }
        }

        #endregion

        #region add

        public void Add(Emulator emulator)
        {
            Emulators.Add(emulator);
        }
        public void Add(Game game)
        {
            Games.Add(game);
        }
        public void Add(Language language)
        {
            Languages.Add(language);
        }
        public void Add(Platform platform)
        {
            Platforms.Add(platform);
        }
        public void Add(Repository repository)
        {
            Repositories.Add(repository);
        }

        #endregion

        #region contains

        public bool Contains(Emulator emulator)
        {
            return Emulators.Contains(emulator);
        }
        public bool Contains(Game game)
        {
            return Games.Contains(game);
        }
        public bool Contains(Language language)
        {
           return Languages.Contains(language);
        }
        public bool Contains(Platform platform)
        {
            return Platforms.Contains(platform);
        }
        public bool Contains(Repository repository)
        {
            return Repositories.Contains(repository);
        }

        #endregion


        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return null;
        //}
        //IEnumerator<GenericLibraryItem> IEnumerable<GenericLibraryItem>.GetEnumerator()
        //{
        //    foreach (GenericLibraryItem item in Emulators)
        //        yield return item;
        //}
        //TODO:

        #region remove

        public void Remove(Emulator emulator)
        {
            Emulators.Remove(emulator);
        }
        public void Remove(Game game)
        {
            Games.Remove(game);
        }
        public void Remove(Language language)
        {
            Languages.Remove(language);
        }
        public void Remove(Platform platform)
        {
            Platforms.Remove(platform);
        }
        public void Remove(Repository repository)
        {
            Repositories.Remove(repository);
        }

        #endregion
    }
}

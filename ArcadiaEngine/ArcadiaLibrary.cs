using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

namespace SomewhatGeeky.Arcadia.Engine
{
    [System.Runtime.InteropServices.GuidAttribute("1A2B6E5D-AB5B-4BA9-8563-4A8E9A403D5B")]
    public class ArcadiaLibrary : ICollection<GenericLibraryItem>
    {
        public EmulatorCollection Emulators { get; private set; }
        public GameCollection Games { get; private set; }
        public LanguageCollection Languages { get; private set; }
        public PlatformCollection Platforms { get; private set; }
        public RepositoryCollection Repositories { get; private set; }
        public LibrarySearcher Searcher { get; private set; }
        internal IdGenerator IdGenerator { get; private set; }

        public ArcadiaLibrary()
        {
            Emulators = new EmulatorCollection(this);
            Games = new GameCollection(this);
            Languages = new LanguageCollection(this);
            Platforms = new PlatformCollection(this);
            Repositories = new RepositoryCollection(this);
            Searcher = new LibrarySearcher(this);
            IdGenerator = new IdGenerator();
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

            Platforms.WriteToXml(writer);
            Languages.WriteToXml(writer);
            Emulators.WriteToXml(writer);
            Repositories.WriteToXml(writer);
            Games.WriteToXml(writer);

            writer.WriteEndElement();
        }

        public void ReadFromXml(XmlNode node)
        {
            if(node == null)
                return;

            Platforms.ReadFromXml(node.SelectSingleNode("platformCollection"));
            Languages.ReadFromXml(node.SelectSingleNode("languageCollection"));
            Emulators.ReadFromXml(node.SelectSingleNode("emulatorCollection"));
            Repositories.ReadFromXml(node.SelectSingleNode("repositoryCollection"));
            Games.ReadFromXml(node.SelectSingleNode("gameCollection"));
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

            Platforms.WriteToXml(writer);
            Languages.WriteToXml(writer);

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

        public IEnumerable<Game> ScanForNewGames()
        {
            foreach (var repository in Repositories)
            {
                foreach (var game in repository.ScanForNewGames())
                {
                    yield return game;
                }
            }
        }

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

        public void Clear()
        {
            Emulators.Clear();
            Games.Clear();
            Languages.Clear();
            Platforms.Clear();
            Repositories.Clear();
        }

        #region remove

        public bool Remove(Emulator emulator)
        {
            return Emulators.Remove(emulator);
        }
        public bool Remove(Game game)
        {
            return Games.Remove(game);
        }
        public bool Remove(Language language)
        {
            return Languages.Remove(language);
        }
        public bool Remove(Platform platform)
        {
            return Platforms.Remove(platform);
        }
        public bool Remove(Repository repository)
        {
            return Repositories.Remove(repository);
        }

        #endregion

        #region ICollection

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        public IEnumerable<GenericLibraryItem> AllItems
        {
            get
            {
                var result = ((IEnumerable<GenericLibraryItem>)Emulators)
                        .Concat((IEnumerable<GenericLibraryItem>)Games)
                        .Concat((IEnumerable<GenericLibraryItem>)Languages)
                        .Concat((IEnumerable<GenericLibraryItem>)Platforms)
                        .Concat((IEnumerable<GenericLibraryItem>)Repositories);

                return result;
            }
        }

        #region IEnumerator<GenericLibraryItem>

        IEnumerator<GenericLibraryItem> IEnumerable<GenericLibraryItem>.GetEnumerator()
        {
            return AllItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<GenericLibraryItem>)this).GetEnumerator();
        }

        #endregion

        #region ICollection<GenericLibraryItem>

        void ICollection<GenericLibraryItem>.Add(GenericLibraryItem item)
        {
            if (item is Emulator)
            {
                Add((Emulator)item);
            }

            if (item is Game)
            {
                Add((Game)item);
            }

            if (item is Language)
            {
                Add((Language)item);
            }

            if (item is Platform)
            {
                Add((Platform)item);
            }

            if (item is Repository)
            {
                Add((Repository)item);
            }
        }

        void ICollection<GenericLibraryItem>.Clear()
        {
            Clear();
        }

        bool ICollection<GenericLibraryItem>.Contains(GenericLibraryItem item)
        {
            if (item is Emulator)
            {
                return Contains((Emulator)item);
            }

            if (item is Game)
            {
                return Contains((Game)item);
            }

            if (item is Language)
            {
                return Contains((Language)item);
            }

            if (item is Platform)
            {
                return Contains((Platform)item);
            }

            if (item is Repository)
            {
                return Contains((Repository)item);
            }

            return false;
        }

        void ICollection<GenericLibraryItem>.CopyTo(GenericLibraryItem[] array, int arrayIndex)
        {
            ((ICollection<GenericLibraryItem>)this).CopyTo(array, arrayIndex);
        }

        int ICollection<GenericLibraryItem>.Count
        {
            get
            {
                return AllItems.Count();
            }
        }

        bool ICollection<GenericLibraryItem>.Remove(GenericLibraryItem item)
        {
            if (item is Emulator)
            {
                return Remove((Emulator)item);
            }

            if (item is Game)
            {
                return Remove((Game)item);
            }

            if (item is Language)
            {
                return Remove((Language)item);
            }

            if (item is Platform)
            {
                return Remove((Platform)item);
            }

            if (item is Repository)
            {
                return Remove((Repository)item);
            }

            return false;
        }

        #endregion

    }
}

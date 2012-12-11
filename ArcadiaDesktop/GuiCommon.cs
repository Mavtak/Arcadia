using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Desktop
{
    static class GuiCommon
    {
        public static String MainWindowBaseTitle
        {
            get
            {
                return "Arcadia Preview 3";
            }
        }

        

        public static Version Version
        {
            get
            {
                return InternalLibraryVersion.GetLibraryVersion();
                //System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
            }
        }

        public static DateTime? BuildTime
        {
            get
            {
                return InternalLibraryVersion.LastBuildTime;
            }
        }

        private static string GuiSaveElementName
        {
            get
            {
                return "Arcadia.Desktop";
            }
        }

        private static string GuiSavePath
        {
            get
            {
                return "Arcadia.Desktop.xml";
            }
        }

        public static string LibraryFilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arcadia.xml");
            }
        }

        public static void Save(MainWindow window)
        {
            var root = new XElement(GuiSaveElementName);

            if(!String.IsNullOrEmpty(window.searchBox.Text))
            {
                var search = new XElement("SearchBox", window.searchBox.Text);
                root.Add(search);
            }

            root.Save(GuiSavePath);
        }

        public static void Load(MainWindow window)
        {
            if (!File.Exists(GuiSavePath))
            {
                return;
            }

            var root = XElement.Load(GuiSavePath);

            var searchBoxNode = root.Descendants("SearchBox").FirstOrDefault();
            if (searchBoxNode != null)
            {
                window.searchBox.Text = searchBoxNode.Value;
            }
        }

        public static void PlayGame(Window owner, Game game, Func<string> getStatus = null, Action<string> setStatus = null)
        {
            if (game == null)
            {
                return;
            }

            string oldStatus = null;

            if (getStatus != null && setStatus != null)
            {
                oldStatus = getStatus();
                setStatus("starting " + game.Name);
            }

            try
            {
                var library = game.ParentGameLibrary;

                Emulator emulator = null;
                var emulatorOptions = library.Emulators.GetEmulatorChoices(game.Platform).ToList();


                if (emulatorOptions.Count == 1)
                {
                    emulator = emulatorOptions[0];
                }
                else
                {
                    if (emulatorOptions.Count == 0)
                        emulatorOptions = library.Emulators.ToList();

                    var dialog = new EmulatorSelectorWindow(owner, library.Emulators, emulatorOptions);
                    dialog.ShowDialog();

                    emulator = dialog.Result;
                }

                if (emulator == null)
                {
                    return;
                }

                emulator.OpenGame(game);
            }
            finally
            {
                if (setStatus != null && oldStatus != null)
                {
                    setStatus(oldStatus);
                }
            }
        }
    }
}

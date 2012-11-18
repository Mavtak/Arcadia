using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace SomewhatGeeky.Arcadia.Desktop
{
    static class GuiCommon
    {
        public static String MainWindowBaseTitle
        {
            get
            {
                return "Arcadia Preview 2";
            }
        }
        public static MessageBoxResult ShowMessageBox(string message, MessageBoxButton button, MessageBoxImage image)
        {
            return MessageBox.Show(message, MainWindowBaseTitle, button, image);
        }
        public static MessageBoxResult ShowMessageBox(string message)
        {
            return ShowMessageBox(message, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowCriticalErrorMessageBox(Exception exception)
        {
            return ShowMessageBox(
                "An unanticipated error has occured.  Please email this error to me at " + Arcadia.Engine.Common.ContactEmail
                +"\nCopy the contents of this window to the clipboard by pressing ctrl+c."
                + "\n\n\n" + exception.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static Version Version
        {
            get
            {
                return InternalLibraryVersion.GetLibraryVersion();
                //System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
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
    }
}

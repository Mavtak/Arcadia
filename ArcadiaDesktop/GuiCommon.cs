using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;

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
    }
}

using System;
using System.Text;
using System.Windows;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public class MessageBoxGenerator
    {
        public static MessageBoxResult ShowMessageBox(string message, MessageBoxButton button, MessageBoxImage image)
        {
            return MessageBox.Show(message, GuiCommon.MainWindowBaseTitle, button, image);
        }

        public static MessageBoxResult ShowMessageBox(string message)
        {
            return ShowMessageBox(message, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowCriticalErrorMessageBox(Exception exception)
        {
            var builder = new StringBuilder();
            builder.Append("An unanticipated error has occured.  Please email this error to me at ");
            builder.Append(builder+ Arcadia.Engine.Common.ContactEmail);
            builder.Append("\nCopy the contents of this window to the clipboard by pressing ctrl+c.");
            builder.Append("\n\n\n");
            builder.Append(exception.ToString());
            
            var message = builder.ToString();
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            ShowMessageBox(message, options, icon);
        }
    }
}

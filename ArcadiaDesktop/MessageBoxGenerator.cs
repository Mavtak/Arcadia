using System;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public class MessageBoxGenerator
    {
        public static MessageBoxResult ShowMessageBox(string message, MessageBoxButton button, MessageBoxImage image)
        {
            System.Windows.Forms.Application.EnableVisualStyles();

            var title = GuiCommon.MainWindowBaseTitle;
            var formsButton = ConvertOptions(button);
            var formsIcon = ConvertIcon(image);
            
            var formsResult = System.Windows.Forms.MessageBox.Show(
                caption: title,
                text: message,
                buttons: formsButton,
                icon: formsIcon
                );

            var result = ConvertResult(formsResult);

            return result;
        }

        public static MessageBoxResult ShowMessageBox(string message)
        {
            return ShowMessageBox(message, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowError(string message)
        {
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            ShowMessageBox(message, options, icon);
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

        private static MessageBoxButtons ConvertOptions(MessageBoxButton options)
        {
            switch (options)
            {
                case MessageBoxButton.OKCancel:
                    return MessageBoxButtons.OKCancel;

                case MessageBoxButton.YesNo:
                    return MessageBoxButtons.YesNo;

                case MessageBoxButton.YesNoCancel:
                    return MessageBoxButtons.YesNoCancel;

                case MessageBoxButton.OK:
                default:
                    return MessageBoxButtons.OK;
            }
        }

        private static MessageBoxIcon ConvertIcon(MessageBoxImage icon)
        {
            switch(icon)
            {
                case MessageBoxImage.Error:
                    return MessageBoxIcon.Error;

                case MessageBoxImage.Information:
                    return MessageBoxIcon.Information;

                case MessageBoxImage.None:
                    return MessageBoxIcon.None;

                case MessageBoxImage.Question:
                    return MessageBoxIcon.Question;

                case MessageBoxImage.Warning:
                    return MessageBoxIcon.Warning;

                default:
                    return MessageBoxIcon.None;
            }
        }

        private static MessageBoxResult ConvertResult(DialogResult formsResult)
        {
            switch (formsResult)
            {
                case DialogResult.Abort:
                case DialogResult.Cancel:
                    return MessageBoxResult.Cancel;

                case DialogResult.No:
                    return MessageBoxResult.No;

                case DialogResult.OK:
                    return MessageBoxResult.OK;

                case DialogResult.Yes:
                    return MessageBoxResult.Yes;

                case DialogResult.None:
                case DialogResult.Ignore:
                case DialogResult.Retry:
                default:
                    return MessageBoxResult.None;
            }
        }
    }
}

using System.IO;
using System.Windows;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public class MainWindowMessageBoxGenerator : MessageBoxGenerator
    {
        public static void ShowErrorLoadingDataFileError()
        {
            var message = "Oops!  There was an error loading the data file.  Am I on a read-only file location?";
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            ShowMessageBox(message, options, icon);
        }

        public static void ShowNoProblemsMessage()
        {
            var message = "no problems! :)";
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            ShowMessageBox(message, options, icon);
        }

        public static void ShowScanningError(IOException exception)
        {
            var message = exception.Message;
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            ShowMessageBox(message, options, icon);
        }

        public static MessageBoxResult ShowNoRepositoriesMessage()
        {
            var message = "It looks like you don't have any repositories set.\nHow about you tell me where your ROMs and things are?";
            var options = MessageBoxButton.OKCancel;
            var icon = MessageBoxImage.Question;

            return ShowMessageBox(message, options, icon);
        }

        public static MessageBoxResult ShouldScanForRoms()
        {
            var message = "When you want Arcadia to scan for new ROMs, click \"Settings and Tools\" and then \"Scan For ROMs\".\nDo you want to scan now?";
            var options = MessageBoxButton.YesNo;
            var icon = MessageBoxImage.Question;

            return ShowMessageBox(message, options, icon);
        }
    }
}

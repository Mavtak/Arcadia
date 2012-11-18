using System.Text;
using System.Windows;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public class EmulatorEditorMessageBoxGenerator : MessageBoxGenerator
    {
        public static void ShowEmulatorArgumentPatternError()
        {
            var builder = new StringBuilder();
            builder.Append("The Argument Pattern does not contain ");
            builder.Append(Emulator.ArgumentPatternFileVariable);
            builder.Append(".  It needs that or else the emulator will have no way of knowing what file you want to open.  If you don't know what to do, clear the box for a default value.");

            var message = builder.ToString();

            ShowMessageBox(message);
        }

        public static void ShowEmulatorArgumentPatternHelp()
        {
            var builder = new StringBuilder();
            builder.Append(Emulator.ArgumentPatternFileVariable);
            builder.Append(" represents the file path.  Leave blank for the default value.\n\nMost programs require \"$(FilePath)\" (WITH quotes), but Project 64 requires \"$(FilePath)\" WITHOUT quotes.");

            var message = builder.ToString();
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            ShowMessageBox(message, options, icon);
        }

        public static void ShowEmulatorPlatformHelp()
        {
            var message = "Select multiple platforms by control-clicking.";
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            ShowMessageBox(message, options, icon);
        }
    }
}

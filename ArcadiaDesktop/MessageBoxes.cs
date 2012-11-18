using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SomewhatGeeky.Arcadia.Engine.Items;
using System.IO;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public static class MessageBoxes
    {
        public static void ShowErrorLoadingDataFileError()
        {
            var message = "Oops!  There was an error loading the data file.  Am I on a read-only file location?";
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            GuiCommon.ShowMessageBox(message, options, icon);
        }

        public static void ShowNoProblemsMessage()
        {
            var message = "no problems! :)";
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            GuiCommon.ShowMessageBox(message, options, icon);
        }

        public static void ShowScanningError(IOException exception)
        {
            var message = exception.Message;
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Error;

            GuiCommon.ShowMessageBox(message, options, icon);
        }

        public static MessageBoxResult ShowNoRepositoriesMessage()
        {
            var message = "It looks like you don't have any repositories set.\nHow about you tell me where your ROMs and things are?";
            var options = MessageBoxButton.OKCancel;
            var icon = MessageBoxImage.Question;

            return GuiCommon.ShowMessageBox(message, options, icon);
        }

        public static MessageBoxResult ShouldScanForRoms()
        {
            var message = "When you want Arcadia to scan for new ROMs, click \"Settings and Tools\" and then \"Scan For ROMs\".\nDo you want to scan now?";
            var options = MessageBoxButton.YesNo;
            var icon = MessageBoxImage.Question;

            return GuiCommon.ShowMessageBox(message, options, icon);
        }

        public static void ShowEmulatorArgumentPatternError()
        {
            var builder = new StringBuilder();
            builder.Append("The Argument Pattern does not contain ");
            builder.Append(Emulator.ArgumentPatternFileVariable);
            builder.Append(".  It needs that or else the emulator will have no way of knowing what file you want to open.  If you don't know what to do, clear the box for a default value.");

            var message = builder.ToString();

            GuiCommon.ShowMessageBox(message);
        }

        public static void ShowEmulatorArgumentPatternHelp()
        {
            var builder = new StringBuilder();
            builder.Append(Emulator.ArgumentPatternFileVariable);
            builder.Append(" represents the file path.  Leave blank for the default value.\n\nMost programs require \"$(FilePath)\" (WITH quotes), but Project 64 requires \"$(FilePath)\" WITHOUT quotes.");

            var message = builder.ToString();
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            GuiCommon.ShowMessageBox(message, options, icon);
        }

        public static void ShowEmulatorPlatformHelp()
        {
            var message = "Select multiple platforms by control-clicking.";
            var options = MessageBoxButton.OK;
            var icon = MessageBoxImage.Information;

            GuiCommon.ShowMessageBox(message, options, icon);
        }
    }
}

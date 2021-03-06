﻿using System;
using System.Windows;
using Microsoft.Win32;
using SomewhatGeeky.Arcadia.Engine;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for EmulatorEditorWindow.xaml
    /// </summary>
    public partial class EmulatorEditorWindow : Window
    {
        private Emulator item;

        public EmulatorEditorWindow(Window owner, PlatformCollection platforms, Emulator item)
        {
            InitializeComponent();

            this.Owner = owner;

            this.item = item;

            nameBox.Text = item.Name;

            locationBox.Text = item.Path;

            argumentPatternBox.Text = item.ArgumentPattern;

            platformsList.ItemsSource = platforms;
            foreach(Platform platform in item.CompatablePlatforms)
            {
                int index = platformsList.Items.IndexOf(platform);
                if (index < 0)
                {
                    throw new Exception("David!  What did I say about that lazy programming?");
                }

                platformsList.SelectedItems.Add(platformsList.Items[index]);
            }
        }

        public EmulatorEditorWindow(Window owner, PlatformCollection platforms)
            : this(owner, platforms, new Emulator(platforms.ParentGameLibrary))
        { }

        public Emulator Result
        {
            get
            {
                if (DialogResult.HasValue && DialogResult == true)
                {
                    return item;
                }

                return null;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(argumentPatternBox.Text)
                && !argumentPatternBox.Text.Contains(Emulator.ArgumentPatternFileVariable))
            {
                EmulatorEditorMessageBoxGenerator.ShowEmulatorArgumentPatternError();
                e.Handled = true;
                return;
            }

            DialogResult = true;

            item.Name = nameBox.Text;

            item.Path = locationBox.Text;

            item.ArgumentPattern = argumentPatternBox.Text;

            item.CompatablePlatforms.Clear();
            foreach (Platform platform in platformsList.SelectedItems)
            {
                item.CompatablePlatforms.Add(platform);
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Program Files|*.exe";

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string path = dialog.FileName;

            path = Common.MakeRelativePath(path);

            locationBox.Text = path;

            if (String.IsNullOrEmpty(nameBox.Text))
            {
                nameBox.Text = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(path));
            }
        }

        private void argumentPatternHelpLink_Click(object sender, RoutedEventArgs e)
        {
            EmulatorEditorMessageBoxGenerator.ShowEmulatorArgumentPatternHelp();
        }

        private void platformsHelpLink_Click(object sender, RoutedEventArgs e)
        {
            EmulatorEditorMessageBoxGenerator.ShowEmulatorPlatformHelp();
        }
    }
}

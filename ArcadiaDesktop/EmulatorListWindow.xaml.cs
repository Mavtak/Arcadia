﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SomewhatGeeky.Arcadia.Engine;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for EmulatorList.xaml
    /// </summary>
    public partial class EmulatorListWindow : Window
    {
        private EmulatorCollection collection;

        public EmulatorListWindow(Window owner, EmulatorCollection collection)
        {
            InitializeComponent();

            this.Owner = owner;

            this.collection = collection;
            itemList.ItemsSource = collection;

            itemList_SelectionChanged(this, null);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new EmulatorEditorWindow(this, collection.ParentGameLibrary.Platforms);
            window.ShowDialog();
            if (window.Result == null)
            {
                return;
            }

            collection.Add(window.Result);
            itemList.Items.Refresh();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var item = itemList.SelectedItem as Emulator;
            if (item == null)
            {
                throw new Exception("David, lazy programming fail!");
            }

            var window = new EmulatorEditorWindow(this, collection.ParentGameLibrary.Platforms, item);
            window.ShowDialog();
            itemList.Items.Refresh();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Emulator platform in itemList.SelectedItems)
            {
                collection.Remove(platform);
            }

            itemList.Items.Refresh();
        }

        private void itemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editButton.IsEnabled = itemList.SelectedItems.Count == 1;
            deleteButton.IsEnabled = itemList.SelectedItems.Count > 0;
        }

        private void itemList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && itemList.SelectedItems.Count == 1)
            {
                editButton_Click(sender, null);
            }
        }
        private void itemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (itemList.SelectedItems.Count == 1)
            {
                editButton_Click(sender, null);
            }
        }
    }
}

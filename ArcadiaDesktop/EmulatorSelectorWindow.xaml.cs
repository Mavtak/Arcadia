using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using SomewhatGeeky.Arcadia.Engine;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for EmulatorSelectorWindow.xaml
    /// </summary>
    public partial class EmulatorSelectorWindow : Window
    {
        private EmulatorCollection completeCollection;
        private List<Emulator> options;

        public EmulatorSelectorWindow(Window owner, EmulatorCollection completeCollection, List<Emulator> options)
        {
            InitializeComponent();

            this.Owner = owner;
            this.completeCollection = completeCollection;
            this.options = options;
            itemList.ItemsSource = options;

            itemList_SelectionChanged(this, null);
        }

        public Emulator Result
        {
            get
            {
                if (!DialogResult.HasValue)
                    return null;
                if (DialogResult != true)
                    return null;
                if (itemList.SelectedItems.Count != 1)
                    return null;

                return (Emulator)(itemList.SelectedItem);
            }
        }



        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }


        private void itemList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                && itemList.SelectedItems.Count == 1)
                okButton_Click(sender, null);
        }
        private void itemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (itemList.SelectedItems.Count == 1)
                okButton_Click(sender, null);
        }

        private void itemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            okButton.IsEnabled = itemList.SelectedItems.Count == 1;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void editEmulatorsLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            List<Emulator> existingEmulators = completeCollection.GetValues();
            EmulatorListWindow dialog = new EmulatorListWindow(this, completeCollection);
            dialog.ShowDialog();

            //remove removed emulators
            List<Emulator> toRemove = new List<Emulator>();
            foreach (Emulator emulator in options)
            {
                if(!completeCollection.Contains(emulator))
                    toRemove.Add(emulator);
            }
            foreach (Emulator emulator in toRemove)
                options.Remove(emulator);


            //add new emulators
            foreach (Emulator emulator in completeCollection)
            {
                if (!existingEmulators.Contains(emulator))
                    options.Add(emulator);
            }

            itemList.Items.Refresh();

        }
    }
}
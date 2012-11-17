using System;
using System.Windows;
using System.Windows.Forms;
using SomewhatGeeky.Arcadia.Engine;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for RepositoryEditorWindow.xaml
    /// </summary>
    public partial class RepositoryEditorWindow : Window
    {
        private Repository item;

        public RepositoryEditorWindow(Window owner, Repository item)
        {
            InitializeComponent();

            this.Owner = owner;

            this.item = item;

            nameBox.Text = item.Name;
            rootPathBox.Text = item.RootPath;
        }

        public RepositoryEditorWindow(Window owner, ArcadiaLibrary library)
            : this(owner, new Repository(library))
        {
        }

        public Repository Result
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
            DialogResult = true;

            item.Name = nameBox.Text;
            item.RootPath = rootPathBox.Text;

        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = "Select a root directory.";

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string path = dialog.SelectedPath;

            path = Common.MakeRelativePath(path);

            rootPathBox.Text = path;

            if (String.IsNullOrEmpty(nameBox.Text))
            {
                nameBox.Text = System.IO.Path.GetFileName(path);

                if (path.StartsWith(@"\\"))
                {
                    string networkComputerName = path;
                    while (!string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(networkComputerName)))
                    {
                        networkComputerName = System.IO.Path.GetDirectoryName(networkComputerName);
                    }
                    networkComputerName = networkComputerName.Substring(2, networkComputerName.LastIndexOf(@"\") - 2);
                    nameBox.Text += " on " + networkComputerName;
                }
            }
        }
    }
}

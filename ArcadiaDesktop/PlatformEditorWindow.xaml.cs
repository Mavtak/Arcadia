using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using SomewhatGeeky.Arcadia.Engine;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for PlatformEditorWindow.xaml
    /// </summary>
    public partial class PlatformEditorWindow : Window
    {
        private Platform item;

        public PlatformEditorWindow(Window owner, Platform item)
        {
            InitializeComponent();

            this.Owner = owner;

            this.item = item;

            nameBox.Text = item.Name;
            otherNamesBox.Text = Common.ListToString(item.OtherNames);
            fileExtensionsBox.Text = Common.ListToString(item.UniqueFileExtensions);
            namePatternsBox.Text = Common.ListToString(item.NamePatterns.Select(pattern => pattern.ToString()));
        }

        public PlatformEditorWindow(Window owner)
            : this(owner, new Platform())
        { }

        public Platform Result
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

            item.OtherNames.Clear();
            item.OtherNames.AddRange(Common.StringToList(otherNamesBox.Text));

            item.UniqueFileExtensions.Clear();
            item.UniqueFileExtensions.AddRange(Common.StringToList(fileExtensionsBox.Text));

            item.NamePatterns.Clear();
            foreach (string pattern in Common.StringToList(namePatternsBox.Text))
            {
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                item.NamePatterns.Add(regex);
            }
        }
    }
}

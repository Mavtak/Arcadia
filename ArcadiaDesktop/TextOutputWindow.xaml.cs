using System.Windows;
using System.Windows.Input;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for TextOutputWindow.xaml
    /// </summary>
    public partial class TextOutputWindow : Window
    {
        public TextOutputWindow(Window owner, string title, string text)
        {
            InitializeComponent();
            this.Owner = owner;
            this.Title = title;
            textBox.Text = text;
        }

        public static void ShowTextDialog(Window owner, string title, string text)
        {
            var dialog = new TextOutputWindow(owner, title, text);
            dialog.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter)
            {
                this.Close();
            }
        }
    }
}

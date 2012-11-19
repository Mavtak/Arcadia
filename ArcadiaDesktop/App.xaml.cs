using System;
using System.Linq;
using System.Windows;

namespace SomewhatGeeky.Arcadia.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (e.Args.Any())
                {
                    var launcher = new StandaloneGameLoader();

                    foreach (var path in e.Args)
                    {
                        launcher.Open(path);
                    }

                    Application.Current.Shutdown(0);
                }
            }
            catch (Exception exception)
            {
                MessageBoxGenerator.ShowCriticalErrorMessageBox(exception);
            }
        }
    }
}

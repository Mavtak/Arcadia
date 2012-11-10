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
using SomewhatGeeky.UpdateChecker.Client;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public delegate void ParameterlessDelegate();
    public delegate void StringDelegate(string text);
    public delegate void ListOfGamesDelegate(List<Game> games);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        private Arcadia.Engine.ArcadiaLibrary library = new Arcadia.Engine.ArcadiaLibrary();
        private UpdaterClient updateChecker = new UpdaterClient(new string[] { "http://arcadia.SomewhatGeeky.com/update/", "http://arcadia.SomewhatGeeky.com/updates/", "http://update.SomewhatGeeky.com/", "http://updates.SomewhatGeeky.com/" }, "Arcadia Desktop", GuiCommon.Version, false);

        #region load and close code
        public MainWindow()
        {
            InitializeComponent();
            changeWindowTitle("");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                updateChecker.UpdateFound += new UpdatesFoundEventHandler(updateChecker_UpdateFound);
                updateChecker.UpdateCheckFailed += new UpdateCheckFailedEventHandler(updateChecker_UpdateCheckFailed);

                if (!(Environment.UserName.Equals("david", StringComparison.InvariantCultureIgnoreCase) && Environment.MachineName.Equals("rory", StringComparison.InvariantCultureIgnoreCase)))
                {
                    
                    //woop!  Only check for updates if you're not me!
                    updateChecker.CheckForUpdatesAsync();
                }

                loadDataFile();
            }
            catch(Exception exc)
            {
                GuiCommon.ShowCriticalErrorMessageBox(exc);
                die();
            }
        }

        private void die()
        {
            //ensures that this thread runs in the main thread.
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new ParameterlessDelegate(die), null);
                return;
            }

            Application.Current.Shutdown(1);
        }

        private void loadDataFile()
        {
            //runs this method in a thread other than the main thread
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                System.Threading.Thread newThread = new System.Threading.Thread(new System.Threading.ThreadStart(loadDataFile));
                newThread.Start();
                return;
            }

            try
            {
                if (System.IO.File.Exists(DataFilePath))
                    library.ReadFromFile(DataFilePath);
                else
                {
                    library.LoadDefaultSettings();
                    showAboutWindow();
                }
                updateSearch();

                showAddRepositoriesSuggestion();
            }
            catch (System.IO.IOException)
            {
                GuiCommon.ShowMessageBox("Oops!  There was an error loading the data file.  Am I on a read-only file location?", MessageBoxButton.OK, MessageBoxImage.Error);
                die();
            }
            catch (Exception exc)
            {
                GuiCommon.ShowCriticalErrorMessageBox(exc);
                die();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            changeWindowTitle("Saving");
            library.WriteToFile(DataFilePath);
        }

        private string DataFilePath
        {
            get
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arcadia.xml");
            }
        }
        #endregion

        #region update checker code
        void updateChecker_UpdateCheckFailed(object sender, EventArgs eventArgs)
        {
            //ensures that this thread runs in the main thread.
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new UpdateCheckFailedEventHandler(updateChecker_UpdateCheckFailed), new object[] { sender, eventArgs });
                return;
            }

            menu1.Items.Add("Couldn't check for updates");
        }        

        void updateChecker_UpdateFound(object sender, UpdatesFoundEventArgs eventArgs)
        {
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new UpdatesFoundEventHandler(updateChecker_UpdateFound), new object[] { sender, eventArgs });
                return;
            }

            foreach (SomewhatGeeky.UpdateChecker.Common.Update update in eventArgs.Updates)
            {
                TextOutputWindow dialog = new TextOutputWindow(this, "Update available",
                    "Name: " + (update.ProjectName ?? "unknown")
                    + "\nVersion: " + ((update.Version != null) ? (update.Version.ToString()) : ("unknown"))
                    + "\nStable: " + ((update.Stable != null) ? (update.Stable.ToString()) : ("unknown"))
                    + "\nInformation URL: " + (update.InformationUrl ?? "unknown")
                    + "\n\n" + update.Description
                    );

                dialog.Show();
            }
        }
        #endregion

        #region search code
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            updateSearch();
        }

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void updateSearch()
        {
            //ensures that this method executes in the main thread.
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new ParameterlessDelegate(updateSearch), null);
                return;
            }

           
            gameList.ItemsSource = library.Searcher.Search(searchBox.Text);
            changeWindowTitle("Displaying " + gameList.Items.Count);
        }

        private void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            updateSearch();
        }
        #endregion

        private void addGamesToList(List<Game> games)
        {
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new ListOfGamesDelegate(addGamesToList), new object[] { games });
                return;
            }

            foreach (Game game in games)
                gameList.Items.Add(game);
        }

        

        private void EditRepositoriesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RepositoryListWindow window = new RepositoryListWindow(this, library.Repositories);
            window.ShowDialog();

            updateSearch();
        }

        private void editPlatformsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PlatformListWindow window = new PlatformListWindow(this, library.Platforms);
            window.ShowDialog();
        }

        private void EditEmulatorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EmulatorListWindow window = new EmulatorListWindow(this, library.Emulators);
            window.ShowDialog();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateSearch();
        }

        private void reprocessGameDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (Game game in library.Games)
                game.FillInInformation(true);
        }

        public Game SelectedGame
        {
            get
            {
                if (gameList.SelectedItems.Count != 1)
                    return null;
                return (Game)(gameList.SelectedItem);
            }
        }

        private void gameList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                playGame(SelectedGame);
        }     
        private void gameList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            playGame(SelectedGame);
        }
        //TODO: make sure tablet pens work

        private void playGame(Game game)
        {
            if (game == null)
                return;

            Emulator emulator = null;
            var emulatorOptions = library.Emulators.GetEmulatorChoices(game.Platform).ToList();

            
            if (emulatorOptions.Count == 1)
            {
                emulator = emulatorOptions[0];
            }
            else
            {
                if (emulatorOptions.Count == 0)
                    emulatorOptions = library.Emulators.ToList();

                EmulatorSelectorWindow dialog = new EmulatorSelectorWindow(this, library.Emulators, emulatorOptions);
                dialog.ShowDialog();

                emulator = dialog.Result;
            }

            if (emulator == null)
                return;

            emulator.OpenGame(game);
        }

        private void produceDefaultSettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(builder);

            library.WriteDefaultSettings(writer);

            writer.Close();

            TextOutputWindow window = new TextOutputWindow(this, "Default Settings as XML", builder.ToString());

            window.Show();
        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            if (library.Repositories.Count == 0)
            {
                GuiCommon.ShowMessageBox("You haven't told Arcadia where your games are.", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                EditRepositoriesMenuItem_Click(sender, null);
                return;
            }

            scanAsync();            
        }

        private void scanAsync()
        {
            if (Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                System.Threading.Thread newThread = new System.Threading.Thread(new System.Threading.ThreadStart(scanAsync));
                newThread.Start();
                return;
            }

            changeWindowTitle("Scanning...");
            int addedCount = 0;
            foreach (Repository repository in library.Repositories)
            {
                try
                {
                    var addedGames = repository.ScanForNewGames();

                    foreach (var game in addedGames)
                    {
                        addedCount++;

                        if(addedCount % 100 == 0)
                        {
                            changeWindowTitle("Scanning... added " + addedCount);
                        }
                    }
                }
                catch (System.IO.DirectoryNotFoundException exc1)
                {
                    GuiCommon.ShowMessageBox(exc1.Message, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch (Exception exc2)
                {
                    GuiCommon.ShowCriticalErrorMessageBox(exc2);
                }
            }

            if (addedCount == 0)
                changeWindowTitle("No games found.");
            else if (addedCount == 1)
                changeWindowTitle("1 game found");
            else
                changeWindowTitle(addedCount + " games found.");

            updateSearch();

        }

        private void checkLibraryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<string> messages = new List<string>();

            //TODO: lots sanity checks
            //foreach (GenericLibraryItem item in library)
            //{
            //    if(item.ParentGameLibrary != library)
            //        messages.Add("parent ArcadiaLibrary for the " + item.GetType() + " is not set properly. (it's value is " + game.ParentGameLibrary + ")");
            //}

            foreach (Game game in library.Games)
            {
                if (game.Repository == null)
                {
                    messages.Add("Repository for " + game + " is null");
                }
                else if (!library.Contains(game.Repository))
                {
                    messages.Add("Repository for " + game + " is not in the library.  (repository is located at " + game.Repository.RootPath);
                }

            }

            if (messages.Count == 0)
            {
                GuiCommon.ShowMessageBox("no problems! :-)");
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("I found some problems. :-(");
            foreach (string message in messages)
                builder.Append("\n\n" + message);


            TextOutputWindow.ShowTextDialog(this, "Library Sanity Check Results", builder.ToString());
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            showAboutWindow();
        }

        private void changeWindowTitle(string text)
        {
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new StringDelegate(changeWindowTitle), new object[] { text });
                return;
            }

            if(String.IsNullOrEmpty(text))
                Title = GuiCommon.MainWindowBaseTitle;
            else
                Title = GuiCommon.MainWindowBaseTitle + " (" + text + ")";
        }

        private void showAboutWindow()
        {
            //ensure that this method is executed in the main thread
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.Invoke(new ParameterlessDelegate(showAboutWindow), null);
                return;
            }

            TextOutputWindow dialog = new TextOutputWindow(this, GuiCommon.MainWindowBaseTitle,
                    "Welcome to " + GuiCommon.MainWindowBaseTitle + "!"
                    + "\n"
                    + "\n        Arcadia is an emulator frontend that manages a wide variety of classic gaming system files.  I design it to be as configurable as the users want and yet still be easy to use right for the first time.  Going forward I hope to build an online community around Arcadia where users can share game ratings, reviews, links, and game saves.  I also hope to incorporate Arcadia into social networking websites like Facebook and Twitter.  All of this is purely for my own recreation, so I hope you guys are patient with me, and as excited as I am!"
                    + "\n        Look around, enjoy, and email any comments to " + Arcadia.Engine.Common.ContactEmail + ".  I really appreciate your feedback."
                    + "\n        For the latest information, check out posts about Arcadia on my blog: davidmcgrath.com/arcadia"
                    + "\n"
                    + "\nThanks,"
                    + "\nDavid McGrath"
                    + "\n\n"
                    + "\n Arcadia.Desktop Version: " + GuiCommon.Version
                    + "\n Arcadia.Engine Version: " + Arcadia.Engine.Common.LibraryVersion
                    );
            dialog.ShowDialog();
        }

        private void showAddRepositoriesSuggestion()
        {
            if (library.Repositories.Count > 0)
                return;

            //ensure that this method is executed in the main thread
            if (Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new ParameterlessDelegate(showAddRepositoriesSuggestion), null);
                return;
            }

            if (GuiCommon.ShowMessageBox("It looks like you don't have any repositories set.\nHow about you tell me where your ROMs and things are?", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                EditRepositoriesMenuItem_Click(null, null);

                if (library.Repositories.Count > 0 && GuiCommon.ShowMessageBox("When you want Arcadia to scan for new ROMs, click \"Settings and Tools\" and then \"Scan For ROMs\".\nDo you want to scan now?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    scanAsync();
                }
            }
        }
        
    }
}

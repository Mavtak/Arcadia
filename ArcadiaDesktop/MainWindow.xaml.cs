using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using SomewhatGeeky.Arcadia.Engine;
using SomewhatGeeky.Arcadia.Engine.Items;
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
        

        private ArcadiaLibrary library = new ArcadiaLibrary();
        private UpdaterClient updateChecker = new UpdaterClient(new string[] { "http://arcadia.SomewhatGeeky.com/update/", "http://arcadia.SomewhatGeeky.com/updates/", "http://update.SomewhatGeeky.com/", "http://updates.SomewhatGeeky.com/" }, "Arcadia Desktop", GuiCommon.Version, false);

        #region load and close code
        public MainWindow()
        {
            InitializeComponent();
            changeWindowTitle();
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

                GuiCommon.Load(this);

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
            if (Dispatcher.Thread == Thread.CurrentThread)
            {
                var newThread = new Thread(new System.Threading.ThreadStart(loadDataFile));
                newThread.Start();
                return;
            }

            try
            {
                if (System.IO.File.Exists(DataFilePath))
                {
                    library.ReadFromFile(DataFilePath);
                }
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
                MessageBoxes.ShowErrorLoadingDataFileError();
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
            GuiCommon.Save(this);
            library.WriteToFile(DataFilePath);
        }

        private string DataFilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arcadia.xml");
            }
        }
        #endregion

        #region update checker code
        void updateChecker_UpdateCheckFailed(object sender, EventArgs eventArgs)
        {
            //ensures that this thread runs in the main thread.
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new UpdateCheckFailedEventHandler(updateChecker_UpdateCheckFailed), new object[] { sender, eventArgs });
                return;
            }

            menu1.Items.Add("Couldn't check for updates");
        }        

        void updateChecker_UpdateFound(object sender, UpdatesFoundEventArgs eventArgs)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new UpdatesFoundEventHandler(updateChecker_UpdateFound), new object[] { sender, eventArgs });
                return;
            }

            foreach (SomewhatGeeky.UpdateChecker.Common.Update update in eventArgs.Updates)
            {
                var dialog = new TextOutputWindow(this, "Update available",
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
            if (Dispatcher.Thread != Thread.CurrentThread)
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

        private void EditRepositoriesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new RepositoryListWindow(this, library.Repositories);
            window.ShowDialog();

            updateSearch();
        }

        private void editPlatformsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new PlatformListWindow(this, library.Platforms);
            window.ShowDialog();
        }

        private void EditEmulatorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new EmulatorListWindow(this, library.Emulators);
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
                {
                    return null;
                }

                return gameList.SelectedItem as Game;
            }
        }

        private void gameList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                playGame(SelectedGame);
            }
        }     
        private void gameList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            playGame(SelectedGame);
        }
        //TODO: make sure tablet pens work

        private void playGame(Game game)
        {
            if (game == null)
            {
                return;
            }

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

                var dialog = new EmulatorSelectorWindow(this, library.Emulators, emulatorOptions);
                dialog.ShowDialog();

                emulator = dialog.Result;
            }

            if (emulator == null)
            {
                return;
            }

            emulator.OpenGame(game);
        }

        private void produceDefaultSettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var builder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(builder);

            library.WriteDefaultSettings(writer);

            writer.Close();

            var window = new TextOutputWindow(this, "Default Settings as XML", builder.ToString());

            window.Show();
        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            if (library.Repositories.Count == 0)
            {
                showAddRepositoriesSuggestion(true);
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

            try
            {
                foreach (var game in library.ScanForNewGames())
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
                MessageBoxes.ShowScanningError(exc1);
            }
            catch (Exception exc2)
            {
                GuiCommon.ShowCriticalErrorMessageBox(exc2);
            }

            if (addedCount == 0)
            {
                changeWindowTitle("No games found.");
            }
            else if (addedCount == 1)
            {
                changeWindowTitle("1 game found");
            }
            else
            {
                changeWindowTitle(addedCount + " games found.");
            }

            updateSearch();
        }

        private void checkLibraryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var messages = new List<string>();

            //TODO: lots sanity checks

            foreach (var item in library.AllItems)
            {
                if (item.ParentGameLibrary != library)
                {
                    messages.Add("parent ArcadiaLibrary for the " + item.GetType() + " is not set properly. (it's value is " + item.ParentGameLibrary + ")");
                }
            }

            foreach (var game in library.Games)
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
                MessageBoxes.ShowNoProblemsMessage();
                return;
            }

            var builder = new StringBuilder();
            builder.Append("I found some problems. :-(");
            foreach (string message in messages)
            {
                builder.Append("\n\n");
                builder.Append(message);
            }

            TextOutputWindow.ShowTextDialog(this, "Library Sanity Check Results", builder.ToString());
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            showAboutWindow();
        }

        private void changeWindowTitle(string text = null)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new StringDelegate(changeWindowTitle), text);
                return;
            }

            var builder = new StringBuilder();

            builder.Append(GuiCommon.MainWindowBaseTitle);

            if (!String.IsNullOrEmpty(text))
            {
                builder.Append(" (");
                builder.Append(text);
                builder.Append(")");
            }

            Title = builder.ToString();
        }

        private void showAboutWindow()
        {
            //ensure that this method is executed in the main thread
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new ParameterlessDelegate(showAboutWindow), null);
                return;
            }

            var dialog = new AboutWindow(this);

            dialog.ShowDialog();
        }

        private void showAddRepositoriesSuggestion(bool autoScan)
        {
            if (library.Repositories.Count > 0)
            {
                return;
            }

            //ensure that this method is executed in the main thread
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.BeginInvoke(new ParameterlessDelegate(showAddRepositoriesSuggestion), null);
                return;
            }

            if (MessageBoxes.ShowNoRepositoriesMessage() == MessageBoxResult.OK)
            {
                EditRepositoriesMenuItem_Click(null, null);

                if (library.Repositories.Count > 0 && (autoScan || MessageBoxes.ShouldScanForRoms() == MessageBoxResult.Yes))
                {
                    scanAsync();
                }
            }
        }

        private void showAddRepositoriesSuggestion()
        {
            showAddRepositoriesSuggestion(false);
        }
        
    }
}

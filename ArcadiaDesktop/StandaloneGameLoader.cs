using System.IO;
using SomewhatGeeky.Arcadia.Engine;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Desktop
{
    public class StandaloneGameLoader
    {
        private ArcadiaLibrary library;

        public void Open(string path)
        {
                if (!File.Exists(GuiCommon.LibraryFilePath))
                {
                    MessageBoxGenerator.ShowError("Could not run ROM, because there is no library.");
                    return;
                }

                if (library == null)
                {
                    library = new ArcadiaLibrary();
                    library.ReadFromFile(GuiCommon.LibraryFilePath);
                }
                else
                {
                    library.LoadDefaultSettings();
                }

                var game = library.Games.GetByPath(path);

                if (game == null)
                {
                    var tempRepository = new Repository("Temporary Repository", library);
                    tempRepository.RootPath = Path.GetDirectoryName(path);
                    library.Add(tempRepository);

                    game = new Game(Path.GetFileNameWithoutExtension(path), library)
                    {
                        InnerPath = Path.GetFileName(path),
                        Repository = tempRepository
                    };
                    game.FillInInformation(true);

                    library.Remove(tempRepository);
                }

                GuiCommon.PlayGame(null, game);
        }
    }
}

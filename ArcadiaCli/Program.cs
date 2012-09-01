using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SomewhatGeeky.Arcadia.Engine;

namespace SomewhatGeeky.Arcadia.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            ArcadiaLibrary library = new ArcadiaLibrary();

            library.Platforms.CommonFileExtensions.Add("zip");

            Platform platform1 = new Platform("Nintendo 64", library);
            library.Add(platform1);
            platform1.OtherNames.Add("N64");
            platform1.OtherNames.Add("Nintendo64");
            platform1.UniqueFileExtensions.Add("n64");
            platform1.UniqueFileExtensions.Add("z64");

            Language language1 = new Language("English", library);
            library.Add(language1);
            language1.OtherNames.Add("EN");
            language1.OtherNames.Add("ENGL");

            Repository repository1 = new Repository("BLEH", library);
            library.Add(repository1);
            repository1.RootPath = @"\\DREAMINGNEST\Emulated Games\ROMs\Nintendo 64";
            repository1.ScanForNewGames();

            //repository1.AddGame(repository1.RootPath + @"\game a (en).z64");
            //repository1.AddGame(repository1.RootPath + @"\game b (u) (en).n64");
            //repository1.AddGame(repository1.RootPath + @"\Nintendo 64\game c.zip");
            //repository1.AddGame(repository1.RootPath + @"\N64\game d.zip");
            //repository1.AddGame(repository1.RootPath + @"\Super Nintendo\game e.zip");


            /*
            Game game1 = new Game("Mario LOL", library);
            library.Add(game1);
            game1.OtherNames.Add("mario_lol");
            game1.OtherNames.Add("mariolol");
            game1.Platform = platform1;
            game1.Language = language1;
            game1.Repository = repository1;
            game1.Players = new NumberRange("1-4");
            */

            string filename = @"C:\Users\David\Desktop\Arcadia library.xml";
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(filename);
            library.WriteToXml(writer, library.DefaultXmlNodeName);
            writer.Close();
            System.Diagnostics.Process.Start(filename);
            
            //Console.WriteLine(library);

            /*
            foreach (Game game in library.Games)
            {
                Console.WriteLine();
                Console.WriteLine("Name:\t " + game.Name);
                if(game.PlatformIsSet)
                    Console.WriteLine("Platform:\t" + game.Platform.Name);
                if(game.FilenameFlagsAreSet)
                {
                    Console.WriteLine("FilenameFlags:");
                    foreach (string filenameFlag in game.FilenameFlags)
                        Console.WriteLine("\t\t" + filenameFlag);
                }
                
            }*/
            
            Console.ReadKey();
        }
    }
}

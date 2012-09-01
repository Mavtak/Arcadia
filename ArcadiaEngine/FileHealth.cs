using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace SomewhatGeeky.Arcadia.Engine
{
    public static class FileHealth
    {

        public static void RemoveFileExtensionFromName(Game game)
        {
            game.Name = game.Name.Replace(System.IO.Path.GetExtension(game.Name), "");
        }

        public static void StripFlagsFromTitle(Game game)
        {
            foreach (string filenameFlag in game.FilenameFlags)
                game.Name = game.Name.Replace(filenameFlag, "");

            while (game.Name.Contains("  "))
                game.Name = game.Name.Replace("  ", " ");
        }
    }
}

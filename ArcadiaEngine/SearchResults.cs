using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class SearchResults : System.Collections.Generic.IEnumerable<Game>, System.Collections.IEnumerable
    {
        private ArcadiaLibrary library;
        private string query;

        internal SearchResults(ArcadiaLibrary library, string query)
        {
            this.library = library;
            this.query = query;
        }


        private static bool gameMatchesWord(Game game, string word)
        {
            if (game.Name != null && game.Name.ToLower().Contains(word))
                return true;
            if (game.Platform != null && game.Platform.NameMatches(word))
                return true;
            if (game.InnerPath != null && game.InnerPath.ToLower().Contains(word))
                return true;

            return false;
        }

        private static bool gameMatches(Game game, string [] queryWords)
        {
            foreach (string queryWord in queryWords)
            {
                if (!gameMatchesWord(game, queryWord))
                    return false;
            }
            return true;
        }


        System.Collections.Generic.IEnumerator<Game> System.Collections.Generic.IEnumerable<Game>.GetEnumerator()
        {
            string[] queryWords = query.ToLower().Split(' ');
            bool matches;

            lock (library.Games)
            {
                foreach (Game game in library.Games)
                {
                    if(gameMatches(game, queryWords))
                        yield return game;
                }
            }

            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (Game game in (IEnumerable<Game>)this)
                yield return game;
            yield break;
        }
    }
}

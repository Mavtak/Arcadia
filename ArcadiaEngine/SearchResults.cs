using System.Collections.Generic;
using System.Linq;

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
            {
                return true;
            }

            if (game.Platform != null && game.Platform.NameMatches(word))
            {
                return true;
            }

            if (game.InnerPath != null && game.InnerPath.ToLower().Contains(word))
            {
                return true;
            }

            return false;
        }

        private static bool gameMatches(Game game, IEnumerable<string> queryWords)
        {
            var result = queryWords.All(word => gameMatchesWord(game, word));

            return result;
        }

        IEnumerator<Game> IEnumerable<Game>.GetEnumerator()
        {
            var queryWords = query.ToLower().Split(' ');

            lock (library.Games)
            {
                var results = from game in library.Games
                              where gameMatches(game, queryWords)
                              select game;

                foreach (var result in results)
                {
                    yield return result;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Game>)this).GetEnumerator();
        }
    }
}

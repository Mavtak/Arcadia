
namespace SomewhatGeeky.Arcadia.Engine
{
    public class LibrarySearcher
    {
        private ArcadiaLibrary library;

        internal LibrarySearcher(ArcadiaLibrary library)
        {
            this.library = library;
        }

        public SearchResults Search(string query)
        {
            return new SearchResults(library, query);
        }
    }
}

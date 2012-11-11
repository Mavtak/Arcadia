using System.Collections.Generic;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class GameCollection : GenericLibraryItemCollection<Game>
    {
        private HashSet<string> paths;


        public GameCollection(ArcadiaLibrary parentLibrary)
            : base(parentLibrary)
        {
            paths = new HashSet<string>();
        }

        public override void Add(Game item)
        {
            paths.Add(item.FullPath);

            base.Add(item);
        }

        public override bool Remove(Game item)
        {
            paths.Remove(item.FullPath);

            return base.Remove(item);
        }

        public bool ContainsPath(string path)
        {
            //TODO: improve
            return paths.Contains(System.IO.Path.GetFullPath(path));
        }

    }
}

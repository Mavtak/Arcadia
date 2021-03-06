﻿using System.Collections.Generic;
using SomewhatGeeky.Arcadia.Engine.Items;
using System.IO;
using System.Linq;

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
            lock (this)
            {
                paths.Add(item.FullPath);

                base.Add(item);
            }
        }

        public override bool Remove(Game item)
        {
            lock (this)
            {
                paths.Remove(item.FullPath);

                return base.Remove(item);
            }
        }

        public Game GetByPath(string path)
        {
            path = Path.GetFullPath(path);

            var results = from game in this
                          where game.FullPath.Equals(path, System.StringComparison.InvariantCultureIgnoreCase)
                          select game;

            var result = results.FirstOrDefault();

            return result;
        }

        public bool ContainsPath(string path)
        {
            //TODO: improve
            return paths.Contains(System.IO.Path.GetFullPath(path));
        }

    }
}

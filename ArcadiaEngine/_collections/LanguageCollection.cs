using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class LanguageCollection : GenericLibraryItemCollection<Language>
    {

        public LanguageCollection(ArcadiaLibrary parentGameLibrary)
            : base(parentGameLibrary)
        {
        }

    }
}

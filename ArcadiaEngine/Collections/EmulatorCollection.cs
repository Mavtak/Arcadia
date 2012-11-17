using System.Collections.Generic;
using System.Linq;
using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class EmulatorCollection : GenericLibraryItemCollection<Emulator>
    {

        #region constructors

        public EmulatorCollection(ArcadiaLibrary parentGameLibrary)
            : base(parentGameLibrary)
        {
        }
        public EmulatorCollection()
            : this(null)
        {
        }

        #endregion


        public IEnumerable<Emulator> GetEmulatorChoices(Platform platform)
        {
            var result = from emulator in this
                         where emulator.CompatablePlatforms.Contains(platform)
                         select emulator;

            return result;
        }

    }
}

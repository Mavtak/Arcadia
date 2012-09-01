using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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


        public List<Emulator> GetEmulatorChoices(Platform platform)
        {
            List<Emulator> result = new List<Emulator>();

            foreach (Emulator emulator in this)
                if (emulator.CompatablePlatforms.Contains(platform))
                    result.Add(emulator);

            return result;
        }

    }
}

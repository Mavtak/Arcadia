using System;

namespace SomewhatGeeky.UpdateChecker.Server
{
   internal class InternalLibraryVersion
    {
        internal static Version GetLibraryVersion()
        {
            //automatically incremented by build script
            return new Version("1.0.87.0");
        }
    }
}

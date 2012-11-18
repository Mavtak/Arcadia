using System;

namespace SomewhatGeeky.UpdateChecker.Client
{
   internal class InternalLibraryVersion
    {
        internal static Version GetLibraryVersion()
        {
            //automatically incremented by build script
            return new Version("1.3.0.0");
        }
    }
}

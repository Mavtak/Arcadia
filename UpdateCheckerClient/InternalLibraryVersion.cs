using System;

namespace SomewhatGeeky.UpdateChecker.Client
{
   internal class InternalLibraryVersion
    {
        internal static Version GetLibraryVersion()
        {
            //automatically incremented by build script
            return new Version("1.0.132.0");
        }
    }
}

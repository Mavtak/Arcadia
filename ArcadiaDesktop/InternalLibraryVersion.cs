using System;

namespace SomewhatGeeky.Arcadia.Desktop
{
   internal class InternalLibraryVersion
    {
        internal static Version GetLibraryVersion()
        {
            //automatically incremented by build script
            return new Version("1.3.0.0");
        }

        internal static DateTime? LastBuildTime
        {
            get
            {
                var date = "2012-11-18T18:26:24.1630859Z";

                if (String.IsNullOrEmpty(date))
                {
                    return null;
                }

                return Convert.ToDateTime(date);
            }
        }
    }
}

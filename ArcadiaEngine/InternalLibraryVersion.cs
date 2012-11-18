using System;

namespace SomewhatGeeky.Arcadia.Engine
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
                var date = "2012-11-18T18:24:48.7695312Z";

                if (String.IsNullOrEmpty(date))
                {
                    return null;
                }

                return Convert.ToDateTime(date);
            }
        }
    }
}

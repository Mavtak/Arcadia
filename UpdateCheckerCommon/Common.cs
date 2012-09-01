using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.UpdateChecker.Common
{
    public static class Common
    {
        public static bool XIsLaterThanY(Version x, Version y)
        {
            if (x.Major > y.Major)
                return true;
            if (x.Major == y.Major && x.Minor > y.Minor)
                return true;
            if (x.Major == y.Major && x.Minor == y.Minor && x.Revision > y.Revision)
                return true;
            if (x.Major == y.Major && x.Minor == y.Minor && x.Revision == y.Revision && x.Build > y.Build)
                return true;
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public interface IFilterItem
    {
        bool Matches(Game game);
    }
}

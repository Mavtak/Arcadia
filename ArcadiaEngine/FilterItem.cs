using SomewhatGeeky.Arcadia.Engine.Items;

namespace SomewhatGeeky.Arcadia.Engine
{
    public interface IFilterItem
    {
        bool Matches(Game game);
    }
}

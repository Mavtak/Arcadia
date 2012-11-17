
namespace SomewhatGeeky.Arcadia.Engine.Items
{
    public class Language : GenericLibraryItem, IFilterItem
    {
        #region constructors

        public Language(string name, ArcadiaLibrary parentGameLibrary)
            : base(name, parentGameLibrary)
        { }
        public Language(string name)
            : this(name, null)
        { }
        public Language(ArcadiaLibrary parentGameLibrary)
            : this(null, parentGameLibrary)
        { }
        public Language()
            : this(null, null)
        { }

        #endregion

        bool IFilterItem.Matches(Game game)
        {
            return game.Language.Equals(this);
        }
    }
}

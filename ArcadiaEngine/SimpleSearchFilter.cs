using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomewhatGeeky.Arcadia.Engine
{
    public class SimpleSearchFilter
    {
        public enum SearchFilterMode
        {
            MatchAllFilterItems, MatchSomeFilterItems, MatchNoFilterItems
        }
        private ArcadiaLibrary parentGameLibrary;
        private List<IFilterItem> filterItems;
        private SearchFilterMode searchMode = SearchFilterMode.MatchSomeFilterItems;

        public SimpleSearchFilter(ArcadiaLibrary parentGameLibrary)
        {
            this.parentGameLibrary = parentGameLibrary;
            filterItems = new List<IFilterItem>();
        }

        public bool GameIsMatch(Game game)
        {
            if (filterItems.Count == 0)
                return true;
            
            foreach (IFilterItem filterItem in filterItems)
            {
                if (filterItem.Matches(game))
                {
                    if (searchMode == SearchFilterMode.MatchSomeFilterItems)
                        return true;
                    if(searchMode == SearchFilterMode.MatchNoFilterItems)
                        return false;
                }
                else
                {
                    if (searchMode == SearchFilterMode.MatchAllFilterItems)
                        return false;
                }
            }
            throw new Exception("whaaaaa? problem with the search filter.");
        }
    }
}

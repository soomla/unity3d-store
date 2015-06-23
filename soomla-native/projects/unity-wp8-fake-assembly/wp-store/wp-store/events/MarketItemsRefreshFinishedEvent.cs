using System.Collections.Generic;
using SoomlaWpStore.domain;
using SoomlaWpCore.events;
namespace SoomlaWpStore.events
{
    public class MarketItemsRefreshFinishedEvent : SoomlaEvent
    {
        private List<MarketItem> mMarketItems;

        public List<MarketItem> GetMarketItems()
        {
            return mMarketItems;
        }

        public MarketItemsRefreshFinishedEvent(List<MarketItem> marketItems)
            : this(marketItems, null)
        {
        }

        public MarketItemsRefreshFinishedEvent(List<MarketItem> marketItems, object sender)
            : base(sender)
        {
            mMarketItems = marketItems;
        }
    }
}

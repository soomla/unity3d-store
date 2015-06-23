using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class MarketItemsRefreshStartedEvent : SoomlaEvent
    {
        public MarketItemsRefreshStartedEvent()
            : this(null)
        {
        }

        public MarketItemsRefreshStartedEvent(object sender)
            : base(sender)
        {
        }
    }
}

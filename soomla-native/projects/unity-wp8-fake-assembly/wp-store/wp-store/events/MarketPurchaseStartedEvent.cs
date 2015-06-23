using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class MarketPurchaseStartedEvent : SoomlaEvent
    {
        private PurchasableVirtualItem mPurchasableVirtualItem;

        public PurchasableVirtualItem GetPurchasableVirtualItem()
        {
            return mPurchasableVirtualItem;
        }

        public MarketPurchaseStartedEvent(PurchasableVirtualItem item)
            : this(item,null)
        {
        }

        public MarketPurchaseStartedEvent(PurchasableVirtualItem item, object sender)
            : base(sender)
        {
            mPurchasableVirtualItem = item;
        }
    }
}

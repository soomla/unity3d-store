using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class MarketPurchaseCancelledEvent : SoomlaEvent
    {
        private PurchasableVirtualItem mPurchasableVirtualItem;

        public PurchasableVirtualItem GetPurchasableVirtualItem()
        {
            return mPurchasableVirtualItem;
        }

        public MarketPurchaseCancelledEvent(PurchasableVirtualItem item)
            : this(item,null)
        {
        }

        public MarketPurchaseCancelledEvent(PurchasableVirtualItem item, object sender)
            : base(sender)
        {
            mPurchasableVirtualItem = item;
        }
    }
}

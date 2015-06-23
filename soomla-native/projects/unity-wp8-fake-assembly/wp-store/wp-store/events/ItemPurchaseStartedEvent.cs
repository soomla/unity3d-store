using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class ItemPurchaseStartedEvent : SoomlaEvent
    {
        private PurchasableVirtualItem mPurchasableVirtualItem;

        public PurchasableVirtualItem GetPurchasableVirtualItem()
        {
            return mPurchasableVirtualItem;
        }

        public ItemPurchaseStartedEvent(PurchasableVirtualItem item)
            : this(item,null)
        {
        }

        public ItemPurchaseStartedEvent(PurchasableVirtualItem item, object sender):base(sender)
        {
            mPurchasableVirtualItem = item;
        }
    }
}

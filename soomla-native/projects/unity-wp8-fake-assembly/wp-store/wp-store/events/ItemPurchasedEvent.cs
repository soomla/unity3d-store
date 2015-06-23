using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class ItemPurchasedEvent : SoomlaEvent
    {
        private PurchasableVirtualItem mPurchasableVirtualItem;
        private string mPayload;

        public PurchasableVirtualItem GetPurchasableVirtualItem()
        {
            return mPurchasableVirtualItem;
        }
        public string GetPayload()
        {
            return mPayload;
        }

        public ItemPurchasedEvent(PurchasableVirtualItem item, string payload)
            : this(item,payload,null)
        {
        }

        public ItemPurchasedEvent(PurchasableVirtualItem item, string payload, object sender)
            : base(sender)
        {
            mPurchasableVirtualItem = item;
            mPayload = payload;
        }
    }
}

using SoomlaWpStore.domain;
using SoomlaWpCore.events;

namespace SoomlaWpStore.events
{
    public class MarketPurchaseEvent : SoomlaEvent
    {
        private PurchasableVirtualItem mPurchasableVirtualItem;
        private string mPayload;
        private string mToken;

        public PurchasableVirtualItem GetPurchasableVirtualItem()
        {
            return mPurchasableVirtualItem;
        }
        public string GetPayload()
        {
            return mPayload;
        }
        public string GetToken()
        {
            return mToken;
        }

        public MarketPurchaseEvent(PurchasableVirtualItem item, string payload, string token)
            : this(item, payload, token, null)
        {
        }

        public MarketPurchaseEvent(PurchasableVirtualItem item, string payload, string token, object sender)
            : base(sender)
        {
            mPurchasableVirtualItem = item;
            mPayload = payload;
            mToken = token;
        }
    }
}

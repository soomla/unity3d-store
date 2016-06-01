using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
    public class ItemPurchasedEvent : SoomlaEvent
    {
		private PurchasableVirtualItem mItem ;
        private String mPayload;

		public ItemPurchasedEvent(PurchasableVirtualItem item , String payload) : this(item , payload, null)
        {

        }

		public ItemPurchasedEvent(PurchasableVirtualItem item , String payload, Object sender) : base(sender)
        {
            mItem  = item ;
            mPayload = payload;
        }

		public PurchasableVirtualItem getItem ()
        {
            return mItem ;
        }

        public String getPayload()
        {
            return mPayload;
        }

    }
}
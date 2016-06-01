using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class MarketRefundEvent : SoomlaEvent
	{
		private PurchasableVirtualItem mPurchasableVirtualItem;

		public MarketRefundEvent (PurchasableVirtualItem purchasableVirtualItem): this(purchasableVirtualItem, null)
		{
		}

		public MarketRefundEvent (PurchasableVirtualItem purchasableVirtualItem, Object sender):base(sender)
		{
			mPurchasableVirtualItem = purchasableVirtualItem;
		}

		public PurchasableVirtualItem getPurchasableVirtualItem ()
		{
			return mPurchasableVirtualItem;
		}
	}
}
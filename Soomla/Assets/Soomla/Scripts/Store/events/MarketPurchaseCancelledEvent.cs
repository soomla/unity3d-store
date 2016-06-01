using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class MarketPurchaseCancelledEvent : SoomlaEvent
	{
		private PurchasableVirtualItem mPurchasableVirtualItem;

		public MarketPurchaseCancelledEvent (PurchasableVirtualItem purchasableVirtualItem) : this(purchasableVirtualItem, null)
		{

		}

		public MarketPurchaseCancelledEvent (PurchasableVirtualItem purchasableVirtualItem, Object sender) : base(sender)
		{
			mPurchasableVirtualItem = purchasableVirtualItem;
		}

		public PurchasableVirtualItem getPurchasableVirtualItem ()
		{
			return mPurchasableVirtualItem;
		}
	}
}

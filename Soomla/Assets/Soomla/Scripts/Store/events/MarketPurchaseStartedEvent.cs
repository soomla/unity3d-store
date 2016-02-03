using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class MarketPurchaseStartedEvent : SoomlaEvent
	{
		private PurchasableVirtualItem mPurchasableVirtualItem;
		private bool mFraudProtection;

		public MarketPurchaseStartedEvent (PurchasableVirtualItem purchasableVirtualItem) : this(purchasableVirtualItem, false, null)
		{

		}

		public MarketPurchaseStartedEvent (PurchasableVirtualItem purchasableVirtualItem, bool fraudProtection) : this(purchasableVirtualItem, fraudProtection, null)
		{

		}

		public MarketPurchaseStartedEvent (PurchasableVirtualItem purchasableVirtualItem, Object sender) : this(purchasableVirtualItem, false, sender)
		{
		}

		public MarketPurchaseStartedEvent (PurchasableVirtualItem purchasableVirtualItem, bool fraudProtection, Object sender) : base(sender)
		{
			mPurchasableVirtualItem = purchasableVirtualItem;
			mFraudProtection = fraudProtection;
		}

		public PurchasableVirtualItem getPurchasableVirtualItem ()
		{
			return mPurchasableVirtualItem;
		}

		public bool isFraudProtection ()
		{
			return mFraudProtection;
		}
	}
}
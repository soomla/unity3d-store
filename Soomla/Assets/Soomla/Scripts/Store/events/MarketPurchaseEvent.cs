using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class MarketPurchaseEvent : SoomlaEvent
	{
		public readonly PurchasableVirtualItem PurchasableVirtualItem;
		public readonly String Payload;
		public readonly Dictionary<String, String> ExtraInfo;

		public MarketPurchaseEvent (PurchasableVirtualItem purchasableVirtualItem, String payload,
		                           Dictionary<String, String> extraInfo) : this(purchasableVirtualItem, payload, extraInfo, null)
		{

		}

		public MarketPurchaseEvent (PurchasableVirtualItem purchasableVirtualItem, String payload,
		                           Dictionary<String, String> extraInfo, Object sender) : base(sender)
		{
			PurchasableVirtualItem = purchasableVirtualItem;
			Payload = payload;
			ExtraInfo = extraInfo;
		}
	}
}

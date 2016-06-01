using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class MarketItemsRefreshFinishedEvent : SoomlaEvent
	{
		List<MarketItem> mMarketItems;

		public MarketItemsRefreshFinishedEvent (List<MarketItem> marketItems) : this(marketItems, null)
		{

		}

		public MarketItemsRefreshFinishedEvent (List<MarketItem> marketItems, Object sender) : base(sender)
		{
			this.mMarketItems = marketItems;
		}

		public List<MarketItem> getMarketItems ()
		{
			return mMarketItems;
		}
	}
}

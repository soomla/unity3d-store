using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla
{
	public class MarketItemsRefreshFailedEvent : SoomlaEvent
	{
		public String ErrorMessage;

		public MarketItemsRefreshFailedEvent (String errorMessage) : this(errorMessage, null)
		{

		}

		public MarketItemsRefreshFailedEvent (String errorMessage, Object sender) : base(sender)
		{
			ErrorMessage = errorMessage;
		}
	}
}

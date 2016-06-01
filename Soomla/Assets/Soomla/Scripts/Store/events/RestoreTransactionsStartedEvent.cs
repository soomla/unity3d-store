using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class RestoreTransactionsStartedEvent : SoomlaEvent
	{

		public RestoreTransactionsStartedEvent () : this(null)
		{

		}

		public RestoreTransactionsStartedEvent (Object sender) : base(sender)
		{

		}
	}
}

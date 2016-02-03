using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class SoomlaStoreInitializedEvent : SoomlaEvent
	{
		public SoomlaStoreInitializedEvent () : this(null)
		{
		}

		public SoomlaStoreInitializedEvent (Object sender) : base(sender)
		{

		}
	}
}

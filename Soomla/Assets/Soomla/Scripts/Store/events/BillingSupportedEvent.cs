using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class BillingSupportedEvent : SoomlaEvent
	{
		public BillingSupportedEvent () : this(null)
		{

		}

		public BillingSupportedEvent (Object sender) : base(sender)
		{

		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class GoodEquippedEvent : SoomlaEvent
	{
		private EquippableVG mItem;

		public GoodEquippedEvent (EquippableVG item) : this(item, null)
		{
			mItem = item;
		}

		public GoodEquippedEvent (EquippableVG item, Object sender) : base(sender)
		{
			mItem = item;
		}

		public EquippableVG getGoodItem ()
		{
			return mItem;
		}

	}
}

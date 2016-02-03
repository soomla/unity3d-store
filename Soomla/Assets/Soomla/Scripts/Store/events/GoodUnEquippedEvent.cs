using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class GoodUnEquippedEvent : SoomlaEvent
	{
		private EquippableVG mItem ;

		public GoodUnEquippedEvent (EquippableVG item ) : this(item , null)
		{
		}

		public GoodUnEquippedEvent (EquippableVG item , Object sender) : base(sender)
		{
			mItem  = item ;
		}

		public EquippableVG getGoodItem  ()
		{
			return mItem ;
		}

	}
}

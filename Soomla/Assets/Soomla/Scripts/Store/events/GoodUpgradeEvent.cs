using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class GoodUpgradeEvent : SoomlaEvent
	{
		private VirtualGood mItem ;
		private UpgradeVG mCurrentUpgradeItem ;

		public GoodUpgradeEvent (VirtualGood item , UpgradeVG upgradeVGItem ) : this(item , upgradeVGItem , null)
		{

		}

		public GoodUpgradeEvent (VirtualGood item , UpgradeVG upgradeVGItem , Object sender) : base(sender)
		{
			mItem  = item ;
			mCurrentUpgradeItem  = upgradeVGItem ;
		}

		public VirtualGood getGoodItem  ()
		{
			return mItem ;
		}

		public UpgradeVG getCurrentUpgrade ()
		{
			return mCurrentUpgradeItem ;
		}
	}
}

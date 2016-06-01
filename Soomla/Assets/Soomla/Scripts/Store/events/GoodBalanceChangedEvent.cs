using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class GoodBalanceChangedEvent : SoomlaEvent
	{

		private VirtualGood mItem;
		private int mBalance;
		private int mAmountAdded;

		public GoodBalanceChangedEvent (VirtualGood item, int balance, int amountAdded) : this(item, balance, amountAdded, null)
		{

		}

		public GoodBalanceChangedEvent (VirtualGood item, int balance, int amountAdded, Object sender) : base(sender)
		{
			mItem = item;
			mBalance = balance;
			mAmountAdded = amountAdded;
		}

		public VirtualGood getGoodItemId ()
		{
			return mItem;
		}

		public int getBalance ()
		{
			return mBalance;
		}

		public int getAmountAdded ()
		{
			return mAmountAdded;
		}

	}
}

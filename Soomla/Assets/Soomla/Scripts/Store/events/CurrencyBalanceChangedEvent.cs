using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class CurrencyBalanceChangedEvent : SoomlaEvent
	{
		private VirtualCurrency mItem;
		private int mBalance;
		private int mAmountAdded;

		public CurrencyBalanceChangedEvent (VirtualCurrency item , int balance, int amountAdded) : this(item,balance,amountAdded,null)
		{

		}

		public CurrencyBalanceChangedEvent (VirtualCurrency item , int balance, int amountAdded, Object sender) : base(sender)
		{
			mItem = item ;
			mBalance = balance;
			mAmountAdded = amountAdded;
		}

		public VirtualCurrency getCurrencyItem ()
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

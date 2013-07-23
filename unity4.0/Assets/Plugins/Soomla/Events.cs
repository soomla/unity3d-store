using System;
using UnityEngine;

namespace com.soomla.unity
{
	public class Events
	{
		public delegate void Action();
		
		public static Action OnBillingNotSupported = delegate {};
		
		public static Action OnBillingSupported = delegate {};
		
		public static Action OnClosingStore = delegate {};
		
		public static Action<VirtualCurrency, int, int> OnCurrencyBalanceChanged = delegate {};
		
		public static Action<VirtualGood, int, int> OnGoodBalanceChanged = delegate {};
		
		public static Action<EquippableVG> OnGoodEquipped = delegate {};
		
		public static Action<EquippableVG> OnGoodUnEquipped = delegate {};
		
		public static Action<VirtualGood, UpgradeVG> OnGoodUpgrade = delegate {};
		
		public static Action<PurchasableVirtualItem> OnItemPurchased = delegate {};
		
		public static Action<PurchasableVirtualItem> OnItemPurchaseStarted = delegate {};
		
		public static Action OnOpeningStore = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketPurchaseCancelled = delegate {};	
		
		public static Action<PurchasableVirtualItem, string> OnMarketPurchase = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketPurchaseStarted = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketRefund = delegate {};
		
		public static Action<bool> OnRestoreTransactions = delegate {};
		
		public static Action OnRestoreTransactionsStarted = delegate {};
		
		public static Action OnUnexpectedErrorInStore = delegate {};
		
	}
}
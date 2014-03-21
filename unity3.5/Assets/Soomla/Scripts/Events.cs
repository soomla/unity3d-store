using System;
using UnityEngine;

namespace Soomla
{
	public class Events
	{
		public delegate void Action();
		
		public static Action OnBillingNotSupported = delegate {};
		
		public static Action OnBillingSupported = delegate {};
				
		public static Action<VirtualCurrency, int, int> OnCurrencyBalanceChanged = delegate {};
		
		public static Action<VirtualGood, int, int> OnGoodBalanceChanged = delegate {};
		
		public static Action<EquippableVG> OnGoodEquipped = delegate {};
		
		public static Action<EquippableVG> OnGoodUnEquipped = delegate {};
		
		public static Action<VirtualGood, UpgradeVG> OnGoodUpgrade = delegate {};
		
		public static Action<PurchasableVirtualItem> OnItemPurchased = delegate {};
		
		public static Action<PurchasableVirtualItem> OnItemPurchaseStarted = delegate {};
				
		public static Action<PurchasableVirtualItem> OnMarketPurchaseCancelled = delegate {};	
		
		public static Action<PurchasableVirtualItem> OnMarketPurchase = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketPurchaseStarted = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketRefund = delegate {};
		
		public static Action<bool> OnRestoreTransactions = delegate {};
		
		public static Action OnRestoreTransactionsStarted = delegate {};
		
		public static Action<string> OnUnexpectedErrorInStore = delegate {};
		
		public static Action OnStoreControllerInitialized = delegate {};

#if UNITY_ANDROID && !UNITY_EDITOR
        public static Action<MarketItemDetails, bool> OnItemDetailsRetrieved = delegate { };

        public static Action OnItemDetailsRetrievedFailed = delegate { };

		public static Action OnIabServiceStarted = delegate {};
		
		public static Action OnIabServiceStopped = delegate {};
#endif
		
	}
}
using System;
using UnityEngine;

namespace com.soomla.unity
{
	public class Events
	{
		public delegate void Action();
		
		public static Action<MarketItem> OnMarketPurchase = delegate {};
		
		public static Action<MarketItem> OnMarketRefund = delegate {};
		
		public static Action<VirtualGood> OnVirtualGoodPurchased = delegate {};
		
		public static Action<VirtualGood> OnVirtualGoodEquipped = delegate {};
		
		public static Action<VirtualGood> OnVirtualGoodUnEquipped = delegate {};
		
		public static Action OnBillingSupported = delegate {};
		
		public static Action OnBillingNotSupported = delegate {};
		
		public static Action<MarketItem> OnMarketPurchaseProcessStarted = delegate {};
		
		public static Action OnGoodsPurchaseProcessStarted = delegate {};
		
		public static Action OnClosingStore = delegate {};
		
		public static Action OnOpeningStore = delegate {};
		
		public static Action OnUnexpectedErrorInStore = delegate {};
		
		public static Action<VirtualCurrency, int> OnCurrencyBalanceChanged = delegate {};
		
		public static Action<VirtualGood, int> OnGoodBalanceChanged = delegate {};
		
		public static Action<MarketItem> OnMarketPurchaseCancelled = delegate {};
	}
}
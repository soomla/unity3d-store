using System;

namespace com.soomla.unity.example
{
	public class ExampleEventHandler
	{
		
		public ExampleEventHandler ()
		{
			Events.OnMarketPurchase += onMarketPurchase;
			Events.OnMarketRefund += onMarketRefund;
			Events.OnItemPurchased += onItemPurchased;
			Events.OnGoodEquipped += onGoodEquipped;
			Events.OnGoodUnEquipped += onGoodUnequipped;
			Events.OnGoodUpgrade += onGoodUpgrade;
			Events.OnBillingSupported += onBillingSupported;
			Events.OnBillingNotSupported += onBillingNotSupported;
			Events.OnMarketPurchaseStarted += onMarketPurchaseStarted;
			Events.OnItemPurchaseStarted += onItemPurchaseStarted;
			Events.OnClosingStore += onClosingStore;
			Events.OnOpeningStore += onOpeningStore;
			Events.OnUnexpectedErrorInStore += onUnexpectedErrorInStore;
			Events.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
			Events.OnGoodBalanceChanged += onGoodBalanceChanged;
			Events.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
			Events.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
			Events.OnRestoreTransactions += onRestoreTransactions;
		}
		
		public void onMarketPurchase(PurchasableVirtualItem pvi, string transactionReceipt) {
			
		}
		
		public void onMarketRefund(PurchasableVirtualItem pvi) {

		}
		
		public void onItemPurchased(PurchasableVirtualItem pvi) {

		}
		
		public void onGoodEquipped(EquippableVG good) {
			
		}
		
		public void onGoodUnequipped(EquippableVG good) {
			
		}
		
		public void onGoodUpgrade(VirtualGood good, UpgradeVG currentUpgrade) {
			
		}
		
		public void onBillingSupported() {
			
		}
		
		public void onBillingNotSupported() {
			
		}
		
		public void onMarketPurchaseStarted(PurchasableVirtualItem pvi) {
			
		}
		
		public void onItemPurchaseStarted(PurchasableVirtualItem pvi) {
			
		}
		
		public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi) {
			
		}
		
		public void onClosingStore() {
			
		}
		
		public void onUnexpectedErrorInStore() {
			
		}
		
		public void onOpeningStore() {
			
		}
		
		public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
			ExampleLocalStoreInfo.UpdateBalances();
		}
		
		public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded) {
			ExampleLocalStoreInfo.UpdateBalances();
		}
		
		public void onRestoreTransactionsStarted() {
			
		}
		
		public void onRestoreTransactions(bool success) {
			
		}
	}
}


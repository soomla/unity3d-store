using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace com.soomla.unity {
	public class Soomla : MonoBehaviour {
		
		public static string AND_PUB_KEY_DEFAULT = "YOUR GOOGLE PLAY PUBLIC KEY";
		public static string ONLY_ONCE_DEFAULT = "SET ONLY ONCE";
		
		private const string TAG = "SOOMLA Soomla";
		private static Soomla instance = null;
		
		void Awake(){
			if(instance == null){ 	//making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} else {					//Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}
		
		public string customSecret = ONLY_ONCE_DEFAULT;
		public string androidPublicKey = AND_PUB_KEY_DEFAULT;
		public bool androidTestMode = false;
		public string soomSec = ONLY_ONCE_DEFAULT;
		
		public static Soomla GetInstance(){
			return instance;
		}
		
		public void onBillingSupported(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingSupported");
			
			Events.OnBillingSupported();
		}
	
		
		public void onBillingNotSupported(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingNotSupported");
			
			Events.OnBillingNotSupported();
		}
		
		public void onClosingStore(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onClosingStore");
			
			Events.OnClosingStore();
		}
		
		public void onCurrencyBalanceChanged(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onCurrencyBalanceChanged:" + message);
			
			string[] vars = Regex.Split(message, "#SOOM#");
			
			VirtualCurrency vc = (VirtualCurrency)StoreInfo.GetItemByItemId(vars[0]);
			int balance = int.Parse(vars[1]);
			int amountAdded = int.Parse(vars[2]);
			Events.OnCurrencyBalanceChanged(vc, balance, amountAdded);
		}
		
		public void onGoodBalanceChanged(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onGoodBalanceChanged:" + message);
			
			string[] vars = Regex.Split(message, "#SOOM#");
			
			VirtualGood vg = (VirtualGood)StoreInfo.GetItemByItemId(vars[0]);
			int balance = int.Parse(vars[1]);
			int amountAdded = int.Parse(vars[2]);
			Events.OnGoodBalanceChanged(vg, balance, amountAdded);
		}
		
		public void onGoodEquipped(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onVirtualGoodEquipped:" + message);
			
			EquippableVG vg = (EquippableVG)StoreInfo.GetItemByItemId(message);
			Events.OnGoodEquipped(vg);
		}
	
		public void onGoodUnequipped(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onVirtualGoodUnEquipped:" + message);
			
			EquippableVG vg = (EquippableVG)StoreInfo.GetItemByItemId(message);
			Events.OnGoodUnEquipped(vg);
		}
		
		public void onGoodUpgrade(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onGoodUpgrade:" + message);
			
			string[] vars = Regex.Split(message, "#SOOM#");
			
			VirtualGood vg = (VirtualGood)StoreInfo.GetItemByItemId(vars[0]);
			UpgradeVG vgu = (UpgradeVG)StoreInfo.GetItemByItemId(vars[1]);
			Events.OnGoodUpgrade(vg, vgu);
		}
		
		public void onItemPurchased(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onItemPurchased:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			Events.OnItemPurchased(pvi);
		}
		
		public void onItemPurchaseStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onItemPurchaseStarted:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			Events.OnItemPurchaseStarted(pvi);
		}
		
		public void onOpeningStore(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onOpeningStore");
			
			Events.OnOpeningStore();
		}
		
		public void onMarketPurchaseCancelled(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketPurchaseCancelled: " + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			Events.OnMarketPurchaseCancelled(pvi);
		}

		public void onMarketPurchase(string message) {
			Debug.Log ("SOOMLA/UNITY onMarketPurchase:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			Events.OnMarketPurchase(pvi);
		}
		
		public void onMarketPurchaseStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketPurchaseStarted: " + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			Events.OnMarketPurchaseStarted(pvi);
		}
		
		public void onMarketRefund(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketRefund:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			Events.OnMarketPurchaseStarted(pvi);
		}
		
		public void onRestoreTransactions(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onRestoreTransactions:" + message);
			
			bool success = Convert.ToBoolean(int.Parse(message));
			Events.OnRestoreTransactions(success);
		}
		
		public void onRestoreTransactionsStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onRestoreTransactionsStarted");
			
			Events.OnRestoreTransactionsStarted();
		}

		public void onUnexpectedErrorInStore(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onUnexpectedErrorInStore");
			
			Events.OnUnexpectedErrorInStore();
		}

	}
}

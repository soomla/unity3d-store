using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace Soomla {
	public class StoreEvents : MonoBehaviour {

		private const string TAG = "SOOMLA StoreEvents";
		
		private static StoreEvents instance = null;
		
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
			} else {				// Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}
				
		public void onBillingSupported(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingSupported");
			
			StoreEvents.OnBillingSupported();
		}
	
		
		public void onBillingNotSupported(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingNotSupported");
			
			StoreEvents.OnBillingNotSupported();
		}
		
		public void onCurrencyBalanceChanged(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onCurrencyBalanceChanged:" + message);
			
			string[] vars = Regex.Split(message, "#SOOM#");
			
			VirtualCurrency vc = (VirtualCurrency)StoreInfo.GetItemByItemId(vars[0]);
			int balance = int.Parse(vars[1]);
			int amountAdded = int.Parse(vars[2]);
			StoreEvents.OnCurrencyBalanceChanged(vc, balance, amountAdded);
		}
		
		public void onGoodBalanceChanged(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onGoodBalanceChanged:" + message);
			
			string[] vars = Regex.Split(message, "#SOOM#");
			
			VirtualGood vg = (VirtualGood)StoreInfo.GetItemByItemId(vars[0]);
			int balance = int.Parse(vars[1]);
			int amountAdded = int.Parse(vars[2]);
			StoreEvents.OnGoodBalanceChanged(vg, balance, amountAdded);
		}
		
		public void onGoodEquipped(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onVirtualGoodEquipped:" + message);
			
			EquippableVG vg = (EquippableVG)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnGoodEquipped(vg);
		}
	
		public void onGoodUnequipped(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onVirtualGoodUnEquipped:" + message);
			
			EquippableVG vg = (EquippableVG)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnGoodUnEquipped(vg);
		}
		
		public void onGoodUpgrade(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onGoodUpgrade:" + message);
			
			string[] vars = Regex.Split(message, "#SOOM#");
			
			VirtualGood vg = (VirtualGood)StoreInfo.GetItemByItemId(vars[0]);
			UpgradeVG vgu = (UpgradeVG)StoreInfo.GetItemByItemId(vars[1]);
			StoreEvents.OnGoodUpgrade(vg, vgu);
		}
		
		public void onItemPurchased(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onItemPurchased:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnItemPurchased(pvi);
		}
		
		public void onItemPurchaseStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onItemPurchaseStarted:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnItemPurchaseStarted(pvi);
		}
		
		public void onMarketPurchaseCancelled(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketPurchaseCancelled: " + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnMarketPurchaseCancelled(pvi);
		}

		public void onMarketPurchase(string message) {
			Debug.Log ("SOOMLA/UNITY onMarketPurchase:" + message);

			string[] vars = Regex.Split(message, "#SOOM#");
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(vars[0]);
			string payload = "";
			string purchaseToken = "";
			if (vars.Length > 1) {
				payload = vars[1];
			}
			if (vars.Length > 2) {
				purchaseToken = vars[2];
			}

			StoreEvents.OnMarketPurchase(pvi, purchaseToken);
		}
		
		public void onMarketPurchaseStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketPurchaseStarted: " + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnMarketPurchaseStarted(pvi);
		}
		
		public void onMarketRefund(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketRefund:" + message);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnMarketPurchaseStarted(pvi);
		}
		
		public void onRestoreTransactionsFinished(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onRestoreTransactionsFinished:" + message);
			
			bool success = Convert.ToBoolean(int.Parse(message));
			StoreEvents.OnRestoreTransactionsFinished(success);
		}
		
		public void onRestoreTransactionsStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onRestoreTransactionsStarted");
			
			StoreEvents.OnRestoreTransactionsStarted();
		}

		public void onMarketItemsRefreshed(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketItemsRefreshed: " + message);

			string[] marketItemsChanges = Regex.Split(message, "#SOOM#");
			foreach (string mic in marketItemsChanges) {
				JSONObject micJSON = new JSONObject(mic);
				string productId = micJSON["productId"].str;
				string marketPrice = micJSON["market_price"].str;
				string marketTitle = micJSON["market_title"].str;
				string marketDescription = micJSON["market_desc"].str;
				try {
					PurchasableVirtualItem pvi = StoreInfo.GetPurchasableItemWithProductId(productId);
					MarketItem mi = ((PurchaseWithMarket)pvi.PurchaseType).MarketItem;
					mi.MarketPrice = marketPrice;
					mi.MarketTitle = marketTitle;
					mi.MarketDescription = marketDescription;
				} catch (VirtualItemNotFoundException ex){
					StoreUtils.LogDebug(TAG, ex.Message);
				}
			}
		
			StoreEvents.OnMarketItemsRefreshed();
		}

		public void onUnexpectedErrorInStore(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onUnexpectedErrorInStore");
			
			StoreEvents.OnUnexpectedErrorInStore(message);
		}
		
		public void onStoreControllerInitialized(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onStoreControllerInitialized");
			
			StoreEvents.OnStoreControllerInitialized();
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public void onIabServiceStarted(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onIabServiceStarted");
			
			StoreEvents.OnIabServiceStarted();
		}
		
		public void onIabServiceStopped(string message) {
			StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onIabServiceStopped");
			
			StoreEvents.OnIabServiceStopped();
		}
#endif


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
		
		public static Action<PurchasableVirtualItem, string> OnMarketPurchase = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketPurchaseStarted = delegate {};
		
		public static Action<PurchasableVirtualItem> OnMarketRefund = delegate {};
		
		public static Action<bool> OnRestoreTransactionsFinished = delegate {};
		
		public static Action OnRestoreTransactionsStarted = delegate {};
		
		public static Action OnMarketItemsRefreshed = delegate {};
		
		public static Action<string> OnUnexpectedErrorInStore = delegate {};
		
		public static Action OnStoreControllerInitialized = delegate {};
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		public static Action OnIabServiceStarted = delegate {};
		
		public static Action OnIabServiceStopped = delegate {};
		#endif

	}
}

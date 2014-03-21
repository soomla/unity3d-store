using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Soomla;

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
		
		Events.OnBillingSupported();
	}

	
	public void onBillingNotSupported(string message) {
		StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingNotSupported");
		
		Events.OnBillingNotSupported();
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
		
		Events.OnUnexpectedErrorInStore(message);
	}
	
	public void onStoreControllerInitialized(string message) {
		StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onStoreControllerInitialized");
		
		Events.OnStoreControllerInitialized();
    }

#if UNITY_ANDROID && !UNITY_EDITOR
        public void onItemDetailsRetrieved(string message)
        {
            StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onItemDetailsRetrieved");

            string[] vars = Regex.Split(message, "#SOOM#");

            Events.OnItemDetailsRetrieved(new MarketItemDetails(vars[0], vars[1], vars[2], vars[3]), Boolean.Parse(vars[4]));
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        public void onItemDetailsRetrievedFailedEvent(string message)
        {
            StoreUtils.LogDebug(TAG, "SOOMLA/UNITY ItemDetailsRetrievedFailedEvent");

            Events.OnItemDetailsRetrievedFailed();
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
	public void onIabServiceStarted(string message) {
		StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onIabServiceStarted");
		
		Events.OnIabServiceStarted();
	}
	
	public void onIabServiceStopped(string message) {
		StoreUtils.LogDebug(TAG, "SOOMLA/UNITY onIabServiceStopped");
		
		Events.OnIabServiceStopped();
	}
#endif

}

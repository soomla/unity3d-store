/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Soomla.Store {

	/// <summary>
	/// This class provides functions for event handling.
	/// </summary>
	public class StoreEvents : MonoBehaviour {

		private const string TAG = "SOOMLA StoreEvents";

		private static StoreEvents instance = null;

		/// <summary>
		/// Initializes game state before the game starts.
		/// </summary>
		void Awake(){
			if(instance == null){ 	// making sure we only initialize one instance.
				instance = this;
				GameObject.DontDestroyOnLoad(this.gameObject);
				Initialize();
			} else {				// Destroying unused instances.
				GameObject.Destroy(this.gameObject);
			}
		}

		public static void Initialize() {
			SoomlaUtils.LogDebug (TAG, "Initializing StoreEvents ...");
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			//init EventHandler
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.unity.StoreEventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			#elif UNITY_IOS && !UNITY_EDITOR
			// On iOS, this is initialized inside the bridge library when we call "soomlaStore_Init" in SoomlaStoreIOS
			#endif
		}

		/// <summary>
		/// Handles an <c>onBillingSupported</c> event, which is fired when SOOMLA knows that billing IS
		/// supported on the device.
		/// </summary>
		/// <param name="message">Not used here.</param>
		public void onBillingSupported(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingSupported");

			StoreEvents.OnBillingSupported();
		}

		/// <summary>
		/// Handles an <c>onBillingNotSupported</c> event, which is fired when SOOMLA knows that billing is NOT
		/// supported on the device.
		/// </summary>
		/// <param name="message">Not used here.</param>
		public void onBillingNotSupported(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onBillingNotSupported");

			StoreEvents.OnBillingNotSupported();
		}

		/// <summary>
		/// Handles an <c>onCurrencyBalanceChanged</c> event, which is fired when the balance of a specific
		/// <c>VirtualCurrency</c> has changed.
		/// </summary>
		/// <param name="message">Message that contains information about the currency whose balance has
		/// changed.</param>
		public void onCurrencyBalanceChanged(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onCurrencyBalanceChanged:" + message);

			string[] vars = Regex.Split(message, "#SOOM#");

			VirtualCurrency vc = (VirtualCurrency)StoreInfo.GetItemByItemId(vars[0]);
			int balance = int.Parse(vars[1]);
			int amountAdded = int.Parse(vars[2]);
			StoreEvents.OnCurrencyBalanceChanged(vc, balance, amountAdded);
		}

		/// <summary>
		/// Handles an <c>onGoodBalanceChanged</c> event, which is fired when the balance of a specific
		/// <c>VirtualGood</c> has changed.
		/// </summary>
		/// <param name="message">Message that contains information about the good whose balance has
		/// changed.</param>
		public void onGoodBalanceChanged(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGoodBalanceChanged:" + message);

			string[] vars = Regex.Split(message, "#SOOM#");

			VirtualGood vg = (VirtualGood)StoreInfo.GetItemByItemId(vars[0]);
			int balance = int.Parse(vars[1]);
			int amountAdded = int.Parse(vars[2]);
			StoreEvents.OnGoodBalanceChanged(vg, balance, amountAdded);
		}

		/// <summary>
		/// Handles an <c>onGoodEquipped</c> event, which is fired when a specific <c>EquippableVG</c> has been
		/// equipped.
		/// </summary>
		/// <param name="message">Message that contains information about the <c>EquippableVG</c>.</param>
		public void onGoodEquipped(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onVirtualGoodEquipped:" + message);

			EquippableVG vg = (EquippableVG)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnGoodEquipped(vg);
		}

		/// <summary>
		/// Handles an <c>onGoodUnequipped</c> event, which is fired when a specific <c>EquippableVG</c>
		/// has been unequipped.
		/// </summary>
		/// <param name="message">Message that contains information about the <c>EquippableVG</c>.</param>
		public void onGoodUnequipped(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onVirtualGoodUnEquipped:" + message);

			EquippableVG vg = (EquippableVG)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnGoodUnEquipped(vg);
		}

		/// <summary>
		/// Handles an <c>onGoodUpgrade</c> event, which is fired when a specific <c>UpgradeVG</c> has
		/// been upgraded/downgraded.
		/// </summary>
		/// <param name="message">Message that contains information about the good that has been
		/// upgraded/downgraded.</param>
		public void onGoodUpgrade(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onGoodUpgrade:" + message);

			string[] vars = Regex.Split(message, "#SOOM#");

			VirtualGood vg = (VirtualGood)StoreInfo.GetItemByItemId(vars[0]);
			UpgradeVG vgu = null;
			if (vars.Length > 1) {
				vgu = (UpgradeVG)StoreInfo.GetItemByItemId(vars[1]);
		  }
			StoreEvents.OnGoodUpgrade(vg, vgu);
		}

		/// <summary>
		/// Handles an <c>onItemPurchased</c> event, which is fired when a specific
		/// <c>PurchasableVirtualItem</c> has been purchased.
		/// </summary>
		/// <param name="message">Message that contains information about the good that has been purchased.</param>
		public void onItemPurchased(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onItemPurchased:" + message);

			string[] vars = Regex.Split(message, "#SOOM#");
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(vars[0]);
			string payload = "";
			if (vars.Length > 1) {
				payload = vars[1];
			}

			StoreEvents.OnItemPurchased(pvi, payload);
		}

		/// <summary>
		/// Handles the <c>onItemPurchaseStarted</c> event, which is fired when a specific
		/// <c>PurchasableVirtualItem</c> purchase process has started.
		/// </summary>
		/// <param name="message">Message that contains information about the item being purchased.</param>
		public void onItemPurchaseStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onItemPurchaseStarted:" + message);

			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnItemPurchaseStarted(pvi);
		}

		/// <summary>
		/// Handles the <c>onMarketPurchaseCancelled</c> event, which is fired when a Market purchase was cancelled
		/// by the user.
		/// </summary>
		/// <param name="message">Message that contains information about the market purchase that is being
		/// cancelled.</param>
		public void onMarketPurchaseCancelled(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketPurchaseCancelled: " + message);

			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnMarketPurchaseCancelled(pvi);
		}

		/// <summary>
		/// Handles the <c>onMarketPurchase</c> event, which is fired when a Market purchase has occurred.
		/// </summary>
		/// <param name="message">Message that contains information about the market purchase.</param>
		public void onMarketPurchase(string message) {
			Debug.Log ("SOOMLA/UNITY onMarketPurchase:" + message);

			string[] vars = Regex.Split(message, "#SOOM#");

			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(vars[0]);
			string payload = "";
			string purchaseToken = "";
			string orderId = "";
			if (vars.Length > 1) {
				payload = vars[1];
			}
			if (vars.Length > 2) {
				purchaseToken = vars[2];
			}
			if (vars.Length > 3) {
				orderId = vars[3];
			}

			StoreEvents.OnMarketPurchase(pvi, purchaseToken, payload, orderId);
		}

		/// <summary>
		/// Handles the <c>onMarketPurchaseStarted</c> event, which is fired when a Market purchase has started.
		/// </summary>
		/// <param name="message">Message that contains information about the maret purchase that is being
		/// started.</param>
		public void onMarketPurchaseStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketPurchaseStarted: " + message);

			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnMarketPurchaseStarted(pvi);
		}

		/// <summary>
		/// Handles the <c>onMarketRefund</c> event, which is fired when a Market refund has been issued.
		/// </summary>
		/// <param name="message">Message that contains information about the market refund that has occurred.</param>
		public void onMarketRefund(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketRefund:" + message);

			PurchasableVirtualItem pvi = (PurchasableVirtualItem)StoreInfo.GetItemByItemId(message);
			StoreEvents.OnMarketPurchaseStarted(pvi);
		}

		/// <summary>
		/// Handles the <c>onRestoreTransactionsFinished</c> event, which is fired when the restore transactions
		/// process has finished.
		/// </summary>
		/// <param name="message">Message that contains information about the <c>restoreTransactions</c> process that
		/// has finished.</param>
		public void onRestoreTransactionsFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRestoreTransactionsFinished:" + message);

			bool success = Convert.ToBoolean(int.Parse(message));
			StoreEvents.OnRestoreTransactionsFinished(success);
		}

		/// <summary>
		/// Handles the <c>onRestoreTransactionsStarted</c> event, which is fired when the restore transactions
		/// process has started.
		/// </summary>
		/// <param name="message">Message that contains information about the <c>restoreTransactions</c> process that
		/// has started.</param>
		public void onRestoreTransactionsStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onRestoreTransactionsStarted");

			StoreEvents.OnRestoreTransactionsStarted();
		}

		/// <summary>
		/// Handles the <c>onMarketItemsRefreshStarted</c> event, which is fired when items associated with market
		/// refresh process has started.
		/// </summary>
		/// <param name="message">Message that contains information about the <c>market refresh</c> process that
		/// has started.</param>
		public void onMarketItemsRefreshStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketItemsRefreshStarted");

			StoreEvents.OnMarketItemsRefreshStarted();
		}

		/// <summary>
		/// Handles the <c>onMarketItemsRefreshFinished</c> event, which is fired when items associated with market are
		/// refreshed (prices, titles ...).
		/// </summary>
		/// <param name="message">Message that contains information about the process that is occurring.</param>
		public void onMarketItemsRefreshFinished(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onMarketItemsRefreshFinished: " + message);

			string[] marketItemsChanges = Regex.Split(message, "#SOOM#");
			List<MarketItem> marketItems = new List<MarketItem>();
			foreach (string mic in marketItemsChanges) {
				if (string.IsNullOrEmpty(mic.Trim())) {
					continue;
				}

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
					pvi.save();

					marketItems.Add(mi);
				} catch (VirtualItemNotFoundException ex){
					SoomlaUtils.LogDebug(TAG, ex.Message);
				}
			}

			StoreEvents.OnMarketItemsRefreshFinished(marketItems);
		}

		/// <summary>
		/// Handles the <c>onItemPurchaseStarted</c> event, which is fired when an unexpected/unrecognized error
		/// occurs in store.
		/// </summary>
		/// <param name="message">Message that contains information about the error.</param>
		public void onUnexpectedErrorInStore(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onUnexpectedErrorInStore");

			StoreEvents.OnUnexpectedErrorInStore(message);
		}

		/// <summary>
		/// Handles the <c>onSoomlaStoreInitialized</c> event, which is fired when <c>SoomlaStore</c>
		/// is initialized.
		/// </summary>
		/// <param name="message">Not used here.</param>
		public void onSoomlaStoreInitialized(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onSoomlaStoreInitialized");

			StoreEvents.OnSoomlaStoreInitialized();
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		public void onIabServiceStarted(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onIabServiceStarted");

			StoreEvents.OnIabServiceStarted();
		}

		public void onIabServiceStopped(string message) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY onIabServiceStopped");

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

		public static Action<PurchasableVirtualItem, string> OnItemPurchased = delegate {};

		public static Action<PurchasableVirtualItem> OnItemPurchaseStarted = delegate {};

		public static Action<PurchasableVirtualItem> OnMarketPurchaseCancelled = delegate {};

		public static Action<PurchasableVirtualItem, string, string, string> OnMarketPurchase = delegate {};

		public static Action<PurchasableVirtualItem> OnMarketPurchaseStarted = delegate {};

		public static Action<PurchasableVirtualItem> OnMarketRefund = delegate {};

		public static Action<bool> OnRestoreTransactionsFinished = delegate {};

		public static Action OnRestoreTransactionsStarted = delegate {};

		public static Action OnMarketItemsRefreshStarted = delegate {};

		public static Action<List<MarketItem>> OnMarketItemsRefreshFinished = delegate {};

		public static Action<string> OnUnexpectedErrorInStore = delegate {};

		public static Action OnSoomlaStoreInitialized = delegate {};

		#if UNITY_ANDROID && !UNITY_EDITOR
		public static Action OnIabServiceStarted = delegate {};

		public static Action OnIabServiceStopped = delegate {};
		#endif

	}
}

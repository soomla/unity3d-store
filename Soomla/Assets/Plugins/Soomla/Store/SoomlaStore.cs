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
using System.Runtime.InteropServices;

namespace Soomla.Store
{
	/// <summary>
	/// This class holds the basic assets needed to operate the Store.
	/// You can use it to purchase products from the mobile store.
	/// This is the only class you need to initialize in order to use the SOOMLA SDK.
	/// </summary>
	public class SoomlaStore
	{

		static SoomlaStore _instance = null;
		static SoomlaStore instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new SoomlaStoreAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new SoomlaStoreIOS();
					#else
					_instance = new SoomlaStore();
					#endif
				}
				return _instance;
			}
		}

		/// <summary>
		/// Initializes the SOOMLA SDK.
		/// </summary>
		/// <param name="storeAssets">Your game's economy.</param>
		/// <exception cref="ExitGUIException">Thrown if soomlaSecret is missing or has not been changed.
		/// </exception>
		public static void Initialize(IStoreAssets storeAssets) {
			if (string.IsNullOrEmpty(CoreSettings.SoomlaSecret)) {
				SoomlaUtils.LogError(TAG, "SOOMLA/UNITY MISSING SoomlaSecret !!! Stopping here !!");
				throw new ExitGUIException();
			}

			if (CoreSettings.SoomlaSecret==CoreSettings.ONLY_ONCE_DEFAULT) {
				SoomlaUtils.LogError(TAG, "SOOMLA/UNITY You have to change SoomlaSecret !!! Stopping here !!");
				throw new ExitGUIException();
			}

			instance._initialize(storeAssets);
		}

		/// <summary>
		/// Starts a purchase process in the market.
		/// </summary>
		/// <param name="productId">id of the item to buy.</param>
		/// <param name="payload">some text you want to get back when the purchasing process is completed. NOTE: This is not supported on iOS !</param>
		public static void BuyMarketItem(string productId, string payload) {

			// NOTE: payload is not supported on iOS !

			instance._buyMarketItem(productId, payload);
		}

		/// <summary>
		/// This method will run RestoreTransactions followed by RefreshMarketItemsDetails
		/// </summary>
		public static void RefreshInventory() {
			instance._refreshInventory();
		}

		/// <summary>
		/// Creates a list of all metadata stored in the Market (the items that have been purchased).
		/// The metadata includes the item's name, description, price, product id, etc...
		/// Posts a <c>MarketItemsRefreshed</c> event with the list just created.
		/// Upon failure, prints error message.
		/// </summary>
		public static void RefreshMarketItemsDetails() {
			instance._refreshMarketItemsDetails();
		}

		/// <summary>
		/// Initiates the restore transactions process.
		/// </summary>
		public static void RestoreTransactions() {
			instance._restoreTransactions();
		}

		/// <summary>
		/// Checks if transactions were already restored.
		/// </summary>
		/// <returns><c>true</c> if transactions were already restored, <c>false</c> otherwise.</returns>
		public static bool TransactionsAlreadyRestored() {
			return instance._transactionsAlreadyRestored();
		}

		/// <summary>
		/// Starts in-app billing service in background.
		/// </summary>
		public static void StartIabServiceInBg() {
			instance._startIabServiceInBg();
		}

		/// <summary>
		/// Stops in-app billing service in background.
		/// </summary>
		public static void StopIabServiceInBg() {
			instance._stopIabServiceInBg();
		}


		protected virtual void _initialize(IStoreAssets storeAssets) { 
			StoreInfo.Initialize(storeAssets);

			var storeEvents = GameObject.FindObjectOfType<StoreEvents> ();
			if (storeEvents != null) {
				storeEvents.onSoomlaStoreInitialized(null);
			}
			else {
				Debug.LogError("SOOMLA/UNITY StoreEvents Component not found in scene");
			}
		}

		protected virtual void _buyMarketItem(string productId, string payload) {
			PurchasableVirtualItem item = StoreInfo.GetPurchasableItemWithProductId(productId);
			if (item == null) {
				throw new VirtualItemNotFoundException("ProductId", productId);
			}

			StoreInventory.BuyItem(item.ItemId, payload);
		}

		protected virtual void _refreshInventory() { }

		protected virtual void _restoreTransactions() { }

		protected virtual void _refreshMarketItemsDetails() { }

		protected virtual bool _transactionsAlreadyRestored() {
			return true;
		}

		protected virtual void _startIabServiceInBg() { }

		protected virtual void _stopIabServiceInBg() { }


		/// <summary> Class Members </summary>

		protected const string TAG = "SOOMLA SoomlaStore";

	}
}

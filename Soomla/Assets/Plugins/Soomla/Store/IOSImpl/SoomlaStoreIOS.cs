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
using System.Runtime.InteropServices;

namespace Soomla.Store {

	/// <summary>
	/// <c>StoreController</c> for iOS.
	/// This class holds the basic assets needed to operate the Store.
	/// You can use it to purchase products from the mobile store.
	/// This is the only class you need to initialize in order to use the SOOMLA SDK.
	/// </summary>
	public class SoomlaStoreIOS : SoomlaStore {
#if UNITY_IOS && !UNITY_EDITOR

		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern void soomlaStore_Init();
		[DllImport ("__Internal")]
		private static extern int soomlaStore_BuyMarketItem(string productId);
		[DllImport ("__Internal")]
		private static extern void soomlaStore_RestoreTransactions();
		[DllImport ("__Internal")]
		private static extern void soomlaStore_RefreshInventory();
		[DllImport ("__Internal")]
		private static extern void soomlaStore_RefreshMarketItemsDetails();
		[DllImport ("__Internal")]
		private static extern void soomlaStore_TransactionsAlreadyRestored(out bool outResult);
		[DllImport ("__Internal")]
		private static extern void soomlaStore_SetSSV(bool ssv, string verifyUrl);


		/// <summary>
		/// Initializes the SOOMLA SDK.
		/// </summary>
		/// <param name="storeAssets">Your game's economy.</param>
		protected override void _initialize(IStoreAssets storeAssets) {
			soomlaStore_SetSSV(StoreSettings.IosSSV, "https://verify.soom.la/verify_ios?platform=unity4");
			StoreInfo.Initialize(storeAssets);
			soomlaStore_Init();
		}

		/// <summary>
		/// Starts a purchase process in the market.
		/// </summary>
		/// <param name="productId">id of the item to buy.</param>
		protected override void _buyMarketItem(string productId, string payload) {

			// NOTE: payload is not supported on iOS !

			soomlaStore_BuyMarketItem(productId);
		}

		/// <summary>
		/// This method will run _restoreTransactions followed by _refreshMarketItemsDetails.
		/// </summary>
		protected override void _refreshInventory() {
			soomlaStore_RefreshInventory();
		}

		/// <summary>
		/// Initiates the restore transactions process.
		/// </summary>
		protected override void _restoreTransactions() {
			soomlaStore_RestoreTransactions();
		}

		/// <summary>
		/// Creates a list of all metadata stored in the Market (the items that have been purchased).
		/// The metadata includes the item's name, description, price, product id, etc...
		/// </summary>
		protected override void _refreshMarketItemsDetails() {
			soomlaStore_RefreshMarketItemsDetails();
		}

		/// <summary>
		/// Checks if transactions were already restored.
		/// <returns>true if transactions were restored, false otherwise.</returns>
		/// </summary>
		protected override bool _transactionsAlreadyRestored() {
			bool restored = false;
			soomlaStore_TransactionsAlreadyRestored(out restored);
			return restored;
		}
#endif
	}
}

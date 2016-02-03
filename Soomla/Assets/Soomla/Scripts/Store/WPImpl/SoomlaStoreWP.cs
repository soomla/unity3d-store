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

namespace Soomla.Store {

	/// <summary>
	/// <c>SoomlaStore</c> for Android. 
	/// This class holds the basic assets needed to operate the Store.
	/// You can use it to purchase products from the mobile store.
	/// This is the only class you need to initialize in order to use the SOOMLA SDK.
	/// </summary>
	public class SoomlaStoreWP : SoomlaStore {

#if UNITY_WP8 && !UNITY_EDITOR
		/// <summary>
		/// Load the billing service.
		/// </summary>
        protected override void _loadBillingService()
        {
            SoomlaWpStore.StoreConfig.STORE_TEST_MODE = StoreSettings.WP8TestMode;
            SoomlaWpCore.SoomlaConfig.logDebug = CoreSettings.DebugMessages;
            SoomlaWpStore.SoomlaStore.GetInstance().initStoreManager();
        }

		/// <summary>
		/// Starts a purchase process in the market.
		/// </summary>
		/// <param name="productId">id of the item to buy.</param>
		protected override void _buyMarketItem(string productId, string payload) {
            SoomlaWpStore.domain.PurchasableVirtualItem wpPVI = SoomlaWpStore.data.StoreInfo.getPurchasableItem(productId);
            if (wpPVI.GetPurchaseType() is SoomlaWpStore.purchasesTypes.PurchaseWithMarket)
            {
                SoomlaWpStore.purchasesTypes.PurchaseWithMarket wpPWM = (SoomlaWpStore.purchasesTypes.PurchaseWithMarket)wpPVI.GetPurchaseType();
                SoomlaWpStore.SoomlaStore.GetInstance().buyWithMarket(
                    wpPWM.getMarketItem(),
                    payload
                );
            }
            
		}

		/// <summary>
		/// This method will run _restoreTransactions followed by _refreshMarketItemsDetails.
		/// </summary>
		protected override void _refreshInventory() {
			SoomlaWpStore.SoomlaStore.GetInstance().refreshInventory();
		}

		/// <summary>
		/// Creates a list of all metadata stored in the Market (the items that have been purchased).
		/// The metadata includes the item's name, description, price, product id, etc...
		/// </summary>
		protected override void _refreshMarketItemsDetails() {
            SoomlaWpStore.billing.wp.store.StoreManager.GetInstance().LoadListingInfo();
		}

		/// <summary>
		/// Initiates the restore transactions process.
		/// </summary>
		protected override void _restoreTransactions() {
            SoomlaWpStore.SoomlaStore.GetInstance().restoreTransactions();
		}
#endif
                                             }
}

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
	public class SoomlaStoreAndroid : SoomlaStore {

#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaObject jniSoomlaStore = null;

		/// <summary>
		/// Initializes the SOOMLA SDK.
		/// </summary>
		/// <param name="storeAssets">Your game's economy.</param>
		/// <exception cref="ExitGUIException">Thrown if soomlaSecret is missing or has not been changed.
		/// </exception>
		protected override void _initialize(IStoreAssets storeAssets) {
			if (StoreSettings.GPlayBP && 
			    (string.IsNullOrEmpty(StoreSettings.AndroidPublicKey) ||
			 		StoreSettings.AndroidPublicKey==StoreSettings.AND_PUB_KEY_DEFAULT)) {
				SoomlaUtils.LogError(TAG, "SOOMLA/UNITY You chose Google Play billing service but publicKey is not set!! Stopping here!!");
				throw new ExitGUIException();
			}

			StoreInfo.Initialize(storeAssets);

			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniStoreAssetsInstance = new AndroidJavaObject("com.soomla.unity.StoreAssets")) {
				using(AndroidJavaClass jniSoomlaStoreClass = new AndroidJavaClass("com.soomla.store.SoomlaStore")) {
					jniSoomlaStore = jniSoomlaStoreClass.CallStatic<AndroidJavaObject>("getInstance");
					jniSoomlaStore.Call<bool>("initialize", jniStoreAssetsInstance);
				}
			}

			if (StoreSettings.GPlayBP) {
				using(AndroidJavaClass jniGooglePlayIabServiceClass = new AndroidJavaClass("com.soomla.store.billing.google.GooglePlayIabService")) {
					AndroidJavaObject jniGooglePlayIabService = jniGooglePlayIabServiceClass.CallStatic<AndroidJavaObject>("getInstance");
					jniGooglePlayIabService.Call("setPublicKey", StoreSettings.AndroidPublicKey);

					jniGooglePlayIabServiceClass.SetStatic("AllowAndroidTestPurchases", StoreSettings.AndroidTestPurchases);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Starts a purchase process in the market.
		/// </summary>
		/// <param name="productId">id of the item to buy.</param>
		protected override void _buyMarketItem(string productId, string payload) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniPurchasableItem = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
								new AndroidJavaClass("com.soomla.store.data.StoreInfo"), "getPurchasableItem", productId)) {
				AndroidJNIHandler.CallVoid(jniSoomlaStore, "buyWithMarket", 
				                           jniPurchasableItem.Call<AndroidJavaObject>("getPurchaseType").Call<AndroidJavaObject>("getMarketItem"), 
				                           payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// This method will run _restoreTransactions followed by _refreshMarketItemsDetails.
		/// </summary>
		protected override void _refreshInventory() {
			AndroidJNI.PushLocalFrame(100);
			jniSoomlaStore.Call("refreshInventory");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Creates a list of all metadata stored in the Market (the items that have been purchased).
		/// The metadata includes the item's name, description, price, product id, etc...
		/// </summary>
		protected override void _refreshMarketItemsDetails() {
			AndroidJNI.PushLocalFrame(100);
			jniSoomlaStore.Call("refreshMarketItemsDetails");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Initiates the restore transactions process.
		/// </summary>
		protected override void _restoreTransactions() {
			AndroidJNI.PushLocalFrame(100);
			jniSoomlaStore.Call("restoreTransactions");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Starts in-app billing service in background.
		/// </summary>
		protected override void _startIabServiceInBg() {
			AndroidJNI.PushLocalFrame(100);
			jniSoomlaStore.Call("startIabServiceInBg");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		/// <summary>
		/// Stops in-app billing service in background.
		/// </summary>
		protected override void _stopIabServiceInBg() {
			AndroidJNI.PushLocalFrame(100);
			jniSoomlaStore.Call("stopIabServiceInBg");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
#endif
	}
}

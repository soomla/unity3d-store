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
		/// Load the billing service.
		/// </summary>
		protected override void _loadBillingService() {
			if (StoreSettings.GPlayBP) {
				if (string.IsNullOrEmpty(StoreSettings.AndroidPublicKey) ||
			 		    StoreSettings.AndroidPublicKey == StoreSettings.AND_PUB_KEY_DEFAULT) {

					SoomlaUtils.LogError(TAG, "You chose Google Play billing service, but publicKey is not set!! Stopping here!!");
					throw new ExitGUIException();
				}

				if (StoreSettings.PlaySsvValidation) {
					if (string.IsNullOrEmpty(StoreSettings.PlayClientId) ||
					    StoreSettings.PlayClientId == StoreSettings.PLAY_CLIENT_ID_DEFAULT) {
						
						SoomlaUtils.LogError(TAG, "You chose Google Play Receipt Validation, but clientId is not set!! Stopping here!!");
						throw new ExitGUIException();
					}
					
					if (string.IsNullOrEmpty(StoreSettings.PlayClientSecret) ||
					    StoreSettings.PlayClientSecret == StoreSettings.PLAY_CLIENT_SECRET_DEFAULT) {
						
						SoomlaUtils.LogError(TAG, "You chose Google Play Receipt Validation, but clientSecret is not set!! Stopping here!!");
						throw new ExitGUIException();
					}
					
					if (string.IsNullOrEmpty(StoreSettings.PlayRefreshToken) ||
					    StoreSettings.PlayRefreshToken == StoreSettings.PLAY_REFRESH_TOKEN_DEFAULT) {
                        
                        SoomlaUtils.LogError(TAG, "You chose Google Play Receipt Validation, but refreshToken is not set!! Stopping here!!");
                        throw new ExitGUIException();
                    }
                }
            }
            
            AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaStoreClass = new AndroidJavaClass("com.soomla.store.SoomlaStore")) {
				jniSoomlaStore = jniSoomlaStoreClass.CallStatic<AndroidJavaObject>("getInstance");
				bool success = jniSoomlaStore.Call<bool>("loadBillingService");
				if (!success) {
					SoomlaUtils.LogError(TAG, "Couldn't load billing service! Billing functions won't work.");
				}
			}

			if (StoreSettings.GPlayBP) {
				using(AndroidJavaClass jniGooglePlayIabServiceClass = new AndroidJavaClass("com.soomla.store.billing.google.GooglePlayIabService")) {
					AndroidJavaObject jniGooglePlayIabService = jniGooglePlayIabServiceClass.CallStatic<AndroidJavaObject>("getInstance");
					jniGooglePlayIabService.Call("setPublicKey", StoreSettings.AndroidPublicKey);


					if (StoreSettings.PlaySsvValidation) {
						using(AndroidJavaObject obj_HashMap = new AndroidJavaObject("java.util.HashMap"))
						{
							IntPtr method_Put = AndroidJNIHelper.GetMethodID(obj_HashMap.GetRawClass(), "put",
							                                                 "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
							
							object[] args = new object[2];

							// client ID
							using(AndroidJavaObject k = new AndroidJavaObject("java.lang.String", "clientId"))
							{
								using(AndroidJavaObject v = new AndroidJavaObject("java.lang.String", StoreSettings.PlayClientId))
								{
									args[0] = k;
									args[1] = v;
									AndroidJNI.CallObjectMethod(obj_HashMap.GetRawObject(),
									                            method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
	                            }
	                        }
							
							// client secret
							using(AndroidJavaObject k = new AndroidJavaObject("java.lang.String", "clientSecret"))
							{
								using(AndroidJavaObject v = new AndroidJavaObject("java.lang.String", StoreSettings.PlayClientSecret))
								{
									args[0] = k;
									args[1] = v;
									AndroidJNI.CallObjectMethod(obj_HashMap.GetRawObject(),
									                            method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
	                            }
	                        }
	                        
							// refresh token
							using(AndroidJavaObject k = new AndroidJavaObject("java.lang.String", "refreshToken"))
							{
								using(AndroidJavaObject v = new AndroidJavaObject("java.lang.String", StoreSettings.PlayRefreshToken))
								{
									args[0] = k;
									args[1] = v;
									AndroidJNI.CallObjectMethod(obj_HashMap.GetRawObject(),
									                            method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
								}
							}
							
							// verifyOnServerFailure
							using(AndroidJavaObject k = new AndroidJavaObject("java.lang.String", "verifyOnServerFailure"))
							{
								using(AndroidJavaObject v = new AndroidJavaObject("java.lang.Boolean", StoreSettings.PlayVerifyOnServerFailure))
								{
									args[0] = k;
									args[1] = v;
									AndroidJNI.CallObjectMethod(obj_HashMap.GetRawObject(),
									                            method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
								}
							}
							
							jniGooglePlayIabService.Call("configVerifyPurchases", obj_HashMap);
	                    }
					}
                    
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

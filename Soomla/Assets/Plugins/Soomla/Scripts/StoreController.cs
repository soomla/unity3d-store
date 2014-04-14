using UnityEngine;
using System;
using System.Runtime.InteropServices;


namespace Soomla
{
	/// <summary>
	/// You can use this class to purchase products from the native phone market, buy virtual goods, and do many other store related operations.
	/// </summary>
	public class StoreController
	{
		private const string TAG = "SOOMLA StoreController";
#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern void storeController_Init(string customSecret);
		[DllImport ("__Internal")]
		private static extern int storeController_BuyMarketItem(string productId);
		[DllImport ("__Internal")]
		private static extern void storeController_RestoreTransactions();
		[DllImport ("__Internal")]
		private static extern void storeController_RefreshInventory();
		[DllImport ("__Internal")]
		private static extern void storeController_TransactionsAlreadyRestored(out bool outResult);
		[DllImport ("__Internal")]
		private static extern void storeController_SetSoomSec(string soomSec);
		[DllImport ("__Internal")]
		private static extern void storeController_SetSSV(bool ssv, string verifyUrl);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaObject jniStoreController = null;
#endif

		public static void Initialize(IStoreAssets storeAssets) {
			if (string.IsNullOrEmpty(SoomSettings.CustomSecret) || string.IsNullOrEmpty(SoomSettings.SoomSecret)) {
				StoreUtils.LogError(TAG, "SOOMLA/UNITY MISSING customSecret or soomSec !!! Stopping here !!");
				throw new ExitGUIException();
			}

			if (SoomSettings.CustomSecret==SoomSettings.ONLY_ONCE_DEFAULT || SoomSettings.SoomSecret==SoomSettings.ONLY_ONCE_DEFAULT) {
				StoreUtils.LogError(TAG, "SOOMLA/UNITY You have to change customSecret and soomSec !!! Stopping here !!");
				throw new ExitGUIException();
			}

#if UNITY_ANDROID && !UNITY_EDITOR
			if (string.IsNullOrEmpty(SoomSettings.AndroidPublicKey)) {
				StoreUtils.LogError(TAG, "SOOMLA/UNITY MISSING publickKey !!! Stopping here !!");
				throw new ExitGUIException();
			}

			if (SoomSettings.AndroidPublicKey==SoomSettings.AND_PUB_KEY_DEFAULT) {
				StoreUtils.LogError(TAG, "SOOMLA/UNITY You have to change android publicKey !!! Stopping here !!");
				throw new ExitGUIException();
			}

			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStoreAssets = new AndroidJavaClass("com.soomla.unity.StoreAssets")) {
				jniStoreAssets.CallStatic("setSoomSec", SoomSettings.SoomSecret);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			storeController_SetSSV(SoomSettings.IosSSV, "https://verify.soom.la/verify_ios?platform=unity4");
			storeController_SetSoomSec(SoomSettings.SoomSecret);
#endif
			
			StoreInfo.Initialize(storeAssets);
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			//init EventHandler
			using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.unity.EventHandler")) {
				jniEventHandler.CallStatic("initialize");
			}
			using(AndroidJavaObject jniStoreAssetsInstance = new AndroidJavaObject("com.soomla.unity.StoreAssets")) {
				using(AndroidJavaClass jniStoreControllerClass = new AndroidJavaClass("com.soomla.store.StoreController")) {
					jniStoreController = jniStoreControllerClass.CallStatic<AndroidJavaObject>("getInstance");
					jniStoreController.Call<bool>("initialize", jniStoreAssetsInstance, SoomSettings.AndroidPublicKey, SoomSettings.CustomSecret);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			
#elif UNITY_IOS && !UNITY_EDITOR
			storeController_Init(SoomSettings.CustomSecret);
#endif
		}
		
		
		public static void BuyMarketItem(string productId) {
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniPurchasableItem = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getPurchasableItem", productId)) {
				AndroidJNIHandler.CallVoid(jniStoreController, "buyWithMarket", 
					jniPurchasableItem.Call<AndroidJavaObject>("getPurchaseType").Call<AndroidJavaObject>("getMarketItem"), 
					"");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			storeController_BuyMarketItem(productId);
#endif
		}

		public static void RefreshInventory() {
			if(!Application.isEditor){
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				jniStoreController.Call("refreshInventory", true);
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				storeController_RefreshInventory();
#endif
			}
		}

		public static void RestoreTransactions() {
			if(!Application.isEditor){
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				jniStoreController.Call("refreshInventory", false);
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				storeController_RestoreTransactions();
#endif
			}
		}

#if UNITY_IOS && !UNITY_EDITOR	
		public static bool TransactionsAlreadyRestored() {
			bool restored = false;
			storeController_TransactionsAlreadyRestored(out restored);
			return restored;
		}
#endif
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public static void StartIabServiceInBg() {
			AndroidJNI.PushLocalFrame(100);
			jniStoreController.Call("startIabServiceInBg");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		public static void StopIabServiceInBg() {
			AndroidJNI.PushLocalFrame(100);
			jniStoreController.Call("stopIabServiceInBg");
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
#endif
		
	}
}


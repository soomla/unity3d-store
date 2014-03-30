using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

namespace Soomla
{
	/// <summary>
	/// This class allows some convinience operations on Virtual Goods and Virtual Currencies.
	/// </summary>
	public class StoreInventory
	{
		private const string TAG = "SOOMLA StoreInventory";

#if UNITY_EDITOR
		private static Dictionary<string, int> localItems = new Dictionary<string, int> ();
		private static HashSet<string> equippedGoods = new HashSet<string>();
		private static HashSet<string> nonConsumables = new HashSet<string>();

		private static StoreEvents Evt { get { return GameObject.FindObjectOfType<StoreEvents>(); }}
#endif

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern int storeInventory_BuyItem(string itemId);
		[DllImport ("__Internal")]
		private static extern int storeInventory_GetItemBalance(string itemId, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int storeInventory_GiveItem(string itemId, int amount);
		[DllImport ("__Internal")]
		private static extern int storeInventory_TakeItem(string itemId, int amount);
		[DllImport ("__Internal")]
		private static extern int storeInventory_EquipVirtualGood(string itemId);
		[DllImport ("__Internal")]
		private static extern int storeInventory_UnEquipVirtualGood(string itemId)	;
		[DllImport ("__Internal")]
		private static extern int storeInventory_IsVirtualGoodEquipped(string itemId, out bool outResult);
		[DllImport ("__Internal")]
		private static extern int storeInventory_GetGoodUpgradeLevel(string itemId, out int outResult);
		[DllImport ("__Internal")]
		private static extern int storeInventory_GetGoodCurrentUpgrade(string itemId, out IntPtr outResult);
		[DllImport ("__Internal")]
		private static extern int storeInventory_UpgradeGood(string itemId);
		[DllImport ("__Internal")]
		private static extern int storeInventory_RemoveGoodUpgrades(string itemId);
		[DllImport ("__Internal")]
		private static extern int storeInventory_NonConsumableItemExists(string itemId, out bool outResult);
		[DllImport ("__Internal")]
		private static extern int storeInventory_AddNonConsumableItem(string itemId);
		[DllImport ("__Internal")]
		private static extern int storeInventory_RemoveNonConsumableItem(string itemId);
#endif
		
		public static void BuyItem(string itemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling BuyItem with: " + itemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "buy", itemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_BuyItem(itemId);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
#if UNITY_EDITOR
			VirtualItem item = RequireItem(itemId);

			((PurchasableVirtualItem)item).Buy();
#endif
		}
		
		
		
		
		/** Virtual Items **/
		
		
		public static int GetItemBalance(string itemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GetItemBalance with: " + itemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				int balance = 0;
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					balance = AndroidJNIHandler.CallStatic<int>(jniStoreInventory, "getVirtualItemBalance", itemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
				return balance;
#elif UNITY_IOS && !UNITY_EDITOR
				int balance = 0;
				int err = storeInventory_GetItemBalance(itemId, out balance);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
				
				return balance;
#endif
			}
#if UNITY_EDITOR
			RequireItem(itemId);

			int amount;
			if (localItems.TryGetValue(itemId, out amount))
				return amount;
#endif
			return 0;
		}
		
		public static void GiveItem(string itemId, int amount) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GiveItem with itedId: " + itemId + " and amount: " + amount);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "giveVirtualItem", itemId, amount);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_GiveItem(itemId, amount);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
#if UNITY_EDITOR
			VirtualItem item = RequireItem(itemId);

			int itemAmount;
			if (localItems.TryGetValue (itemId, out itemAmount)) {
					itemAmount += amount;
					localItems [itemId] = itemAmount;
			} else {
					itemAmount = amount;
					localItems [itemId] = amount;
			}

			string change = itemId + "#SOOM#" + itemAmount + "#SOOM#" + amount;
			if (item is VirtualCurrency)
				Evt.onCurrencyBalanceChanged(change);
			else
				Evt.onGoodBalanceChanged(change);
#endif
		}
		
		public static void TakeItem(string itemId, int amount) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling TakeItem with itedId: " + itemId + " and amount: " + amount);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "takeVirtualItem", itemId, amount);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_TakeItem(itemId, amount);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}

#if UNITY_EDITOR
			VirtualItem item = RequireItem(itemId);

			int itemAmount;
			if (localItems.TryGetValue(itemId, out itemAmount) && itemAmount >= amount) {
				itemAmount -= amount;
				
				localItems[itemId] = itemAmount;
			} else {
				if (item is VirtualCurrency)
					throw new InsufficientFundsException(itemId);
				else
					throw new NotEnoughGoodsException(itemId);
			}

			string change = itemId + "#SOOM#" + itemAmount + "#SOOM#" + (-amount);
			if (item is VirtualCurrency)
				Evt.onCurrencyBalanceChanged(change);
			else
				Evt.onGoodBalanceChanged(change);
#endif
		}
				
		
		
		
		
		/** Virtual Goods **/
		
		
		public static void EquipVirtualGood(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling EquipVirtualGood with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "equipVirtualGood", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_EquipVirtualGood(goodItemId);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
#if UNITY_EDITOR
			RequireItem(goodItemId, "GoodItemId");

			equippedGoods.Add(goodItemId);
			
			Evt.onGoodEquipped(goodItemId);
#endif
		}
		
		public static void UnEquipVirtualGood(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling UnEquipVirtualGood with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "unEquipVirtualGood", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_UnEquipVirtualGood(goodItemId);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
#if UNITY_EDITOR
			RequireItem(goodItemId, "GoodItemId");

			equippedGoods.Remove(goodItemId);
			
			Evt.onGoodUnequipped(goodItemId);
#endif
		}
		
		public static bool IsVirtualGoodEquipped(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling IsVirtualGoodEquipped with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				bool result = false;
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					result = AndroidJNIHandler.CallStatic<bool>(jniStoreInventory, "isVirtualGoodEquipped", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
				return result;
#elif UNITY_IOS && !UNITY_EDITOR
				bool result = false;
				int err = storeInventory_IsVirtualGoodEquipped(goodItemId, out result);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
				
				return result;
#endif
			}
#if UNITY_EDITOR
			RequireItem(goodItemId, "GoodItemId");

			return equippedGoods.Contains(goodItemId);
#else
			return false;
#endif
		}
		
		public static int GetGoodUpgradeLevel(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GetGoodUpgradeLevel with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				int level = 0;
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					level = AndroidJNIHandler.CallStatic<int>(jniStoreInventory, "getGoodUpgradeLevel", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
				return level;
#elif UNITY_IOS && !UNITY_EDITOR
				int level = 0;
				int err = storeInventory_GetGoodUpgradeLevel(goodItemId, out level);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
				
				return level;
#endif
			}
#if UNITY_EDITOR
			//				VirtualGood good = StoreInfo.GetVirtualGoods().FirstOrDefault(g => g.ItemId == goodItemId);
			//
			//				StoreInfo.GetUpgradesForVirtualGood(goodItemId);
#endif
			return 0;
		}
		
		public static string GetGoodCurrentUpgrade(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GetGoodCurrentUpgrade with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				string currentItemId = "";
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					currentItemId = AndroidJNIHandler.CallStatic<string>(jniStoreInventory, "getGoodCurrentUpgrade", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
				return currentItemId;
#elif UNITY_IOS && !UNITY_EDITOR				
				IntPtr p = IntPtr.Zero;
				int err = storeInventory_GetGoodCurrentUpgrade(goodItemId, out p);
					
				IOS_ErrorCodes.CheckAndThrowException(err);
				
				string result = Marshal.PtrToStringAnsi(p);
				Marshal.FreeHGlobal(p);
				
				return result;
#endif
			}
			return null;
		}
		
		public static void UpgradeGood(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling UpgradeGood with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "upgradeVirtualGood", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR				
				int err = storeInventory_UpgradeGood(goodItemId);
					
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
		}
		
		public static void RemoveGoodUpgrades(string goodItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling RemoveGoodUpgrades with: " + goodItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "removeUpgrades", goodItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR				
				int err = storeInventory_RemoveGoodUpgrades(goodItemId);
					
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
		}
		
		
		
		
		/** NonConsumables **/
		
		
		public static bool NonConsumableItemExists(string nonConsItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling NonConsumableItemExists with: " + nonConsItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				bool result = false;
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					result = AndroidJNIHandler.CallStatic<bool>(jniStoreInventory, "nonConsumableItemExists", nonConsItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
				return result;
#elif UNITY_IOS && !UNITY_EDITOR
				bool result = false;
				int err = storeInventory_NonConsumableItemExists(nonConsItemId, out result);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
				
				return result;
#endif
			}
#if UNITY_EDITOR
			RequireItem(nonConsItemId, "NonConsItemId");

			return nonConsumables.Contains(nonConsItemId);
#else
			return false;
#endif
		}
		
		public static void AddNonConsumableItem(string nonConsItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling AddNonConsumableItem with: " + nonConsItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "addNonConsumableItem", nonConsItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_AddNonConsumableItem(nonConsItemId);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
#if UNITY_EDITOR
			RequireItem(nonConsItemId, "NonConsItemId");

			nonConsumables.Add(nonConsItemId);
#endif
		}
		
		public static void RemoveNonConsumableItem(string nonConsItemId) {
			if(!Application.isEditor){
				StoreUtils.LogDebug(TAG, "SOOMLA/UNITY Calling RemoveNonConsumableItem with: " + nonConsItemId);
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniStoreInventory = new AndroidJavaClass("com.soomla.store.StoreInventory")) {
					AndroidJNIHandler.CallStaticVoid(jniStoreInventory, "removeNonConsumableItem", nonConsItemId);
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
				int err = storeInventory_RemoveNonConsumableItem(nonConsItemId);
				
				IOS_ErrorCodes.CheckAndThrowException(err);
#endif
			}
#if UNITY_EDITOR
			RequireItem(nonConsItemId, "NonConsItemId");

			nonConsumables.Remove(nonConsItemId);
#endif
		}

#if UNITY_EDITOR
		private static VirtualItem RequireItem(string itemId, string lookupBy = "ItemId") {
			VirtualItem virtualItem = StoreInfo.GetItemByItemId (itemId);
			if (virtualItem == null)
				throw new VirtualItemNotFoundException(lookupBy, itemId);

			return virtualItem;
		}
#endif
	}
}




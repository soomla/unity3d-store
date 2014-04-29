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

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
		private class Upgrade {
			public int level;
			public string itemId;
		}

		private static Dictionary<string, int> localItemBalances = new Dictionary<string, int> ();
		private static Dictionary<string, Upgrade> localUpgrades = new Dictionary<string, Upgrade>();
		private static HashSet<string> localEquippedGoods = new HashSet<string>();

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
#else
			RequireItem<PurchasableVirtualItem>(itemId).Buy();
#endif
  	}

		
		
		
		/** Virtual Items **/
		
		
		public static int GetItemBalance(string itemId) {
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
#else
			int amount;
			if (localItemBalances.TryGetValue(itemId, out amount))
				return amount;
#endif
			return 0;
		}

		public static void GiveItem(string itemId, int amount) {
			GiveItem(itemId, amount, true);
		}

		private static void GiveItem(string itemId, int amount, bool notify) {
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
#else
			VirtualItem item = RequireItem(itemId);

			int itemAmount;
			if (localItemBalances.TryGetValue (itemId, out itemAmount)) {
					itemAmount += amount;
					localItemBalances[itemId] = itemAmount;
			} else {
					itemAmount = amount;
					localItemBalances[itemId] = amount;
			}

			if (notify)
				NotifyChange(item, itemAmount, amount);
#endif
		}
		
		public static void TakeItem(string itemId, int amount) {
			TakeItem(itemId, amount, true);
		}

		private static void TakeItem(string itemId, int amount, bool notify) {
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
#else
			VirtualItem item = RequireItem(itemId);

			int itemAmount;
			if (localItemBalances.TryGetValue(itemId, out itemAmount)) {
				itemAmount -= amount;

				if (itemAmount > 0)
					localItemBalances[itemId] = itemAmount;
				else
					localItemBalances.Remove(itemId);
			}

			if (notify)
				NotifyChange(item, itemAmount, -amount);
#endif
		}
				
#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
		private static void NotifyChange(VirtualItem item, int amount, int change) {
			string changeHash = item.ItemId + "#SOOM#" + amount + "#SOOM#" + change;
			if (item is VirtualCurrency)
				Evt.onCurrencyBalanceChanged(changeHash);
			else if (item is VirtualGood)
				Evt.onGoodBalanceChanged(changeHash);
		}
#endif
		
		
		
		/** Virtual Goods **/
		
		
		public static void EquipVirtualGood(string goodItemId) {
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
#else
			if (StoreInfo.GetItemByItemId(goodItemId) == null)
				throw new VirtualItemNotFoundException("GoodItemId", goodItemId);

			if (GetItemBalance(goodItemId) > 0) {
				EquippableVG item = RequireItem<EquippableVG>(goodItemId);
				if (item.Equipping == EquippableVG.EquippingModel.CATEGORY) {
					VirtualCategory category;
					try {
						category = StoreInfo.GetCategoryForVirtualGood(goodItemId);
					} catch (VirtualItemNotFoundException e) {
						StoreUtils.LogError(TAG, "Tried to unequip all other category VirtualGoods but there was no " +
						                    "associated category. virtual good itemId: " + goodItemId);
						return;
					}
					
					foreach (string itemId in category.GoodItemIds) {
						if (itemId != goodItemId) {
							UnEquipVirtualGood(itemId);
						}
					}
				} else if (item.Equipping == EquippableVG.EquippingModel.GLOBAL) {
					foreach (string itemId in localEquippedGoods) {
						if (itemId != goodItemId) {
							UnEquipVirtualGood(itemId);
						}
					}
				}

				localEquippedGoods.Add(goodItemId);
				Evt.onGoodEquipped(goodItemId);
			}
#endif
		}
		
		public static void UnEquipVirtualGood(string goodItemId) {
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
#else
			if (localEquippedGoods.Remove(goodItemId))
				Evt.onGoodUnequipped(goodItemId);
#endif
		}
		
		public static bool IsVirtualGoodEquipped(string goodItemId) {
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
#else
			return localEquippedGoods.Contains(goodItemId);
#endif
		}
		
		public static int GetGoodUpgradeLevel(string goodItemId) {
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
#else
			Upgrade upgrade;
			if (localUpgrades.TryGetValue(goodItemId, out upgrade)) 
				return upgrade.level;
#endif
			return 0;
		}
		
		public static string GetGoodCurrentUpgrade(string goodItemId) {
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
#else
			Upgrade upgrade;
			if (localUpgrades.TryGetValue(goodItemId, out upgrade))
				return upgrade.itemId;
#endif
			return null;
		}
		
		public static void UpgradeGood(string goodItemId) {
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
#else
			Upgrade upgrade;
			if (localUpgrades.TryGetValue(goodItemId, out upgrade)) {
				UpgradeVG up = RequireItem<UpgradeVG>(upgrade.itemId, "UpgradeItemId");

				if (!string.IsNullOrEmpty(up.NextItemId)) {
					UpgradeVG next = RequireItem<UpgradeVG>(up.NextItemId, "UpgradeItemId");
					next.Buy();
					upgrade.itemId = next.ItemId;
					upgrade.level++;

					Evt.onGoodUpgrade(goodItemId + "#SOOM#" + next.ItemId);
				}
			} else {
				UpgradeVG first = StoreInfo.GetFirstUpgradeForVirtualGood(goodItemId);
				if (first != null) {
					first.Buy();
					localUpgrades.Add(goodItemId, new Upgrade { itemId = first.ItemId, level = 1 });

					Evt.onGoodUpgrade(goodItemId + "#SOOM#" + first.ItemId);
				}
			}
#endif
		}
		
		public static void RemoveGoodUpgrades(string goodItemId) {
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
#else
			// try get current good upgrade
			string upgradeId = GetGoodCurrentUpgrade(goodItemId);
			if (string.IsNullOrEmpty(upgradeId))
				return;

			// downgrade good while it has upgrades
			UpgradeVG upgrade;
			while (true) {
				TakeItem(upgradeId, 1);

				upgrade = RequireItem<UpgradeVG>(upgradeId);
				if (!string.IsNullOrEmpty(upgrade.PrevItemId))
					upgradeId = upgrade.ItemId;
				else
					break;
			}
			// remove all info about upgrades
			localUpgrades.Remove(goodItemId);

			Evt.onGoodUpgrade(goodItemId + "#SOOM#");
#endif
		}
		
		
		
		
		/** NonConsumables **/
		
		
		public static bool NonConsumableItemExists(string nonConsItemId) {
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
#else
			return GetItemBalance(nonConsItemId) > 0;
#endif
		}
		
		public static void AddNonConsumableItem(string nonConsItemId) {
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
#else
			if (!NonConsumableItemExists(nonConsItemId))
				GiveItem(nonConsItemId, 1, false);
#endif
		}
		
		public static void RemoveNonConsumableItem(string nonConsItemId) {
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
#else
			TakeItem(nonConsItemId, 1, false);
#endif
		}

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
		private static T RequireItem<T>(string itemId, string lookupBy = "ItemId") where T : VirtualItem {
			T virtualItem = StoreInfo.GetItemByItemId(itemId) as T;
			if (virtualItem == null)
				throw new VirtualItemNotFoundException(lookupBy, itemId);

			return virtualItem;
		}

		private static VirtualItem RequireItem(string itemId, string lookupBy = "ItemId") {
			return RequireItem<VirtualItem>(itemId, lookupBy);
		}
#endif
	}
}




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
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace Soomla.Store {

	/// <summary>
	/// <c>StoreInfo</c> for iOS.
	/// This class holds the store's meta data including:
	/// virtual currencies definitions, 
	/// virtual currency packs definitions, 
	/// virtual goods definitions, 
	/// virtual categories definitions, and 
	/// virtual non-consumable items definitions
	/// </summary>
	public class StoreInfoIOS : StoreInfo {

#if UNITY_IOS && !UNITY_EDITOR

		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetItemByItemId(string itemId, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetPurchasableItemWithProductId(string productId, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetCategoryForVirtualGood(string goodItemId, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetFirstUpgradeForVirtualGood(string goodItemId, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetLastUpgradeForVirtualGood(string goodItemId, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetUpgradesForVirtualGood(string goodItemId, out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetVirtualCurrencies(out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetVirtualGoods(out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetVirtualCurrencyPacks(out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetNonConsumableItems(out IntPtr json);
		[DllImport ("__Internal")]
		private static extern int storeInfo_GetVirtualCategories(out IntPtr json);
		[DllImport ("__Internal")]
		private static extern void storeAssets_Init(int version, string storeAssetsJSON);

		/// <summary>
		/// Initializes <c>StoreInfo</c>. 
		/// On first initialization, when the database doesn't have any previous version of the store
		/// metadata, <c>StoreInfo</c> gets loaded from the given <c>IStoreAssets</c>.
		/// After the first initialization, <c>StoreInfo</c> will be initialized from the database.
		/// 
		/// IMPORTANT: If you want to override the current <c>StoreInfo</c>, you'll have to bump
		/// the version of your implementation of <c>IStoreAssets</c> in order to remove the
		/// metadata when the application loads. Bumping the version is done by returning a higher 
		/// number in <c>IStoreAssets</c>'s <c>getVersion</c>.
		/// </summary>
		/// <param name="storeAssets">your game's economy</param>
		override protected void _initialize(IStoreAssets storeAssets) {
			SoomlaUtils.LogDebug(TAG, "pushing data to StoreAssets on ios side");
			string storeAssetsJSON = IStoreAssetsToJSON(storeAssets);
			int version = storeAssets.GetVersion();
			storeAssets_Init(version, storeAssetsJSON);
			SoomlaUtils.LogDebug(TAG, "done! (pushing data to StoreAssets on ios side)");
		}

		/// <summary>
		/// Gets the item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">Item id.</param>
		/// <returns>Item with the given id.</returns>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if item is not found.</exception>
		override protected VirtualItem _getItemByItemId(string itemId) {
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetItemByItemId(itemId, out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string json = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			SoomlaUtils.LogDebug(TAG, "Got json: " + json);

			JSONObject obj = new JSONObject(json);
			return VirtualItem.factoryItemFromJSONObject(obj);
		}

		/// <summary>
		/// Gets the purchasable item with the given <c>productId</c>.
		/// </summary>
		/// <param name="productId">Product id.</param>
		/// <returns>Purchasable virtual item with the given id.</returns>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if item is not found.</exception>
		override protected PurchasableVirtualItem _getPurchasableItemWithProductId(string productId) {
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetPurchasableItemWithProductId(productId, out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string nonConsJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);

			JSONObject obj = new JSONObject(nonConsJson);
			return (PurchasableVirtualItem)VirtualItem.factoryItemFromJSONObject(obj);
		}

		/// <summary>
		/// Gets the category that the virtual good with the given <c>goodItemId</c> belongs to.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>Category that the item with given id belongs to.</returns>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if category is not found.</exception>
		override protected VirtualCategory _getCategoryForVirtualGood(string goodItemId) {
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetCategoryForVirtualGood(goodItemId, out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string json = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			JSONObject obj = new JSONObject(json);
			return new VirtualCategory(obj);
		}

		/// <summary>
		/// Gets the first upgrade for virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>The first upgrade for virtual good with the given id.</returns>
		override protected UpgradeVG _getFirstUpgradeForVirtualGood(string goodItemId) {
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetFirstUpgradeForVirtualGood(goodItemId, out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string json = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			JSONObject obj = new JSONObject(json);
			return new UpgradeVG(obj);
		}

		/// <summary>
		/// Gets the last upgrade for the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">item id</param>
		/// <returns>last upgrade for virtual good with the given id</returns>
		override protected UpgradeVG _getLastUpgradeForVirtualGood(string goodItemId) {
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetLastUpgradeForVirtualGood(goodItemId, out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string json = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			JSONObject obj = new JSONObject(json);
			return new UpgradeVG(obj);
		}

		/// <summary>
		/// Gets all the upgrades for the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>All upgrades for virtual good with the given id.</returns>
		override protected List<UpgradeVG> _getUpgradesForVirtualGood(string goodItemId) {
			List<UpgradeVG> vgus = new List<UpgradeVG>();
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetUpgradesForVirtualGood(goodItemId, out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string upgradesJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			SoomlaUtils.LogDebug(TAG, "Got json: " + upgradesJson);
			
			JSONObject upgradesArr = new JSONObject(upgradesJson);

			// .list can be == null when json string is empty (it comes empty from iOS when no entries)
			if (upgradesArr.list != null) {
				foreach(JSONObject obj in upgradesArr.list) {
					vgus.Add(new UpgradeVG(obj));
				}
			}
			return vgus;
		}

		/// <summary>
		/// Fetches the virtual currencies of your game.
		/// </summary>
		/// <returns>The virtual currencies.</returns>
		override protected List<VirtualCurrency> _getVirtualCurrencies() {
			List<VirtualCurrency> vcs = new List<VirtualCurrency>();
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetVirtualCurrencies(out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string currenciesJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			SoomlaUtils.LogDebug(TAG, "Got json: " + currenciesJson);
			
			JSONObject currenciesArr = new JSONObject(currenciesJson);
			// .list can be == null when json string is empty (it comes empty from iOS when no entries)
			if (currenciesArr.list != null) {
				foreach(JSONObject obj in currenciesArr.list) {
					vcs.Add(new VirtualCurrency(obj));
				}
			}
			return vcs;
		}

		/// <summary>
		/// Fetches the virtual goods of your game.
		/// </summary>
		/// <returns>All virtual goods.</returns>
		override protected List<VirtualGood> _getVirtualGoods() {
			List<VirtualGood> virtualGoods = new List<VirtualGood>();
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetVirtualGoods(out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string goodsJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			SoomlaUtils.LogDebug(TAG, "Got json: " + goodsJson);
			
			JSONObject goodsArr = new JSONObject(goodsJson);
			// .list can be == null when json string is empty (it comes empty from iOS when no entries)
			if (goodsArr.list != null) {
				foreach(JSONObject obj in goodsArr.list) {
					virtualGoods.Add((VirtualGood)VirtualItem.factoryItemFromJSONObject(obj));
				}
			}
			return virtualGoods;
		}

		/// <summary>
		/// Fetches the virtual currency packs of your game.
		/// </summary>
		/// <returns>All virtual currency packs.</returns>
		override protected List<VirtualCurrencyPack> _getVirtualCurrencyPacks() {
			List<VirtualCurrencyPack> vcps = new List<VirtualCurrencyPack>();
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetVirtualCurrencyPacks(out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string packsJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			SoomlaUtils.LogDebug(TAG, "Got json: " + packsJson);
			
			JSONObject packsArr = new JSONObject(packsJson);
			// .list can be == null when json string is empty (it comes empty from iOS when no entries)
			if (packsArr.list != null) {
				foreach(JSONObject obj in packsArr.list) {
					vcps.Add(new VirtualCurrencyPack(obj));
				}
			}
			return vcps;
		}

		/// <summary>
		/// Fetches the non consumable items of your game.
		/// </summary>
		/// <returns>All non consumable items.</returns>
		override protected List<NonConsumableItem> _getNonConsumableItems() {
			List<NonConsumableItem> nonConsumableItems = new List<NonConsumableItem>();
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetNonConsumableItems(out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string nonConsumableJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			SoomlaUtils.LogDebug(TAG, "Got json: " + nonConsumableJson);
			
			JSONObject nonConsArr = new JSONObject(nonConsumableJson);
			// .list can be == null when json string is empty (it comes empty from iOS when no entries)
			if (nonConsArr.list != null) {
				foreach(JSONObject obj in nonConsArr.list) {
					nonConsumableItems.Add(new NonConsumableItem(obj));
				}
			}
			return nonConsumableItems;
		}

		/// <summary>
		/// Fetches the virtual categories of your game.
		/// </summary>
		/// <returns>All virtual categories.</returns>
		override protected List<VirtualCategory> _getVirtualCategories() {
			List<VirtualCategory> virtualCategories = new List<VirtualCategory>();
			IntPtr p = IntPtr.Zero;
			int err = storeInfo_GetVirtualCategories(out p);
			IOS_ErrorCodes.CheckAndThrowException(err);
			
			string categoriesJson = Marshal.PtrToStringAnsi(p);
			Marshal.FreeHGlobal(p);
			
			SoomlaUtils.LogDebug(TAG, "Got json: " + categoriesJson);
			
			JSONObject categoriesArr = new JSONObject(categoriesJson);
			// .list can be == null when json string is empty (it comes empty from iOS when no entries)
			if (categoriesArr.list != null) {
				foreach(JSONObject obj in categoriesArr.list) {
					virtualCategories.Add(new VirtualCategory(obj));
				}
			}
			return virtualCategories;
		}
#endif
	}
}

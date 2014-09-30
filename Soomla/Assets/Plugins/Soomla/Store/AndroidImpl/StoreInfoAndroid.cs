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
	/// <c>StoreInfo</c> for Android.
	/// This class holds the store's meta data including:
	/// virtual currencies definitions, 
	/// virtual currency packs definitions, 
	/// virtual goods definitions, 
	/// virtual categories definitions, and 
	/// virtual non-consumable items definitions
	/// </summary>
	public class StoreInfoAndroid : StoreInfo {

#if UNITY_ANDROID && !UNITY_EDITOR

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
		override protected void _initialize(IStoreAssets storeAssets) {
			SoomlaUtils.LogDebug(TAG, "pushing data to StoreAssets on java side");
			string storeAssetsJSON = IStoreAssetsToJSON(storeAssets);
			int version = storeAssets.GetVersion();
			using(AndroidJavaClass jniStoreAssets = new AndroidJavaClass("com.soomla.unity.StoreAssets")) {
				jniStoreAssets.CallStatic("prepare", version, storeAssetsJSON);
			}
			SoomlaUtils.LogDebug(TAG, "done! (pushing data to StoreAssets on java side)");
		}

		/// <summary>
		/// Gets the item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">Item id.</param>
		/// <returns>Item with the given id.</returns>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if item is not found.</exception>
		override protected VirtualItem _getItemByItemId(string itemId) {
			VirtualItem vi = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualItem = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getVirtualItem", itemId)) {
				vi = VirtualItem.factoryItemFromJNI(jniVirtualItem);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vi;
		}

		/// <summary>
		/// Gets the purchasable item with the given <c>productId</c>.
		/// </summary>
		/// <param name="productId">Product id.</param>
		/// <returns>Purchasable virtual item with the given id.</returns>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if item is not found.</exception>
		override protected PurchasableVirtualItem _getPurchasableItemWithProductId(string productId) {
			VirtualItem vi = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualItem = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getPurchasableItem", productId)) {
				vi = VirtualItem.factoryItemFromJNI(jniVirtualItem);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return (PurchasableVirtualItem)vi;
		}

		/// <summary>
		/// Gets the category that the virtual good with the given <c>goodItemId</c> belongs to.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>Category that the item with given id belongs to.</returns>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if category is not found.</exception>
		override protected VirtualCategory _getCategoryForVirtualGood(string goodItemId) {
			VirtualCategory vc = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualVategory = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getCategory", goodItemId)) {
				vc = new VirtualCategory(jniVirtualVategory);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vc;
		}

		/// <summary>
		/// Gets the first upgrade for virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>The first upgrade for virtual good with the given id.</returns>
		override protected UpgradeVG _getFirstUpgradeForVirtualGood(string goodItemId) {
			UpgradeVG vgu = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniUpgradeVG = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getGoodFirstUpgrade", goodItemId)) {
				vgu = new UpgradeVG(jniUpgradeVG);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vgu;
		}

		/// <summary>
		/// Gets the last upgrade for the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">item id</param>
		/// <returns>last upgrade for virtual good with the given id</returns>
		override protected UpgradeVG _getLastUpgradeForVirtualGood(string goodItemId) {
			UpgradeVG vgu = null;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniUpgradeVG = AndroidJNIHandler.CallStatic<AndroidJavaObject>(
				new AndroidJavaClass("com.soomla.store.data.StoreInfo"),"getGoodLastUpgrade", goodItemId)) {
				vgu = new UpgradeVG(jniUpgradeVG);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vgu;
		}

		/// <summary>
		/// Gets all the upgrades for the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>All upgrades for virtual good with the given id.</returns>
		override protected List<UpgradeVG> _getUpgradesForVirtualGood(string goodItemId) {
			List<UpgradeVG> vgus = new List<UpgradeVG>();
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniUpgradeVGs = new AndroidJavaClass("com.soomla.store.data.StoreInfo").CallStatic<AndroidJavaObject>("getGoodUpgrades", goodItemId)) {
				for(int i=0; i<jniUpgradeVGs.Call<int>("size"); i++) {
					using(AndroidJavaObject jnivgu = jniUpgradeVGs.Call<AndroidJavaObject>("get", i)) {
						vgus.Add(new UpgradeVG(jnivgu));
					}
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vgus;
		}

		/// <summary>
		/// Fetches the virtual currencies of your game.
		/// </summary>
		/// <returns>The virtual currencies.</returns>
		override protected List<VirtualCurrency> _getVirtualCurrencies() {
			List<VirtualCurrency> vcs = new List<VirtualCurrency>();
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualCurrencies = new AndroidJavaClass("com.soomla.store.data.StoreInfo").CallStatic<AndroidJavaObject>("getCurrencies")) {
				for(int i=0; i<jniVirtualCurrencies.Call<int>("size"); i++) {
					using(AndroidJavaObject jnivc = jniVirtualCurrencies.Call<AndroidJavaObject>("get", i)) {
						vcs.Add(new VirtualCurrency(jnivc));
					}
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vcs;
		}

		/// <summary>
		/// Fetches the virtual goods of your game.
		/// </summary>
		/// <returns>All virtual goods.</returns>
		override protected List<VirtualGood> _getVirtualGoods() {
			List<VirtualGood> virtualGoods = new List<VirtualGood>();
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualGoods = new AndroidJavaClass("com.soomla.store.data.StoreInfo").CallStatic<AndroidJavaObject>("getGoods")) {
				for(int i=0; i<jniVirtualGoods.Call<int>("size"); i++) {
					AndroidJNI.PushLocalFrame(100);
					using(AndroidJavaObject jniGood = jniVirtualGoods.Call<AndroidJavaObject>("get", i)) {
						virtualGoods.Add((VirtualGood)VirtualItem.factoryItemFromJNI(jniGood));
					}
					AndroidJNI.PopLocalFrame(IntPtr.Zero);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return virtualGoods;
		}

		/// <summary>
		/// Fetches the virtual currency packs of your game.
		/// </summary>
		/// <returns>All virtual currency packs.</returns>
		override protected List<VirtualCurrencyPack> _getVirtualCurrencyPacks() {
			List<VirtualCurrencyPack> vcps = new List<VirtualCurrencyPack>();
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualCurrencyPacks = new AndroidJavaClass("com.soomla.store.data.StoreInfo").CallStatic<AndroidJavaObject>("getCurrencyPacks")) {
				for(int i=0; i<jniVirtualCurrencyPacks.Call<int>("size"); i++) {
					using(AndroidJavaObject jnivcp = jniVirtualCurrencyPacks.Call<AndroidJavaObject>("get", i)) {
						vcps.Add(new VirtualCurrencyPack(jnivcp));
					}
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return vcps;
		}

		/// <summary>
		/// Fetches the non consumable items of your game.
		/// </summary>
		/// <returns>All non consumable items.</returns>
		override protected List<NonConsumableItem> _getNonConsumableItems() {
			List<NonConsumableItem> nonConsumableItems = new List<NonConsumableItem>();
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniNonConsumableItems = new AndroidJavaClass("com.soomla.store.data.StoreInfo").CallStatic<AndroidJavaObject>("getNonConsumableItems")) {
				for(int i=0; i<jniNonConsumableItems.Call<int>("size"); i++) {
					using(AndroidJavaObject jniNon = jniNonConsumableItems.Call<AndroidJavaObject>("get", i)) {
						nonConsumableItems.Add(new NonConsumableItem(jniNon));
					}
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return nonConsumableItems;
		}

		/// <summary>
		/// Fetches the virtual categories of your game.
		/// </summary>
		/// <returns>All virtual categories.</returns>
		override protected List<VirtualCategory> _getVirtualCategories() {
			List<VirtualCategory> virtualCategories = new List<VirtualCategory>();
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaObject jniVirtualCategories = new AndroidJavaClass("com.soomla.store.data.StoreInfo").CallStatic<AndroidJavaObject>("getCategories")) {
				for(int i=0; i<jniVirtualCategories.Call<int>("size"); i++) {
					using(AndroidJavaObject jniCat = jniVirtualCategories.Call<AndroidJavaObject>("get", i)) {
						virtualCategories.Add(new VirtualCategory(jniCat));
					}
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return virtualCategories;
		}
#endif
	}
}

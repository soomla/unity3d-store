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
using System.Linq;
using System.Collections;
using Soomla;

namespace Soomla.Store
{
	/// <summary>
	/// This class holds the store's meta data including:
	/// virtual currencies definitions,
	/// virtual currency packs definitions,
	/// virtual goods definitions,
	/// virtual categories definitions, and
	/// </summary>
	public class StoreInfo
	{

		protected const string TAG = "SOOMLA/UNITY StoreInfo"; // used for Log error messages

		static StoreInfo _instance = null;
		static StoreInfo instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new StoreInfoAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new StoreInfoIOS();
                    #elif UNITY_WP8 && !UNITY_EDITOR
                    _instance = new StoreInfoWP();
                    #else
                    _instance = new StoreInfo();
					#endif
				}
				return _instance;
			}
		}

		private static bool assetsArrayHasMarketIdDuplicates(PurchasableVirtualItem[] assetsArray) {
			HashSet<String> marketIds = new HashSet<String>();
			foreach (PurchasableVirtualItem pvi in assetsArray) {
				if (pvi.PurchaseType.GetType() == typeof(PurchaseWithMarket)) {
					String currentMarketId = ((PurchaseWithMarket)pvi.PurchaseType).MarketItem.ProductId;
					if (marketIds.Contains(currentMarketId)) {
						return false;
					} else {
						marketIds.Add(currentMarketId);
					}
				}
			}
			return true;
		}

		private static void validateStoreAssets(IStoreAssets storeAssets) {
			if (storeAssets == null) {
				throw new ArgumentException("The given store assets can't be null!");
			}

			if (storeAssets.GetCurrencies() == null ||
					storeAssets.GetCurrencyPacks() == null ||
					storeAssets.GetGoods() == null ||
					storeAssets.GetCategories() == null) {
				throw new ArgumentException("All IStoreAssets methods shouldn't return NULL-pointer references!");
			}

			if (!assetsArrayHasMarketIdDuplicates(storeAssets.GetGoods()) 
			    || !assetsArrayHasMarketIdDuplicates(storeAssets.GetCurrencyPacks())) {
				throw new ArgumentException("The given store assets has duplicates at marketItem productId!");
			}
		}

		/// <summary>
		/// NOTE: This function is manually called when you initialize SoomlaStore. You won't need to call
		/// 		it on regular use of SOOMLA Store.
		/// 
		/// Initializes <code>StoreInfo</code>.
		/// On first initialization, when the database doesn't have any previous version of the store
     	/// metadata, <code>StoreInfo</code> gets loaded from the given <code>IStoreAssets</code>.
     	/// After the first initialization, <code>StoreInfo</code> will be initialized from the database.
		/// 
		/// IMPORTANT: If you want to override the metadata on the current <code>StoreInfo</code>, you'll have to bump
		/// the version of your implementation of <code>IStoreAssets</code> in order to remove the
		/// metadata when the application loads. Bumping the version is done by returning a higher number
		/// in {@link com.soomla.store.IStoreAssets#getVersion()}.
		/// </summary>
		public static void SetStoreAssets(IStoreAssets storeAssets){
			SoomlaUtils.LogDebug(TAG, "Setting store assets in SoomlaInfo");
			try {
				validateStoreAssets(storeAssets);

				instance._setStoreAssets(storeAssets);
				
				// At this point we have StoreInfo JSON saved at the local key-value storage. We can just
				// continue by initializing from DB.
				
				initializeFromDB();
			}
			catch (System.ArgumentException exception) {
				SoomlaUtils.LogError(TAG, exception.Message);
			}
		}

		/// <summary>
		/// Gets the item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">Item id.</param>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if item is not found.</exception>
		/// <returns>Item with the given id.</returns>
		public static VirtualItem GetItemByItemId(string itemId) {
			SoomlaUtils.LogDebug(TAG, "Trying to fetch an item with itemId: " + itemId);

			VirtualItem item;
			if (VirtualItems != null && VirtualItems.TryGetValue(itemId, out item)) {
				return item;
			}

			throw new VirtualItemNotFoundException("itemId", itemId);
		}

		/// <summary>
		/// Gets the purchasable item with the given <c>productId</c>.
		/// </summary>
		/// <param name="productId">Product id.</param>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if item is not found.</exception>
		/// <returns>Purchasable virtual item with the given id.</returns>
		public static PurchasableVirtualItem GetPurchasableItemWithProductId(string productId) {
			SoomlaUtils.LogDebug(TAG, "Trying to fetch a purchasable item with productId: " + productId);

			PurchasableVirtualItem item;
			if (PurchasableItems != null && PurchasableItems.TryGetValue(productId, out item)) {
				return item;
			}

			throw new VirtualItemNotFoundException("productId", productId);
		}

		/// <summary>
		/// Gets the category that the virtual good with the given <c>goodItemId</c> belongs to.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <exception cref="VirtualItemNotFoundException">Exception is thrown if category is not found.</exception>
		/// <returns>Category that the item with given id belongs to.</returns>
		public static VirtualCategory GetCategoryForVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Trying to fetch a category for a good with itemId: " + goodItemId);

			VirtualCategory category;
			if (GoodsCategories != null && GoodsCategories.TryGetValue(goodItemId, out category)) {
				return category;
			}

			throw new VirtualItemNotFoundException("goodItemId of category", goodItemId);
		}

		/// <summary>
		/// Gets the first upgrade for virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>The first upgrade for virtual good with the given id.</returns>
		public static UpgradeVG GetFirstUpgradeForVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Trying to fetch first upgrade of a good with itemId: " + goodItemId);

			List<UpgradeVG> upgrades;
			if (GoodsUpgrades != null && GoodsUpgrades.TryGetValue(goodItemId, out upgrades)) {
				return upgrades.FirstOrDefault(up => string.IsNullOrEmpty(up.PrevItemId));
			}

			return null;
		}

		/// <summary>
		/// Gets the last upgrade for the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">item id</param>
		/// <returns>last upgrade for virtual good with the given id</returns>
		public static UpgradeVG GetLastUpgradeForVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Trying to fetch last upgrade of a good with itemId: " + goodItemId);

			List<UpgradeVG> upgrades;
			if (GoodsUpgrades != null && GoodsUpgrades.TryGetValue(goodItemId, out upgrades)) {
				return upgrades.FirstOrDefault(up => string.IsNullOrEmpty(up.NextItemId));
			}

			return null;
		}

		/// <summary>
		/// Gets all the upgrades for the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Item id.</param>
		/// <returns>All upgrades for virtual good with the given id.</returns>
		public static List<UpgradeVG> GetUpgradesForVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Trying to fetch upgrades of a good with itemId: " + goodItemId);

			List<UpgradeVG> upgrades;
			if (GoodsUpgrades != null && GoodsUpgrades.TryGetValue(goodItemId, out upgrades)) {
				return upgrades;
			}

			return null;
		}

		/// <summary>
		/// Saves the store's metadata in the database as JSON.
		/// </summary>
		public static void Save() {
			string store_json = toJSONObject().print();
			SoomlaUtils.LogDebug(TAG, "saving StoreInfo to DB. json is: " + store_json);
			string key = keyMetaStoreInfo();
			KeyValueStorage.SetValue(key, store_json);

			instance.loadNativeFromDB();
		}

		/// <summary>
		/// Replaces the given virtual item, and then saves the store's metadata
		/// (if requested).
		/// </summary>
		/// <param name="virtualItem">the virtual item to replace.</param>
		/// <param name="saveToDB">should the virtual item be persisted to local DB</param>
		public static void Save(VirtualItem virtualItem, bool saveToDB = true) {
			replaceVirtualItem(virtualItem);

			if (saveToDB) {
				Save();
			}
		}

		/// <summary>
		/// Replaces the given virtual item, and then saves the store's metadata
		/// (if requested).
		/// </summary>
		/// <param name="virtualItem">the virtual item to replace.</param>
		/// <param name="saveToDB">should the virtual item be persisted to local DB</param>
		public static void Save(List<VirtualItem> virtualItems, bool saveToDB = true) {
			if ((virtualItems == null) && (virtualItems.Count == 0)) {
				return;
			}

			foreach(VirtualItem virtualItem in virtualItems) {
				replaceVirtualItem(virtualItem);
			}

			if (saveToDB) {
				Save();
			}
		}


		/** Protected Functions **/
		/** These protected virtual functions will only run when in editor **/

		virtual protected void _setStoreAssets(IStoreAssets storeAssets) {
#if UNITY_EDITOR
			string storeJSON = IStoreAssetsToJSON(storeAssets);

			KeyValueStorage.SetValue(keyMetaStoreInfo(), storeJSON);
#endif
		}

		protected virtual void loadNativeFromDB() { /* no implementation for this in editor... only devcies */ }


		/** Protected Functions **/

		protected static string IStoreAssetsToJSON(IStoreAssets storeAssets) {

			// Utils.LogDebug(TAG, "Adding currency");
			JSONObject currencies = new JSONObject(JSONObject.Type.ARRAY);
			foreach(VirtualCurrency vi in storeAssets.GetCurrencies()) {
				currencies.Add(vi.toJSONObject());
			}

			// Utils.LogDebug(TAG, "Adding packs");
			JSONObject packs = new JSONObject(JSONObject.Type.ARRAY);
			foreach(VirtualCurrencyPack vi in storeAssets.GetCurrencyPacks()) {
				packs.Add(vi.toJSONObject());
			}

			// Utils.LogDebug(TAG, "Adding goods");
			JSONObject suGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject ltGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject eqGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject upGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject paGoods = new JSONObject(JSONObject.Type.ARRAY);
			foreach(VirtualGood g in storeAssets.GetGoods()){
				if (g is SingleUseVG) {
					suGoods.Add(g.toJSONObject());
				} else if (g is EquippableVG) {
					eqGoods.Add(g.toJSONObject());
				} else if (g is UpgradeVG) {
					upGoods.Add(g.toJSONObject());
				} else if (g is LifetimeVG) {
					ltGoods.Add(g.toJSONObject());
				} else if (g is SingleUsePackVG) {
					paGoods.Add(g.toJSONObject());
				}
			}
			JSONObject goods = new JSONObject(JSONObject.Type.OBJECT);
			goods.AddField(StoreJSONConsts.STORE_GOODS_SU, suGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_LT, ltGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_EQ, eqGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_UP, upGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_PA, paGoods);

			// Utils.LogDebug(TAG, "Adding categories");
			JSONObject categories = new JSONObject(JSONObject.Type.ARRAY);
			foreach(VirtualCategory vi in storeAssets.GetCategories()) {
				categories.Add(vi.toJSONObject());
			}

			// Utils.LogDebug(TAG, "Preparing StoreAssets  JSONObject");
			JSONObject storeAssetsObj = new JSONObject(JSONObject.Type.OBJECT);
			storeAssetsObj.AddField(StoreJSONConsts.STORE_CATEGORIES, categories);
			storeAssetsObj.AddField(StoreJSONConsts.STORE_CURRENCIES, currencies);
			storeAssetsObj.AddField(StoreJSONConsts.STORE_CURRENCYPACKS, packs);
			storeAssetsObj.AddField(StoreJSONConsts.STORE_GOODS, goods);

			return storeAssetsObj.print();
		}


		/** Private functions **/

		private static void initializeFromDB() {
			string key = keyMetaStoreInfo();
			string val = KeyValueStorage.GetValue(key);
			
			if (string.IsNullOrEmpty(val)){
				SoomlaUtils.LogError(TAG, "store json is not in DB. Make sure you initialized SoomlaStore with your Store assets. The App will shut down now.");
				Application.Quit();
			}
			
			SoomlaUtils.LogDebug(TAG, "the metadata-economy json (from DB) is " + val);

			JSONObject storeJSON = new JSONObject (val);
			fromJSONObject (storeJSON);
		}

		private static void fromJSONObject(JSONObject storeJSON)
		{
			VirtualItems = new Dictionary<string, VirtualItem> ();
			PurchasableItems = new Dictionary<string, PurchasableVirtualItem> ();
			GoodsCategories = new Dictionary<string, VirtualCategory> ();
			GoodsUpgrades = new Dictionary<string, List<UpgradeVG>> ();
			CurrencyPacks = new List<VirtualCurrencyPack> ();
			Goods = new List<VirtualGood> ();
			Categories = new List<VirtualCategory> ();
			Currencies = new List<VirtualCurrency> ();
			if (storeJSON.HasField (StoreJSONConsts.STORE_CURRENCIES)) {
				List<JSONObject> objs = storeJSON [StoreJSONConsts.STORE_CURRENCIES].list;
				foreach (JSONObject o in objs) {
					VirtualCurrency c = new VirtualCurrency (o);
					Currencies.Add (c);
				}
			}
			if (storeJSON.HasField (StoreJSONConsts.STORE_CURRENCYPACKS)) {
				List<JSONObject> objs = storeJSON [StoreJSONConsts.STORE_CURRENCYPACKS].list;
				foreach (JSONObject o in objs) {
					VirtualCurrencyPack c = new VirtualCurrencyPack (o);
					CurrencyPacks.Add (c);
				}
			}
			if (storeJSON.HasField (StoreJSONConsts.STORE_GOODS)) {
				JSONObject goods = storeJSON [StoreJSONConsts.STORE_GOODS];
				if (goods.HasField (StoreJSONConsts.STORE_GOODS_SU)) {
					List<JSONObject> suGoods = goods [StoreJSONConsts.STORE_GOODS_SU].list;
					foreach (JSONObject o in suGoods) {
						var c = new SingleUseVG (o);
						Goods.Add (c);
					}
				}
				if (goods.HasField (StoreJSONConsts.STORE_GOODS_LT)) {
					List<JSONObject> ltGoods = goods [StoreJSONConsts.STORE_GOODS_LT].list;
					foreach (JSONObject o in ltGoods) {
						LifetimeVG c = new LifetimeVG (o);
						Goods.Add (c);
					}
				}
				if (goods.HasField (StoreJSONConsts.STORE_GOODS_EQ)) {
					List<JSONObject> eqGoods = goods [StoreJSONConsts.STORE_GOODS_EQ].list;
					foreach (JSONObject o in eqGoods) {
						EquippableVG c = new EquippableVG (o);
						Goods.Add (c);
					}
				}
				if (goods.HasField (StoreJSONConsts.STORE_GOODS_PA)) {
					List<JSONObject> paGoods = goods [StoreJSONConsts.STORE_GOODS_PA].list;
					foreach (JSONObject o in paGoods) {
						SingleUsePackVG c = new SingleUsePackVG (o);
						Goods.Add (c);
					}
				}
				if (goods.HasField (StoreJSONConsts.STORE_GOODS_UP)) {
					List<JSONObject> upGoods = goods [StoreJSONConsts.STORE_GOODS_UP].list;
					foreach (JSONObject o in upGoods) {
						UpgradeVG c = new UpgradeVG (o);
						Goods.Add (c);
					}
				}
			}

			if (storeJSON.HasField(StoreJSONConsts.STORE_CATEGORIES)) {
				List<JSONObject> categories = storeJSON[StoreJSONConsts.STORE_CATEGORIES].list;
				foreach (JSONObject o in categories){
					VirtualCategory category = new VirtualCategory(o);
					Categories.Add(category);
				}
			}

			updateAggregatedLists ();
		}

		private static void updateAggregatedLists (){
			// rewritten from android java code
			foreach (VirtualCurrency vi in Currencies) {
				VirtualItems.AddOrUpdate(vi.ItemId, vi);
			}
			foreach (VirtualCurrencyPack vi in CurrencyPacks) {
				VirtualItems.AddOrUpdate(vi.ItemId, vi);
				PurchaseType purchaseType = vi.PurchaseType;
				if (purchaseType is PurchaseWithMarket) {
					PurchasableItems.AddOrUpdate(((PurchaseWithMarket)purchaseType).MarketItem.ProductId, vi);
				}
			}
			foreach (VirtualGood vi in Goods) {
				VirtualItems.AddOrUpdate(vi.ItemId, vi);
				if (vi is UpgradeVG) {
					List<UpgradeVG> upgrades;
					if (!GoodsUpgrades.TryGetValue (((UpgradeVG)vi).GoodItemId, out upgrades)) {
						upgrades = new List<UpgradeVG> ();
						GoodsUpgrades.Add(((UpgradeVG)vi).GoodItemId, upgrades);
					}
					upgrades.Add ((UpgradeVG)vi);
				}
				PurchaseType purchaseType = vi.PurchaseType;
				if (purchaseType is PurchaseWithMarket) {
					PurchasableItems.AddOrUpdate(((PurchaseWithMarket)purchaseType).MarketItem.ProductId, vi);
				}
			}
			foreach (VirtualCategory category in Categories) {
				foreach (string goodItemId in category.GoodItemIds) {
					GoodsCategories.AddOrUpdate(goodItemId, category);
				}
			}
		}

		/// <summary>
		/// Replaces an old virtual item with a new one by doing the following:
		/// 1. Determines the type of the given virtual item.
		/// 2. Looks for the given virtual item in the relevant list, according to its type.
		/// 3. If found, removes it.
		/// 4. Adds the given virtual item.
		/// </summary>
		/// <param name="virtualItem">the virtual item that replaces the old one if exists.</param>
		private static void replaceVirtualItem(VirtualItem virtualItem) {
			VirtualItems.AddOrUpdate(virtualItem.ItemId, virtualItem);
			
			if (virtualItem is VirtualCurrency) {
				for(int i=0; i<Currencies.Count(); i++) {
					if (Currencies[i].ItemId == virtualItem.ItemId) {
						Currencies.RemoveAt(i);
						break;
					}
				}
				Currencies.Add((VirtualCurrency)virtualItem);
			}
			
			if (virtualItem is VirtualCurrencyPack) {
				VirtualCurrencyPack vcp = (VirtualCurrencyPack)virtualItem;
				if (vcp.PurchaseType is PurchaseWithMarket) {
					PurchasableItems.AddOrUpdate(((PurchaseWithMarket) vcp.PurchaseType).MarketItem
					                      .ProductId, vcp);
				}
				
				for(int i=0; i<CurrencyPacks.Count(); i++) {
					if (CurrencyPacks[i].ItemId == vcp.ItemId) {
						CurrencyPacks.RemoveAt(i);
						break;
					}
				}
				CurrencyPacks.Add(vcp);
			}
			
			if (virtualItem is VirtualGood) {
				VirtualGood vg = (VirtualGood)virtualItem;
				
				if (vg is UpgradeVG) {
					List<UpgradeVG> upgrades;
					if (!GoodsUpgrades.TryGetValue (((UpgradeVG) vg).GoodItemId, out upgrades)) {
						upgrades = new List<UpgradeVG>();
						GoodsUpgrades.Add(((UpgradeVG) vg).ItemId, upgrades);
					}
					upgrades.Add((UpgradeVG) vg);
				}

				if (vg.PurchaseType is PurchaseWithMarket) {
					PurchasableItems.AddOrUpdate(((PurchaseWithMarket) vg.PurchaseType).MarketItem
					                      .ProductId, vg);
				}
				
				for(int i=0; i<Goods.Count(); i++) {
					if (Goods[i].ItemId == vg.ItemId) {
						Goods.RemoveAt(i);
						break;
					}
				}
				Goods.Add(vg);
			}
		}

		/// <summary>
		/// Converts <code>StoreInfo</code> to a <code>JSONObject</code>.
		/// </summary>
		/// <returns><code>JSONObject</code> representation of <code>StoreInfo</code>.</returns>
		private static JSONObject toJSONObject(){
			
			JSONObject currencies = new JSONObject(JSONObject.Type.ARRAY);
			foreach(VirtualCurrency c in Currencies){
				currencies.Add(c.toJSONObject());
			}

			JSONObject currencyPacks = new JSONObject(JSONObject.Type.ARRAY);
			foreach (VirtualCurrencyPack pack in CurrencyPacks){
				currencyPacks.Add(pack.toJSONObject());
			}
			
			JSONObject goods = new JSONObject();
			JSONObject suGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject ltGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject eqGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject paGoods = new JSONObject(JSONObject.Type.ARRAY);
			JSONObject upGoods = new JSONObject(JSONObject.Type.ARRAY);
			foreach (VirtualGood good in Goods){
				if (good is SingleUseVG) {
					suGoods.Add(good.toJSONObject());
				} else if (good is UpgradeVG) {
					upGoods.Add(good.toJSONObject());
				} else if (good is EquippableVG) {
					eqGoods.Add(good.toJSONObject());
				} else if (good is SingleUsePackVG) {
					paGoods.Add(good.toJSONObject());
				} else if (good is LifetimeVG) {
					ltGoods.Add(good.toJSONObject());
				}
			}
			

			JSONObject categories = new JSONObject(JSONObject.Type.ARRAY);
			foreach (VirtualCategory cat in Categories){
				categories.Add(cat.toJSONObject());
			}
			
			JSONObject jsonObject = new JSONObject();
			goods.AddField(StoreJSONConsts.STORE_GOODS_SU, suGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_LT, ltGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_EQ, eqGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_PA, paGoods);
			goods.AddField(StoreJSONConsts.STORE_GOODS_UP, upGoods);
			
			jsonObject.AddField(StoreJSONConsts.STORE_CATEGORIES, categories);
			jsonObject.AddField(StoreJSONConsts.STORE_CURRENCIES, currencies);
			jsonObject.AddField(StoreJSONConsts.STORE_GOODS, goods);
			jsonObject.AddField(StoreJSONConsts.STORE_CURRENCYPACKS, currencyPacks);
			
			return jsonObject;
		}





		/** Lists containing Store metadata **/
		
		// convenient hash of virtual items
		public static Dictionary<string, VirtualItem> VirtualItems = new Dictionary<string, VirtualItem>();
		
		// convenient hash of purchasable virtual items
		public static Dictionary<string, PurchasableVirtualItem> PurchasableItems = new Dictionary<string, PurchasableVirtualItem>();

		// convenient hash of goods-categories
		public static Dictionary<string, VirtualCategory> GoodsCategories = new Dictionary<string, VirtualCategory>();
		
		// convenient hash of good-upgrades
		public static Dictionary<string, List<UpgradeVG>> GoodsUpgrades = new Dictionary<string, List<UpgradeVG>>();
		
		// list of virtual currencies
		public static List<VirtualCurrency> Currencies = new List<VirtualCurrency>();
		
		// list of currency-packs
		public static List<VirtualCurrencyPack> CurrencyPacks = new List<VirtualCurrencyPack>();
		
		// list of virtual goods
		public static List<VirtualGood> Goods = new List<VirtualGood>();
		
		// list of virtul categories
		public static List<VirtualCategory> Categories = new List<VirtualCategory>();


		/** Private Members **/

		private static string keyMetaStoreInfo() {
			return "meta.storeinfo";
		}
	}
}

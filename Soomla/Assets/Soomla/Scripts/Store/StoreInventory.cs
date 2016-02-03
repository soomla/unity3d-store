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
using System.Collections.Generic;
using System.Linq;

namespace Soomla.Store
{
	/// <summary>
	/// This class will help you do your day to day virtual economy operations easily.
	/// You can give or take items from your users. You can buy items or upgrade them.
	/// You can also check their equipping status and change it.
	/// </summary>
	public class StoreInventory
	{

		protected const string TAG = "SOOMLA StoreInventory";

		/// <summary>
		/// Checks if there is enough funds to afford <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">id of item to be checked</param>
		/// <returns>True if there are enough funds to afford the virtual item with the given item id </returns>
		public static bool CanAfford(string itemId) {
			SoomlaUtils.LogDebug(TAG, "Checking can afford: " + itemId);
			
			PurchasableVirtualItem pvi = (PurchasableVirtualItem) StoreInfo.GetItemByItemId(itemId);
			return pvi.CanAfford();
		}
		
		/// <summary>
		/// Buys the item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">id of item to be bought</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item to be bought is not found.</exception>
		/// <exception cref="InsufficientFundsException">Thrown if the user does not have enough funds.</exception>
		public static void BuyItem(string itemId) {
			BuyItem(itemId, "");
		}
		
		/// <summary>
		/// Buys the item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">id of item to be bought</param>
		/// <param name="payload">a string you want to be assigned to the purchase. This string
		/// is saved in a static variable and will be given bacl to you when the purchase is completed.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item to be bought is not found.</exception>
		/// <exception cref="InsufficientFundsException">Thrown if the user does not have enough funds.</exception>
		public static void BuyItem(string itemId, string payload) {
			SoomlaUtils.LogDebug(TAG, "Buying: " + itemId);

			PurchasableVirtualItem pvi = (PurchasableVirtualItem) StoreInfo.GetItemByItemId(itemId);
			pvi.Buy(payload);
		}

		/// <summary>
		/// Retrieves the balance of the virtual item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">Id of the virtual item to be fetched.</param>
		/// <returns>Balance of the virtual item with the given item id.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static int GetItemBalance(string itemId) {
			int amount;
			if (localItemBalances.TryGetValue(itemId, out amount)) {
				return amount;
			}
			
			VirtualItem item = StoreInfo.GetItemByItemId(itemId);
			return item.GetBalance();
		}

		/** VIRTUAL ITEMS **/

		/// <summary>
		/// Gives your user the given amount of the virtual item with the given <c>itemId</c>.
		/// For example, when your user plays your game for the first time you GIVE him/her 1000 gems.
		///
		/// NOTE: This action is different than buy -
		/// You use <c>give(int amount)</c> to give your user something for free.
		/// You use <c>buy()</c> to give your user something and you get something in return.
		/// </summary>
		/// <param name="itemId">Id of the item to be given.</param>
		/// <param name="amount">Amount of the item to be given.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void GiveItem(string itemId, int amount) {
			SoomlaUtils.LogDebug(TAG, "Giving: " + amount + " pieces of: " + itemId);

			VirtualItem item = StoreInfo.GetItemByItemId(itemId);
			item.Give(amount);
		}

		/// <summary>
		/// Takes from your user the given amount of the virtual item with the given <c>itemId</c>.
		/// For example, when your user requests a refund, you need to TAKE the item he/she is returning from him/her.
		/// </summary>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="amount">Amount.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void TakeItem(string itemId, int amount) {
			SoomlaUtils.LogDebug(TAG, "Taking: " + amount + " pieces of: " + itemId);

			VirtualItem item = StoreInfo.GetItemByItemId(itemId);
			item.Take(amount);
		}


		/// <summary>
		/// Equips the virtual good with the given <c>goodItemId</c>.
		/// Equipping means that the user decides to currently use a specific virtual good.
		/// For more details and examples <see cref="com.soomla.store.domain.virtualGoods.EquippableVG"/>.
		/// </summary>
		/// <param name="goodItemId">Id of the good to be equipped.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		/// <exception cref="NotEnoughGoodsException"></exception>
		public static void EquipVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Equipping: " + goodItemId);

			EquippableVG good = (EquippableVG) StoreInfo.GetItemByItemId(goodItemId);
			
			try {
				good.Equip();
			} catch (NotEnoughGoodsException e) {
				SoomlaUtils.LogError(TAG, "UNEXPECTED! Couldn't equip something");
				throw e;
			}
		}

		/// <summary>
		/// Unequips the virtual good with the given <c>goodItemId</c>. Unequipping means that the
		/// user decides to stop using the virtual good he/she is currently using.
		/// For more details and examples <see cref="com.soomla.store.domain.virtualGoods.EquippableVG"/>.
		/// </summary>
		/// <param name="goodItemId">Id of the good to be unequipped.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void UnEquipVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "UnEquipping: " + goodItemId);
			
			EquippableVG good = (EquippableVG) StoreInfo.GetItemByItemId(goodItemId);
			good.Unequip();
		}

		/// <summary>
		/// Checks if the virtual good with the given <c>goodItemId</c> is currently equipped.
		/// </summary>
		/// <param name="goodItemId">Id of the virtual good who we want to know if is equipped.</param>
		/// <returns>True if the virtual good is equipped, false otherwise.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static bool IsVirtualGoodEquipped(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Checking if " + goodItemId + " is equipped");

			EquippableVG good = (EquippableVG) StoreInfo.GetItemByItemId(goodItemId);
			
			return VirtualGoodsStorage.IsEquipped(good);;
		}

		/// <summary>
        /// Checks currently equipped good in given <c>category</c>
        /// </summary>
        /// <param name="category">Category we want to check</param>
        /// <returns>EquippableVG otherwise null</returns>
	    public static EquippableVG GetEquippedVirtualGood(VirtualCategory category){
            SoomlaUtils.LogDebug(TAG, "Checking equipped goood in " + category.Name + " category");

	        foreach (string goodItemId in category.GoodItemIds)
	        {
                EquippableVG good = (EquippableVG) StoreInfo.GetItemByItemId(goodItemId);

	            if (good != null && good.Equipping == EquippableVG.EquippingModel.CATEGORY &&
                    VirtualGoodsStorage.IsEquipped(good) &&
	                StoreInfo.GetCategoryForVirtualGood(goodItemId) == category)
	                return good;
	        }
            SoomlaUtils.LogError(TAG, "There is no virtual good equipped in " + category.Name + " category");
	        return null;
	    }
		
		/// <summary>
		/// Retrieves the upgrade level of the virtual good with the given <c>goodItemId</c>.
		/// For Example:
		/// Let's say there's a strength attribute to one of the characters in your game and you provide
		/// your users with the ability to upgrade that strength on a scale of 1-3.
		/// This is what you've created:
		/// 1. <c>SingleUseVG</c> for "strength".
		/// 2. <c>UpgradeVG</c> for strength 'level 1'.
		/// 3. <c>UpgradeVG</c> for strength 'level 2'.
		/// 4. <c>UpgradeVG</c> for strength 'level 3'.
		/// In the example, this function will retrieve the upgrade level for "strength" (1, 2, or 3).
		/// </summary>
		/// <param name="goodItemId">Good item identifier.</param>
		/// <returns>The good upgrade level.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static int GetGoodUpgradeLevel(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Checking " + goodItemId + " upgrade level");

			VirtualGood good = (VirtualGood) StoreInfo.GetItemByItemId(goodItemId);
			if (good == null) {
				SoomlaUtils.LogError(TAG, "You tried to get the level of a non-existant virtual good.");
				return 0;
			}
			UpgradeVG upgradeVG = VirtualGoodsStorage.GetCurrentUpgrade(good);
			if (upgradeVG == null) {
				return 0; //no upgrade
			}
			
			UpgradeVG first = StoreInfo.GetFirstUpgradeForVirtualGood(goodItemId);
			int level = 1;
			while (first.ItemId != upgradeVG.ItemId) {
				first = (UpgradeVG) StoreInfo.GetItemByItemId(first.NextItemId);
				level++;
			}
			
			return level;
		}

		/// <summary>
		/// Retrieves the current upgrade of the good with the given id.
		/// </summary>
		/// <param name="goodItemId">Id of the good whose upgrade we want to fetch. </param>
		/// <returns>The good's current upgrade.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static string GetGoodCurrentUpgrade(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "Checking " + goodItemId + " current upgrade");

			VirtualGood good = (VirtualGood) StoreInfo.GetItemByItemId(goodItemId);
			
			UpgradeVG upgradeVG = VirtualGoodsStorage.GetCurrentUpgrade(good);
			if (upgradeVG == null) {
				return "";
			}
			return upgradeVG.ItemId;
		}

		/// <summary>
		/// Upgrades the virtual good with the given <c>goodItemId</c> by doing the following:
		/// 1. Checks if the good is currently upgraded or if this is the first time being upgraded.
		/// 2. If the good is currently upgraded, upgrades to the next upgrade in the series.
		/// In case there are no more upgrades available(meaning the current upgrade is the last available),
		/// the function returns.
		/// 3. If the good has never been upgraded before, the function upgrades it to the first
		/// available upgrade with the first upgrade of the series.
		/// </summary>
		/// <param name="goodItemId">Good item identifier.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void UpgradeGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling UpgradeGood with: " + goodItemId);
			VirtualGood good = (VirtualGood) StoreInfo.GetItemByItemId(goodItemId);
			
			UpgradeVG upgradeVG = VirtualGoodsStorage.GetCurrentUpgrade(good);
			
			if (upgradeVG != null) {
				String nextItemId = upgradeVG.NextItemId;
				if (string.IsNullOrEmpty(nextItemId)) {
					return;
				}
				UpgradeVG vgu = (UpgradeVG) StoreInfo.GetItemByItemId(nextItemId);
				vgu.Buy("");
			} else {
				UpgradeVG first = StoreInfo.GetFirstUpgradeForVirtualGood(goodItemId);
				if (first != null) {
					first.Buy("");
				}
			}
		}

		/// <summary>
		/// Removes all upgrades from the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Id of the good whose upgrades are to be removed.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void RemoveGoodUpgrades(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling RemoveGoodUpgrades with: " + goodItemId);

			List<UpgradeVG> upgrades = StoreInfo.GetUpgradesForVirtualGood(goodItemId);
			foreach (UpgradeVG upgrade in upgrades) {
				VirtualGoodsStorage.Remove(upgrade, 1, true);
			}
			VirtualGood good = (VirtualGood) StoreInfo.GetItemByItemId(goodItemId);
			VirtualGoodsStorage.RemoveUpgrades(good);
		}

		/// <summary>
		/// This function refreshes a local set of objects that will hold your user's balances in memory for quick
		/// and more efficient fetching for your game UI.
		/// This way, we save many JNI or static calls to native platforms.
		/// 
		/// NOTE: You don't need to call this function as it's automatically called when the game initializes.
		/// NOTE: This is less useful when you work in editor.
		/// </summary>
		public static void RefreshLocalInventory() {
			SoomlaUtils.LogDebug(TAG, "Refreshing local inventory");

			localItemBalances = new Dictionary<string, int> ();
			localUpgrades = new Dictionary<string, LocalUpgrade>();
			localEquippedGoods = new HashSet<string>();
			
			foreach(VirtualCurrency item in StoreInfo.Currencies){
				localItemBalances[item.ItemId] = VirtualCurrencyStorage.GetBalance(item);
			}
			
			foreach(VirtualGood item in StoreInfo.Goods){
				localItemBalances[item.ItemId] =  VirtualGoodsStorage.GetBalance(item);
				
				UpgradeVG upgrade = VirtualGoodsStorage.GetCurrentUpgrade(item);
				if (upgrade != null) {
					int upgradeLevel = GetGoodUpgradeLevel(item.ItemId);
					localUpgrades.AddOrUpdate(item.ItemId, new LocalUpgrade { itemId = upgrade.ItemId, level = upgradeLevel });
				}
				
				if (item is EquippableVG) {
					if (VirtualGoodsStorage.IsEquipped((EquippableVG)item)) {
						localEquippedGoods.Add(item.ItemId);
					}
				}
			}
		}


		/** A set of private functions to refresh the local inventory whenever there are changes on runtime. **/

		public static void RefreshOnGoodUpgrade(VirtualGood vg, UpgradeVG uvg) {
			if (uvg == null) {
				localUpgrades.Remove(vg.ItemId);
			} else {
				int upgradeLevel = GetGoodUpgradeLevel(vg.ItemId);
				LocalUpgrade upgrade;
				if (localUpgrades.TryGetValue(vg.ItemId, out upgrade)) {
					upgrade.itemId = uvg.ItemId;
					upgrade.level = upgradeLevel;
				} else {
					localUpgrades.Add(vg.ItemId, new LocalUpgrade { itemId = uvg.ItemId, level = upgradeLevel });
				}
			}
		}
		
		public static void RefreshOnGoodEquipped(EquippableVG equippable) {
			localEquippedGoods.Add(equippable.ItemId);
		}
		
		public static void RefreshOnGoodUnEquipped(EquippableVG equippable) {
			localEquippedGoods.Remove(equippable.ItemId);
		}
		
		public static void RefreshOnCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
			UpdateLocalBalance(virtualCurrency.ItemId, balance);
		}
		
		public static void RefreshOnGoodBalanceChanged(VirtualGood good, int balance, int amountAdded) {
			UpdateLocalBalance(good.ItemId, balance);
		}
		
		private static void UpdateLocalBalance(string itemId, int balance) {
			localItemBalances[itemId] = balance;
		}



		/** Private local balances **/

		private class LocalUpgrade {
			public int level;
			public string itemId;
		}
		
		private static Dictionary<string, int> localItemBalances = null;
		private static Dictionary<string, LocalUpgrade> localUpgrades = null;
		private static HashSet<string> localEquippedGoods = null;
	}
}

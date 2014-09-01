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

		static StoreInventory _instance = null;
		static StoreInventory instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new StoreInventoryAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new StoreInventoryIOS();
					#else
					_instance = new StoreInventory();
					#endif
				}
				return _instance;
			}
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
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling BuyItem with: " + itemId);
			instance._buyItem(itemId, payload);
		}

		virtual protected void _buyItem(string itemId, string payload) {
		}


		/** VIRTUAL ITEMS **/

		/// <summary>
		/// Retrieves the balance of the virtual item with the given <c>itemId</c>.
		/// </summary>
		/// <param name="itemId">Id of the virtual item to be fetched.</param>
		/// <returns>Balance of the virtual item with the given item id.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static int GetItemBalance(string itemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GetItemBalance with: " + itemId);
			return instance._getItemBalance(itemId);
		}

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
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GiveItem with itemId: " + itemId + " and amount: " + amount);
			instance._giveItem(itemId, amount);
		}

		/// <summary>
		/// Takes from your user the given amount of the virtual item with the given <c>itemId</c>.
		/// For example, when your user requests a refund, you need to TAKE the item he/she is returning from him/her.
		/// </summary>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="amount">Amount.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void TakeItem(string itemId, int amount) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling TakeItem with itemId: " + itemId + " and amount: " + amount);
			instance._takeItem(itemId, amount);
		}

		virtual protected int _getItemBalance(string itemId) {
			return 0;
		}

		virtual protected void _giveItem(string itemId, int amount) {
		}

		virtual protected void _takeItem(string itemId, int amount) {
		}


		/** VIRTUAL GOODS **/

		/// <summary>
		/// Equips the virtual good with the given <c>goodItemId</c>.
		/// Equipping means that the user decides to currently use a specific virtual good.
		/// For more details and examples <see cref="com.soomla.store.domain.virtualGoods.EquippableVG"/>.
		/// </summary>
		/// <param name="goodItemId">Id of the good to be equipped.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		/// <exception cref="NotEnoughGoodsException"></exception>
		public static void EquipVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling EquipVirtualGood with: " + goodItemId);
			instance._equipVirtualGood(goodItemId);
		}

		/// <summary>
		/// Unequips the virtual good with the given <c>goodItemId</c>. Unequipping means that the
		/// user decides to stop using the virtual good he/she is currently using.
		/// For more details and examples <see cref="com.soomla.store.domain.virtualGoods.EquippableVG"/>.
		/// </summary>
		/// <param name="goodItemId">Id of the good to be unequipped.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void UnEquipVirtualGood(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling UnEquipVirtualGood with: " + goodItemId);
			instance._unEquipVirtualGood(goodItemId);
		}

		/// <summary>
		/// Checks if the virtual good with the given <c>goodItemId</c> is currently equipped.
		/// </summary>
		/// <param name="goodItemId">Id of the virtual good who we want to know if is equipped.</param>
		/// <returns>True if the virtual good is equipped, false otherwise.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static bool IsVirtualGoodEquipped(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling IsVirtualGoodEquipped with: " + goodItemId);
			return instance._isVertualGoodEquipped(goodItemId);
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
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GetGoodUpgradeLevel with: " + goodItemId);
			return instance._getGoodUpgradeLevel(goodItemId);
		}

		/// <summary>
		/// Retrieves the current upgrade of the good with the given id.
		/// </summary>
		/// <param name="goodItemId">Id of the good whose upgrade we want to fetch. </param>
		/// <returns>The good's current upgrade.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static string GetGoodCurrentUpgrade(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling GetGoodCurrentUpgrade with: " + goodItemId);
			return instance._getGoodCurrentUpgrade(goodItemId);
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
			instance._upgradeGood(goodItemId);
		}

		/// <summary>
		/// Removes all upgrades from the virtual good with the given <c>goodItemId</c>.
		/// </summary>
		/// <param name="goodItemId">Id of the good whose upgrades are to be removed.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void RemoveGoodUpgrades(string goodItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling RemoveGoodUpgrades with: " + goodItemId);
			instance._removeGoodUpgrades(goodItemId);
		}

		virtual protected void _equipVirtualGood(string goodItemId) {
		}

		virtual protected void _unEquipVirtualGood(string goodItemId) {
		}

		virtual protected bool _isVertualGoodEquipped(string goodItemId) {
			return false;
		}

		virtual protected int _getGoodUpgradeLevel(string goodItemId) {
			return 0;
		}

		virtual protected string _getGoodCurrentUpgrade(string goodItemId) {
			return null;
		}

		virtual protected void _upgradeGood(string goodItemId) {
		}

		virtual protected void _removeGoodUpgrades(string goodItemId) {
		}



		/** NON-CONSUMABLES **/

		/// <summary>
		/// Checks if the non-consumable with the given <c>nonConsItemId</c> exists.
		/// </summary>
		/// <param name="nonConsItemId">Id of the item to check if exists.</param>
		/// <returns>True if non-consumable item with nonConsItemId exists, false otherwise.</returns>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static bool NonConsumableItemExists(string nonConsItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling NonConsumableItemExists with: " + nonConsItemId);
			return instance._nonConsumableItemExists(nonConsItemId);
		}

		/// <summary>
		/// Adds the non-consumable item with the given <c>nonConsItemId</c> to the non-consumable items storage.
		/// </summary>
		/// <param name="nonConsItemId">Id of the item to be added.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void AddNonConsumableItem(string nonConsItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling AddNonConsumableItem with: " + nonConsItemId);
			instance._addNonConsumableItem(nonConsItemId);
		}

		/// <summary>
		/// Removes the non-consumable item with the given <c>nonConsItemId</c> from the non-consumable 
		/// items storage.
		/// </summary>
		/// <param name="nonConsItemId">Id of the item to be removed.</param>
		/// <exception cref="VirtualItemNotFoundException">Thrown if the item is not found.</exception>
		public static void RemoveNonConsumableItem(string nonConsItemId) {
			SoomlaUtils.LogDebug(TAG, "SOOMLA/UNITY Calling RemoveNonConsumableItem with: " + nonConsItemId);
			instance._removeNonConsumableItem(nonConsItemId);
		}

		virtual protected bool _nonConsumableItemExists(string nonConsItemId) {
			return false;
		}

		virtual protected void _addNonConsumableItem(string nonConsItemId) {
		}

		virtual protected void _removeNonConsumableItem(string nonConsItemId) {
		}
	}
}




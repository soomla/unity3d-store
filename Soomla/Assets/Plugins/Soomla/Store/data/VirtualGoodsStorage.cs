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
/// See the License for the specific language governing perworlds and
/// limitations under the License.

using UnityEngine;
using System;

namespace Soomla.Store
{

	/// <summary>
	/// This class is an abstract definition of a Virtual Goods Storage.
	/// </summary>
	public class VirtualGoodsStorage : VirtualItemStorage
	{

		/// <summary>
		/// Holds an instance of <c>VirtualGoodsStorage</c> or <c>VirtualGoodsStorageAndroid</c> or <c>VirtualGoodsStorageIOS</c>.
		/// </summary>
		static VirtualGoodsStorage _instance = null;
	
		/// <summary>
		/// Determines which <c>VirtualGoodsStorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
		static VirtualGoodsStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new VirtualGoodsStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new VirtualGoodsStorageIOS();
                    #elif UNITY_WP8 && !UNITY_EDITOR
					_instance = new VirtualGoodsStorageWP();
                    #else
                    _instance = new VirtualGoodsStorage();
					#endif
				}
				return _instance;
			}
		}

		protected VirtualGoodsStorage() {
			TAG = "SOOMLA VirtualGoodsStorage";
		}


		/// <summary>
		/// Removes any upgrade associated with the given <code>VirtualGood</code>.
		/// </summary>
		/// <param name="good">VirtualGood to remove upgrade from.</param>
		public static void RemoveUpgrades(VirtualGood good){
			RemoveUpgrades(good, true);
		}

		/// <summary>
		/// Removes any upgrade associated with the given <code>VirtualGood</code>.
		/// </summary>
		/// <param name="good">VirtualGood to remove upgrade from.</param>
		/// <param name="notify">true will also post event.</param>
		public static void RemoveUpgrades(VirtualGood good, bool notify){
			SoomlaUtils.LogDebug(TAG, "Removing upgrade information from virtual good: "
			                     + good.ItemId);
			
			instance._removeUpgrades(good, notify);
		}

		/// <summary>
		/// Assigns a specific upgrade to the given virtual good.
		/// </summary>
		/// <param name="good">the virtual good to upgrade.</param>
		/// <param name="upgradeVG">the upgrade to assign.</param>
		public static void AssignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG){
			AssignCurrentUpgrade(good, upgradeVG, true);
		}

		/// <summary>
		/// Assigns a specific upgrade to the given virtual good.
		/// </summary>
		/// <param name="good">the virtual good to upgrade.</param>
		/// <param name="upgradeVG">the upgrade to assign.</param>
		/// <param name="notify">true will also post event.</param>
		public static void AssignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG, bool notify){
			SoomlaUtils.LogDebug(TAG, "Assigning upgrade " + upgradeVG.ItemId + " to virtual good: "
			                     + good.ItemId);

			instance._assignCurrentUpgrade(good, upgradeVG, notify);
		}

		/// <summary>
		/// Retrieves the current upgrade for the given virtual good.
		/// </summary>
		/// <param name="good">the virtual good to retrieve upgrade for.</param>
		/// <return>the current upgrade for the given virtual good.</return>
		public static UpgradeVG GetCurrentUpgrade(VirtualGood good){
			SoomlaUtils.LogDebug(TAG, "Fetching upgrade to virtual good: " + good.ItemId);
			
			return instance._getCurrentUpgrade(good);
		}

		/// <summary>
		/// Retrieves the current upgrade for the given virtual good.
		/// </summary>
		/// <param name="good">the virtual good to retrieve upgrade for.</param>
		/// <return>the current upgrade for the given virtual good.</return>
		public static bool IsEquipped(EquippableVG good){
			SoomlaUtils.LogDebug(TAG, "checking if virtual good with itemId: " + good.ItemId +
			                     " is equipped.");
			
			return instance._isEquipped(good);
		}

		/// <summary>
		/// Equips the given <code>EquippableVG</code>.
		/// </summary>
		/// <param name="good">the <code>EquippableVG</code> to equip.</param>
		public static void Equip(EquippableVG good){
			SoomlaUtils.LogDebug(TAG, "equipping: " + good.ItemId);
			
			Equip(good);
		}

		/// <summary>
		/// Equips the given <code>EquippableVG</code>.
		/// </summary>
		/// <param name="good">the <code>EquippableVG</code> to equip.</param>
		/// <param name="notify">true will also post event.</param>
		public static void Equip(EquippableVG good, bool notify){
			SoomlaUtils.LogDebug(TAG, "equipping: " + good.ItemId);
			
			instance._equip(good, notify);
		}

		/// <summary>
		/// UnEquips the given <code>EquippableVG</code>.
		/// </summary>
		/// <param name="good">the <code>EquippableVG</code> to equip.</param>
		public static void UnEquip(EquippableVG good){
			SoomlaUtils.LogDebug(TAG, "unequipping: " + good.ItemId);
			
			UnEquip(good, true);
		}
		
		/// <summary>
		/// UnEquips the given <code>EquippableVG</code>.
		/// </summary>
		/// <param name="good">the <code>EquippableVG</code> to equip.</param>
		/// <param name="notify">true will also post event.</param>
		public static void UnEquip(EquippableVG good, bool notify){
			SoomlaUtils.LogDebug(TAG, "unequipping: " + good.ItemId);
			
			instance._unequip(good, notify);
		}


		/// <summary>
		/// Retrieves the balance of the given virtual item.
		/// </summary>
		/// <returns>The balance of the required virtual item.</returns>
		/// <param name="item">The required virtual item.</param>
		public static int GetBalance(VirtualItem item){
			SoomlaUtils.LogDebug(TAG, "fetching balance for virtual item with itemId: "
			                     + item.ItemId);
			
			return instance._getBalance(item);
		}
		
		/// <summary>
		/// Sets the balance of the given virtual item to be the given balance, and if notify is true
		/// posts the change in the balance to the event bus.
		/// </summary>
		/// <param name="item">the required virtual item.</param>
		/// <param name="balance">the new balance to be set.</param>
		/// <returns>the balance of the required virtual item</returns>
		public static int SetBalance(VirtualItem item, int balance){
			return SetBalance(item, balance, true);
		}
		
		/// <summary>
		/// Same as the other SetBalance but with "notify".
		/// </summary>
		/// <param name="item">the required virtual item.</param>
		/// <param name="balance">the new balance to be set.</param>
		/// <param name="notify">if notify is true post balance change event.</param>
		/// <returns>the balance of the required virtual item</returns>
		public static int SetBalance(VirtualItem item, int balance, bool notify){
			SoomlaUtils.LogDebug(TAG, "setting balance " + balance + " to " + item.ItemId + ".");
			
			return instance._setBalance(item, balance, notify);
		}
		
		/// <summary>
		/// Adds the given amount of items to the storage.
		/// </summary>
		/// <param name="item">the required virtual item.</param>
		/// <param name="amount">the amount of items to add.</param>
		public static int Add(VirtualItem item, int amount){
			return Add(item, amount, true);
		}
		
		/// <summary>
		/// Adds the given amount of items to the storage, and if notify is true
		/// posts the change in the balance to the event bus.
		/// </summary>
		/// <param name="item">the required virtual item.</param>
		/// <param name="amount">the amount of items to add.</param>
		/// <param name="notify">notify if true posts balance change event.</param>
		public static int Add(VirtualItem item, int amount, bool notify){
			SoomlaUtils.LogDebug(TAG, "adding " + amount + " " + item.ItemId);

			return instance._add(item, amount, notify);
		}
		
		/// <summary>
		/// Removes the given amount from the given virtual item's balance.
		/// </summary>
		/// <param name="item">the virtual item to remove the given amount from.</param>
		/// <param name="amount">the amount to remove.</param>
		public static int Remove(VirtualItem item, int amount){
			return Remove(item, amount, true);
		}
		
		/// <summary>
		/// Removes the given amount from the given virtual item's balance.
		/// </summary>
		/// <param name="item">the virtual item to remove the given amount from.</param>
		/// <param name="amount">the amount to remove.</param>
		/// <param name="notify">notify is true post balance change event</para>
		public static int Remove(VirtualItem item, int amount, bool notify){
			SoomlaUtils.LogDebug(TAG, "Removing " + amount + " " + item.ItemId + ".");
			
			return instance._remove(item, amount, true);
		}




		/** Unity-Editor Functions **/

		protected virtual void _removeUpgrades(VirtualGood good, bool notify) {
#if UNITY_EDITOR
			string itemId = good.ItemId;
			string key = keyGoodUpgrade(itemId);

			PlayerPrefs.DeleteKey(key);
			
			if (notify) {
				JSONObject eventJSON = new JSONObject();
				eventJSON.AddField("itemId", good.ItemId);
				StoreEvents.Instance.onGoodUpgrade(eventJSON.print());
			}
#endif
		}

		protected virtual void _assignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG, bool notify) {
#if UNITY_EDITOR
			UpgradeVG upgrade = GetCurrentUpgrade(good);
			if (upgrade != null && upgrade.ItemId == upgradeVG.ItemId) {
				return;
			}
			
			string itemId = good.ItemId;
			string key = keyGoodUpgrade(itemId);
			string upItemId = upgradeVG.ItemId;

			PlayerPrefs.SetString(key, upItemId);
			
			if (notify) {
				var eventJSON = new JSONObject();
				eventJSON.AddField("itemId", good.ItemId);
				eventJSON.AddField("upgradeItemId", upgradeVG.ItemId);
				StoreEvents.Instance.onGoodUpgrade(eventJSON.print());
			}
#endif
		}

		protected virtual UpgradeVG _getCurrentUpgrade(VirtualGood good) {
#if UNITY_EDITOR
			string itemId = good.ItemId;
			string key = keyGoodUpgrade(itemId);
			
			string upItemId = PlayerPrefs.GetString(key);
			
			if (string.IsNullOrEmpty(upItemId)) {
				SoomlaUtils.LogDebug(TAG, "You tried to fetch the current upgrade of " + good.ItemId
				                     + " but there's no upgrade for it.");
				return null;
			}
			
			try {
				return (UpgradeVG) StoreInfo.GetItemByItemId(upItemId);
			} catch (VirtualItemNotFoundException) {
				SoomlaUtils.LogError(TAG,
				                     "The current upgrade's itemId from the DB is not found in StoreInfo.");
			} catch (InvalidCastException) {
				SoomlaUtils.LogError(TAG,
				                     "The current upgrade's itemId from the DB is not an UpgradeVG.");
			}
			return null;
#else			
			return null;
#endif
		}

		protected virtual bool _isEquipped(EquippableVG good){
#if UNITY_EDITOR
			string itemId = good.ItemId;
			string key = keyGoodEquipped(itemId);
			string val = PlayerPrefs.GetString(key);
			
			return !string.IsNullOrEmpty(val);
#else			
			return false;
#endif
		}

		protected virtual void _equip(EquippableVG good, bool notify) {
#if UNITY_EDITOR
			if (IsEquipped(good)) {
				return;
			}
			equipPriv(good, true, notify);
#endif
		}

		protected virtual void _unequip(EquippableVG good, bool notify) {
#if UNITY_EDITOR
			if (!IsEquipped(good)) {
				return;
			}
			equipPriv(good, false, notify);
#endif
		}




#if UNITY_EDITOR
		private void equipPriv(EquippableVG good, bool equip, bool notify){
			string itemId = good.ItemId;
			string key = keyGoodEquipped(itemId);
			
			if (equip) {
				PlayerPrefs.SetString(key, "yes");
				if (notify) {
					var eventJSON = new JSONObject();
					eventJSON.AddField("itemId", good.ItemId);
					StoreEvents.Instance.onGoodEquipped(eventJSON.print());
				}
			} else {
				PlayerPrefs.DeleteKey(key);
				if (notify) {
					var eventJSON = new JSONObject();
					eventJSON.AddField("itemId", good.ItemId);
					StoreEvents.Instance.onGoodUnequipped(eventJSON.print());
				}
			}
		}
#endif


		/** Keys (protected helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		protected override string keyBalance(String itemId) {
			return keyGoodBalance(itemId);
		}

		private static string keyGoodBalance(String itemId) {
			return "good." + itemId + ".balance";
		}

		private static string keyGoodEquipped(String itemId) {
			return "good." + itemId + ".equipped";
		}
		
		private static string keyGoodUpgrade(String itemId) {
			return "good." + itemId + ".currentUpgrade";
		}

		protected override void postBalanceChangeEvent(VirtualItem item, int balance, int amountAdded) {
			JSONObject eventJSON = new JSONObject();
			eventJSON.AddField("itemId", item.ItemId);
			eventJSON.AddField("balance", balance);
			eventJSON.AddField("amountAdded", amountAdded);
			StoreEvents.Instance.onGoodBalanceChanged(eventJSON.print());
		}
#endif
	}
}


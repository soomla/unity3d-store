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
	/// This class is an abstract definition of a Virtual Currency Storage.
	/// </summary>
	public class VirtualCurrencyStorage : VirtualItemStorage
	{

		/// <summary>
		/// Holds an instance of <c>VirtualCurrencyStorage</c> or <c>VirtualCurrencyStorageAndroid</c> or <c>VirtualCurrencyStorageIOS</c>.
		/// </summary>
		static VirtualCurrencyStorage _instance = null;
	
		/// <summary>
		/// Determines which <c>VirtualCurrencyStorage</c> to use according to the platform in use
		/// and if the Unity Editor is being used. 
		/// </summary>
		/// <value>The instance to use.</value>
		static VirtualCurrencyStorage instance {
			get {
				if(_instance == null) {
					#if UNITY_ANDROID && !UNITY_EDITOR
					_instance = new VirtualCurrencyStorageAndroid();
					#elif UNITY_IOS && !UNITY_EDITOR
					_instance = new VirtualCurrencyStorageIOS();
                    #elif UNITY_WP8 && !UNITY_EDITOR
					_instance = new VirtualCurrencyStorageWP();
                    #else
                                          _instance = new VirtualCurrencyStorage();
					#endif
				}
				return _instance;
			}
		}

		protected VirtualCurrencyStorage() {
			TAG = "SOOMLA VirtualCurrencyStorage";
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



		/** Keys (protected helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		protected override string keyBalance(String itemId) {
			return keyCurrencyBalance(itemId);
		}

		private static string keyCurrencyBalance(String itemId) {
			return "currency." + itemId + ".balance";
		}

		protected override void postBalanceChangeEvent(VirtualItem item, int balance, int amountAdded) {
			JSONObject eventJSON = new JSONObject();
			eventJSON.AddField("itemId", item.ItemId);
			eventJSON.AddField("balance", balance);
			eventJSON.AddField("amountAdded", amountAdded);
			StoreEvents.Instance.onCurrencyBalanceChanged(eventJSON.print());
		}
#endif
	}
}


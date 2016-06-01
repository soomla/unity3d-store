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
using System.Runtime.InteropServices;
using System;

namespace Soomla.Store {

	/// <summary>
	/// This is the parent class of all virtual items in the application.
	/// Almost every entity in your virtual economy will be a virtual item. There are many types
	/// of virtual items, each one will extend this class. Each one of the various types extends
	/// <c>VirtualItem</c> and adds its own behavior to it.
	/// </summary>
	public abstract class VirtualItem : SoomlaEntity<VirtualItem> {

		private const string TAG = "SOOMLA VirtualItem";

		/// <summary>
		/// This is the itemId associated with the <c>VirtualItem</c>.
		/// The itemId is a unique id that every item in the SOOMLA economy have.
		/// </summary>
		public string ItemId {
			get { return this._id; }
			set { this._id = value; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		protected VirtualItem (string name, string description, string itemId)
			: base(itemId, name, description)
		{
		}

#if UNITY_WP8 && !UNITY_EDITOR
		protected VirtualItem(SoomlaWpCore.SoomlaEntity<SoomlaWpStore.domain.VirtualItem> wpVirtualItem)
            : base(wpVirtualItem.GetId(),wpVirtualItem.GetName(),wpVirtualItem.GetDescription())
            {
		}
#endif
        /// <summary>
		/// Constructor.
		/// Generates an instance of <c>VirtualItem</c> from the given <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonItem">A JSONObject representation of the wanted <c>VirtualItem</c>.</param>
		protected VirtualItem(JSONObject jsonItem)
			: base(jsonItem)
		{
		}

		public override bool Equals(object obj)	{
			return (obj != null) &&
				(obj.GetType() == this.GetType()) &&
					(((VirtualItem)obj).ItemId == ItemId);
		}

		public override int GetHashCode () {
			return base.GetHashCode ();
		}

        /// <summary>
		/// Gives your user the given amount of the specific virtual item.
		/// For example, when your users play your game for the first time you GIVE them 1000 gems.
		///
		/// NOTE: This action is different than <code>PurchasableVirtualItem</code>'s <code>buy()</code>:
     	/// You use <code>give(int amount)</code> to give your user something for free.
        /// You use <code>buy()</code> to give your user something and get something in return.
		/// 
		/// </summary>
		/// <param name="amount">amount the amount of the specific item to be given</param>
		public int Give(int amount) {
			return Give(amount, true);
		}

		/// <summary>
		/// Works like "give" but receives an argument, notify, to indicate
		/// </summary>
		/// <param name="amount">amount the amount of the specific item to be given.</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public abstract int Give(int amount, bool notify);

		/// <summary>
		/// Takes from your user the given amount of the specific virtual item.
		/// For example, when your user requests a refund, you need to TAKE the item he/she is returning.
		/// </summary>
		/// <param name="amount">The amount of the specific item to be taken.</param>
		public int Take(int amount) {
			return Take(amount, true);
		}

		/// <summary>
		/// Works like "take" but receives an argument, notify, to indicate
		/// if there has been a change in the balance of the current virtual item.
		/// </summary>
		/// <param name="amount">the amount of the specific item to be taken.</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public abstract int Take(int amount, bool notify);

		/// <summary>
		/// Resets this <code>VirtualItem</code>'s balance to the given balance.
		/// </summary>
		/// <returns>The balance of the current virtual item.</returns>
		/// <param name="balance">Balance.</param>
		public int ResetBalance(int balance) {
			return ResetBalance(balance, true);
		}

		/// <summary>
		/// Works like "resetBalance" but receives an argument, notify, to indicate
		/// if there has been a change in the balance of the current virtual item.
		/// </summary>
		/// <returns>The balance after the reset process.</returns>
		/// <param name="balance">The balance of the current virtual item.</param>
		/// <param name="notify">Notify of change in user's balance of current virtual item.</param>
		public abstract int ResetBalance(int balance, bool notify);

		/// <summary>
		/// Will fetch the balance for the current VirtualItem according to its type.
		/// </summary>
		/// <returns>The balance.</returns>
		public abstract int GetBalance();

		/// <summary>
		/// Save this instance with changes that were made to it.
		/// The saving is done in the metadata in StoreInfo and being persisted to the local DB
		/// (if requested).
		/// </summary>
		/// <param name="saveToDB">Should the changes be persisted to local DB</param>
		public void Save(bool saveToDB = true) {
			StoreInfo.Save(this, saveToDB);
		}
	}
}

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
using System.Collections;


namespace Soomla.Store {	
	
	/// <summary>
	/// A <c>LifetimeVG</c> is a virtual good that is bought once and kept forever.
	/// The <c>LifetimeVG</c>'s characteristics are:
	/// 1. Can only be purchased once. 
	/// 2. Your users cannot have more than one of this item.
	/// 
	/// Real Games Examples: 'No Ads', 'Double Coins'
	/// 
	/// This <c>VirtualItem</c> is purchasable.
	/// In case you want this item to be available for purchase in the market (<c>PurchaseWithMarket</c>),
	/// you will need to define the item in the market (App Store, Google Play...).
	/// 
	/// Inheritance: LifetimeVG >
	/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class LifetimeVG : VirtualGood {
		private static string TAG = "SOOMLA LifetimeVG";
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type.</param>
		public LifetimeVG(string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
		}

#if UNITY_WP8 && !UNITY_EDITOR
		public LifetimeVG(SoomlaWpStore.domain.virtualGoods.LifetimeVG wpLifetimeVG)
            : base(wpLifetimeVG)
		{
		}
#endif
        /// <summary>
		/// see parent.
		/// </summary>
		public LifetimeVG(JSONObject jsonVg)
			: base(jsonVg)
		{
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public override JSONObject toJSONObject() {
			return base.toJSONObject();
		}
	

		/// <summary>
		/// Gives your user exactly one <code>LifetimeVG</code>.
		/// </summary>
		/// <param name="amount">he amount of the specific item to be given - if this input is greater than 1,
		///               we force the amount to equal 1, because a <code>LifetimeVG</code> can only be given once..</param>
		/// <param name="notify">Notify.</param>
		public override int Give(int amount, bool notify) {
			if(amount > 1) {
				SoomlaUtils.LogDebug(TAG, "You tried to give more than one LifetimeVG."
				                     + "Will try to give one anyway.");
				amount = 1;
			}
			
			int balance = VirtualGoodsStorage.GetBalance(this);
			
			if (balance < 1) {
				return VirtualGoodsStorage.Add(this, amount, notify);
			}
			return 1;
		}

		/// <summary>
		/// Takes from your user exactly one <code>LifetimeVG</code>.
		/// </summary>
		/// <param name="amount">the amount of the specific item to be taken - if this input is greater than 1,
		///               we force amount to equal 1, because a <code>LifetimeVG</code> can only be
		///               given once and therefore, taken once.</param>
		/// <param name="notify">Notify.</param>
		public override int Take(int amount, bool notify) {
			if (amount > 1) {
				amount = 1;
			}
			
			int balance = VirtualGoodsStorage.GetBalance(this);
			
			if (balance > 0) {
				return VirtualGoodsStorage.Remove(this, amount, notify);
			}
			return 0;
		}

		/// <summary>
		/// etermines if the user is in a state that allows him/her to buy a <code>LifetimeVG</code>,
		/// by checking his/her balance of <code>LifetimeVG</code>s.
		/// From the definition of a <code>LifetimeVG</code>:
		/// If the user has a balance of 0 - he/she can buy.
		/// If the user has a balance of 1 or more - he/she cannot buy more.
		/// </summary>
		/// <returns>true if buying is allowed, false otherwise.</returns>
		protected override bool canBuy() {
			int balance = VirtualGoodsStorage.GetBalance(this);
			
			return balance < 1;
		}
	}
}

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
	/// Single use virtual goods are the most common type of <c>VirtualGood</c>.
	/// 
	/// The <c>SingleUseVG</c>'s characteristics are:
	/// 1. Can be purchased an unlimited number of times.
	/// 2. Has a balance that is saved in the database. Its balance goes up when you "give" it or
	///   "buy" it. The balance goes down when you "take" or (unfriendly) "refund" it.
	/// 
	/// Real Game Examples: 'Hat', 'Sword', 'Muffin'
	/// 
	/// NOTE: In case you want this item to be available for purchase with real money
	/// you will need to define the item in the market (App Store, Google Play...).
	/// 
	/// Inheritance: SingleUseVG >
	/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class SingleUseVG : VirtualGood {
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item identifier.</param>
		/// <param name="purchaseType">Purchase type.</param>
		public SingleUseVG(string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
		}

#if UNITY_WP8 && !UNITY_EDITOR
		public SingleUseVG(SoomlaWpStore.domain.virtualGoods.SingleUseVG suVG)
            : base(suVG)
		{
		}
#endif
        /// <summary>
		/// see parent.
		/// </summary>
		public SingleUseVG(JSONObject jsonVg)
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
		/// Will give a curtain amount of this single use virtual good.
		/// </summary>
		/// <param name="amount">amount the amount of the specific item to be given.</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public override int Give(int amount, bool notify) {
			return VirtualGoodsStorage.Add(this, amount, notify);
		}

		/// <summary>
		/// Will take a curtain amount of this single use virtual good.
		/// </summary>
		/// <param name="amount">the amount of the specific item to be taken.</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public override int Take(int amount, bool notify) {
			return VirtualGoodsStorage.Remove(this, amount, notify);
		}

		protected override bool canBuy() {
			return true;
		}
	}
}

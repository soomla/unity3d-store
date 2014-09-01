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
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public LifetimeVG(AndroidJavaObject jniLifetimeVG) 
			: base(jniLifetimeVG)
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
		/// Saves this instance.
		/// </summary>
		public override void save() 
		{
			save("LifetimeVG");
		}
	}
}

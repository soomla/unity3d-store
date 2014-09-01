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

using System;
using UnityEngine;

namespace Soomla.Store {

	/// <summary>
	/// A representation of a non-consumable item in the Market. These kinds of items are bought by the
	/// user once and kept for him/her forever.
	/// 
	/// Don't be confused: this is not a Lifetime <c>LifetimeVG</c>, it's a MANAGED item in
	/// the Market. This means that the product can be purchased only once per user (such as a new level
	/// in a game), and is remembered by the Market (can be restored if this application is uninstalled
	/// and then re-installed).
	/// 
	/// If you want to make this item available for purchase in the market (purchase with
	/// real money $$), you will need to define it in the desired market(s).
	/// (https://play.google.com/apps/publish), (https://itunesconnect.apple.com) ...
	/// 
	/// Inheritance: NonConsumableItem >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class NonConsumableItem : PurchasableVirtualItem {		

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type.</param>
		public NonConsumableItem (string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public NonConsumableItem(AndroidJavaObject jniNonConsumableItem) 
			: base(jniNonConsumableItem)
		{
		}
#endif
		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>NonConsumableItem</c> from a <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonNon">JSON object</param>
		public NonConsumableItem(JSONObject jsonNon)
			: base(jsonNon)
		{
		}

		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>see parent.</returns>
		public override JSONObject toJSONObject() {
			return base.toJSONObject();
		}

		public override void save() 
		{
			save("NonConsumableItem");
		}
	}
}


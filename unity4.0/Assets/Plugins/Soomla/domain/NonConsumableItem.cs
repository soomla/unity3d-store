/*
 * Copyright (C) 2012 Soomla Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using UnityEngine;

namespace com.soomla.unity
{
	/// <summary>
	/// A representation of a non-consumable item in Google Play. These kinds of items are bought by the user once and kept for him forever.
	/// 
	/// Don't get confused... this is not a Lifetime VirtualGood. It's just a MANAGED item in Google Play.
 	/// This item will be retrieved when you "restoreTransactions"
 	/// 
	/// This VirtualItem is purchasable.
 	/// In case you purchase this item in Google Play or the App Store(PurchaseWithMarket), You need to define the item in Google
 	/// Play Developer Console or in iTunesConnect. (https://play.google.com/apps/publish) (https://itunesconnect.apple.com)
	/// </summary>
	public class NonConsumableItem : PurchasableVirtualItem
	{		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.NonConsumableItem"/> class.
		/// </summary>
		/// <param name='name'>
		/// see parent
		/// </param>
		/// <param name='description'>
		/// see parent
		/// </param>
		/// <param name='itemId'>
		/// see parent
		/// </param>
		/// <param name='purchaseType'>
		/// see parent
		/// </param>
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
		public NonConsumableItem(JSONObject jsonNon)
			: base(jsonNon)
		{
		}
		
		public override JSONObject toJSONObject() {
			return base.toJSONObject();
		}

	}
}


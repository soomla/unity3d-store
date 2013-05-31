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
using UnityEngine;
using System.Collections;


namespace com.soomla.unity{	
	
	

	/// <summary>
	/// SingleUse virtual goods are the most common type of VirtualGood.
	///
	/// The SingleUseVG's characteristics are:
	///  1. Can be purchased unlimited number of times.
	///  2. Has a balance and saved in the database. Its balance goes up when you "give" it or "buy" it. The balance goes
	///      down when it's taken or refunded (in case of an unfriendly refund).
	///
	/// - Usage Examples: 'Hat', 'Sword'
	///
	/// This VirtualItem is purchasable.
 	/// In case you purchase this item in Google Play or the App Store(PurchaseWithMarket), You need to define the item in Google
 	/// Play Developer Console or in iTunesConnect. (https://play.google.com/apps/publish) (https://itunesconnect.apple.com)
	/// </summary>
	public class SingleUseVG : VirtualGood{
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.SingleUseVG"/> class.
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
		public SingleUseVG(string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public SingleUseVG(AndroidJavaObject jniSingleUseVG) 
			: base(jniSingleUseVG)
		{
		}
#endif
		/// <summary>
		/// see parent
		/// </summary>
		public SingleUseVG(JSONObject jsonVg)
			: base(jsonVg)
		{
		}

		/// <summary>
		/// see parent
		/// </summary>
		public override JSONObject toJSONObject() {
			return base.toJSONObject();
		}

	}
}

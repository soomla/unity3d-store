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
	/// SingleUsePacks are just bundles of SingleUse virtual goods.
	/// This kind of virtual good can be used to let your users buy more than one SingleUseVG at once.
	///
	/// The SingleUsePackVG's characteristics are:
	///  1. Can be purchased unlimited number of times.
	///  2. Doesn't Have a balance in the database. The SingleUseVG there's associated with this pack has its own balance. When
	///      your users buy a SingleUsePackVG, the balance of the associated SingleUseVG goes up in the amount that this pack
	///      represents (mGoodAmount).
	///
	///  - Usage Examples: 'Box Of Chocolates', '10 Swords'
	///
	/// This VirtualItem is purchasable.
 	/// In case you purchase this item in Google Play or the App Store(PurchaseWithMarket), You need to define the item in Google
 	/// Play Developer Console or in iTunesConnect. (https://play.google.com/apps/publish) (https://itunesconnect.apple.com)
	/// </summary>
	public class SingleUsePackVG : VirtualGood{
		
//		private static string TAG = "SOOMLA SingleUsePackVG";
		public string GoodItemId;
		public int GoodAmount;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.SingleUsePackVG"/> class.
		/// </summary>
		/// <param name='goodItemId'>
		/// The itemId of the SingleUseVG associated with this pack.
		/// </param>
		/// <param name='amount'>
		/// The number of SingleUseVGs in the pack.
		/// </param>
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
		public SingleUsePackVG(string goodItemId, int amount, string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
			this.GoodItemId = goodItemId;
			this.GoodAmount = amount;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public SingleUsePackVG(AndroidJavaObject jniSingleUsePackVG) 
			: base(jniSingleUsePackVG)
		{
			GoodItemId = jniSingleUsePackVG.Call<string>("getGoodItemId");
			GoodAmount = jniSingleUsePackVG.Call<int>("getGoodAmount");
		}
#endif
		/// <summary>
		/// see parent
		/// </summary>
		public SingleUsePackVG(JSONObject jsonItem)
			: base(jsonItem)
		{
			GoodItemId = jsonItem[JSONConsts.VGP_GOOD_ITEMID].str;
	        this.GoodAmount = System.Convert.ToInt32(((JSONObject)jsonItem[JSONConsts.VGP_GOOD_AMOUNT]).n);
		}

		/// <summary>
		/// see parent
		/// </summary>
		public override JSONObject toJSONObject() 
		{
			JSONObject jsonObject = base.toJSONObject();
	        jsonObject.AddField(JSONConsts.VGP_GOOD_ITEMID, GoodItemId);
	        jsonObject.AddField(JSONConsts.VGP_GOOD_AMOUNT, GoodAmount);
	
	        return jsonObject;
		}

	}
}

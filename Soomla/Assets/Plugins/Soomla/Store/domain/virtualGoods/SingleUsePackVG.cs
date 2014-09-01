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
	/// A <c>SingleUsePackVG</c> is just a bundle of <c>SingleUseVG</c>s.
	/// This kind of virtual good can be used to let your users buy more than one <c>SingleUseVG</c> at once.
	/// 
	/// The <c>SingleUsePackVG</c>'s characteristics are:
	/// 1. Can be purchased an unlimited number of times.
	/// 2. Doesn't have a balance in the database. The <c>SingleUseVG</c> that's associated with this pack
	///    has its own balance. When your users buy a <c>SingleUsePackVG</c>, the balance of the associated
	///    <c>SingleUseVG</c> goes up in the amount that this pack represents.
	/// 
	/// Real Game Examples: 'Box Of Chocolates', '10 Swords'
	/// 
	/// NOTE: In case you want this item to be available for purchase in the market (PurchaseWithMarket),
	/// you will need to define the item in the market (Apple App Store, Google Play, Amazon App Store, etc...).
	/// 
	/// Inheritance: SingleUsePackVG >
	/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class SingleUsePackVG : VirtualGood {
		
//		private static string TAG = "SOOMLA SingleUsePackVG";
		public string GoodItemId;
		public int GoodAmount;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="goodItemId">The itemId of the <c>SingleUseVG</c> associated with this pack.</param>
		/// <param name="amount">The number of <c>SingleUseVG</c>S in the pack.</param>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type.</param>
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
		/// see parent.
		/// </summary>
		public SingleUsePackVG(JSONObject jsonItem)
			: base(jsonItem)
		{
			GoodItemId = jsonItem[JSONConsts.VGP_GOOD_ITEMID].str;
	        this.GoodAmount = System.Convert.ToInt32(((JSONObject)jsonItem[JSONConsts.VGP_GOOD_AMOUNT]).n);
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public override JSONObject toJSONObject() 
		{
			JSONObject jsonObject = base.toJSONObject();
	        jsonObject.AddField(JSONConsts.VGP_GOOD_ITEMID, GoodItemId);
	        jsonObject.AddField(JSONConsts.VGP_GOOD_AMOUNT, GoodAmount);
	
	        return jsonObject;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public override void save() 
		{
			save("SingleUsePackVG");
		}
	}
}

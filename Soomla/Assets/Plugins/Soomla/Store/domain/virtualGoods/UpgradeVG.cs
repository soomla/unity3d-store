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
	/// An upgrade virtual good is one VG in a series of VGs that define an upgrade scale of an
	/// associated <c>VirtualGood</c>.
	/// 
	/// This type of virtual good is best explained with an example:
	/// Let's say there's a strength attribute to one of the characters in your game and that strength is
	/// on a scale of 1-5. You want to provide your users with the ability to upgrade that strength.
	/// 
	/// This is what you'll need to create:
	/// 1. <c>SingleUseVG</c> for 'strength'
	/// 2. <c>UpgradeVG</c> for strength 'level 1'
	/// 3. <c>UpgradeVG</c> for strength 'level 2'
	/// 4. <c>UpgradeVG</c> for strength 'level 3'
	/// 5. <c>UpgradeVG</c> for strength 'level 4'
	/// 6. <c>UpgradeVG</c> for strength 'level 5'
	/// 
	/// When the user buys this <c>UpgradeVG</c>, we check and make sure the appropriate conditions
	/// are met and buy it for you (which actually means we upgrade the associated <c>VirtualGood</c>).
	/// 
	/// NOTE: In case you want this item to be available for purchase with real money
	/// you will need to define the item in the market (App Store, Google Play...).
	/// 	
	/// Inheritance: UpgradeVG >
	/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class UpgradeVG : LifetimeVG {
		
//		private static string TAG = "SOOMLA UpgradeVG";
		public string GoodItemId;
		public string NextItemId;
		public string PrevItemId;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="goodItemId">The itemId of the <c>VirtualGood</c> associated with this upgrade.</param>
		/// <param name="nextItemId">The itemId of the <c>UpgradeVG</c> before, or if this is the first 
		/// 						<c>UpgradeVG</c> in the scale then the value is null.</param>
		/// <param name="prevItemId">The itemId of the <c>UpgradeVG</c> after, or if this is the last
		///                 		<c>UpgradeVG</c> in the scale then the value is null.</param>
		/// <param name="name">nName.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type.</param>
		public UpgradeVG(string goodItemId, string nextItemId, string prevItemId, string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
			this.GoodItemId = goodItemId;
			this.PrevItemId = prevItemId;
			this.NextItemId = nextItemId;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public UpgradeVG(AndroidJavaObject jniUpgradeVG) 
			: base(jniUpgradeVG)
		{
			GoodItemId = jniUpgradeVG.Call<string>("getGoodItemId");
			NextItemId = jniUpgradeVG.Call<string>("getNextItemId");
			PrevItemId = jniUpgradeVG.Call<string>("getPrevItemId");
		}
#endif
		/// <summary>
		/// see parent.
		/// </summary>
		public UpgradeVG(JSONObject jsonItem)
			: base(jsonItem)
		{
			GoodItemId = jsonItem[JSONConsts.VGU_GOOD_ITEMID].str;
	        PrevItemId = jsonItem[JSONConsts.VGU_PREV_ITEMID].str;
			NextItemId = jsonItem[JSONConsts.VGU_NEXT_ITEMID].str;
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public override JSONObject toJSONObject() 
		{
	        JSONObject jsonObject = base.toJSONObject();
            jsonObject.AddField(JSONConsts.VGU_GOOD_ITEMID, this.GoodItemId);
            jsonObject.AddField(JSONConsts.VGU_PREV_ITEMID, string.IsNullOrEmpty(this.PrevItemId) ? "" : this.PrevItemId);
			jsonObject.AddField(JSONConsts.VGU_NEXT_ITEMID, string.IsNullOrEmpty(this.NextItemId) ? "" : this.NextItemId);
	
	        return jsonObject;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public override void save() 
		{
			save("UpgradeVG");
		}
	
	}
}

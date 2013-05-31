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

namespace com.soomla.unity
{
	/// <summary>
	/// A representation of an VirtualItem you can actually purchase.
	/// </summary>
	public class PurchasableVirtualItem : VirtualItem
	{
		private const string TAG = "SOOMLA PurchasableVirtualItem";
		public PurchaseType PurchaseType;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.PurchasableVirtualItem"/> class.
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
		/// The way this item is purchased.
		/// </param>
		protected PurchasableVirtualItem (string name, string description, string itemId, PurchaseType purchaseType) :
			base(name, description, itemId)
		{
			this.PurchaseType = purchaseType;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		protected PurchasableVirtualItem(AndroidJavaObject jniVirtualItem) :
			base(jniVirtualItem)
		{
			StoreUtils.LogDebug(TAG, "Trying to create PurchasableVirtualItem with itemId: " + jniVirtualItem.Call<string>("getItemId"));
			using(AndroidJavaObject jniPurchaseType = jniVirtualItem.Call<AndroidJavaObject>("getPurchaseType")) {
				System.IntPtr cls = AndroidJNI.FindClass("com/soomla/store/purchaseTypes/PurchaseWithMarket");
				if (AndroidJNI.IsInstanceOf(jniPurchaseType.GetRawObject(), cls)) {
					using(AndroidJavaObject jniMarketItem = jniPurchaseType.Call<AndroidJavaObject>("getGoogleMarketItem")) {
						PurchaseType = new PurchaseWithMarket(jniMarketItem.Call<string>("getProductId"), jniMarketItem.Call<double>("getPrice"));
					}
				} else {
					cls = AndroidJNI.FindClass("com/soomla/store/purchaseTypes/PurchaseWithVirtualItem");
					if (AndroidJNI.IsInstanceOf(jniPurchaseType.GetRawObject(), cls)) {
						string itemId = jniPurchaseType.Call<string>("getTargetItemId");
			            int amount = jniPurchaseType.Call<int>("getAmount");
						
						PurchaseType = new PurchaseWithVirtualItem(itemId, amount);
					} else {
						StoreUtils.LogError(TAG, "Couldn't determine what type of class is the given purchaseType.");
					}
				} 
			}
		}
#endif
		/// <summary>
		/// see parent
		/// </returns>
		protected PurchasableVirtualItem(JSONObject jsonItem) :
			base(jsonItem)
		{
			JSONObject purchasableObj = (JSONObject)jsonItem[JSONConsts.PURCHASABLE_ITEM];
			string purchaseType = purchasableObj[JSONConsts.PURCHASE_TYPE].str;

	        if (purchaseType == JSONConsts.PURCHASE_TYPE_MARKET) {
	            JSONObject marketItemObj = (JSONObject)purchasableObj[JSONConsts.PURCHASE_MARKET_ITEM];
	
	            PurchaseType = new PurchaseWithMarket(new MarketItem(marketItemObj));
	        } else if (purchaseType == JSONConsts.PURCHASE_TYPE_VI) {
	            string itemId = purchasableObj[JSONConsts.PURCHASE_VI_ITEMID].str;
	            int amount = System.Convert.ToInt32(((JSONObject)purchasableObj[JSONConsts.PURCHASE_VI_AMOUNT]).n);
	
				PurchaseType = new PurchaseWithVirtualItem(itemId, amount);
	        } else {
	            StoreUtils.LogError(TAG, "Couldn't determine what type of class is the given purchaseType.");
	        }
		}
		
		/// <summary>
		/// see parent
		/// </returns>
		public override JSONObject toJSONObject() {
			JSONObject jsonObject = base.toJSONObject();
	        try {
	            JSONObject purchasableObj = new JSONObject(JSONObject.Type.OBJECT);
	
	            if(PurchaseType is PurchaseWithMarket) {
	                purchasableObj.AddField(JSONConsts.PURCHASE_TYPE, JSONConsts.PURCHASE_TYPE_MARKET);
	
	                MarketItem  mi = ((PurchaseWithMarket) PurchaseType).MarketItem;
	                purchasableObj.AddField(JSONConsts.PURCHASE_MARKET_ITEM, mi.toJSONObject());
	            } else if(PurchaseType is PurchaseWithVirtualItem) {
	                purchasableObj.AddField(JSONConsts.PURCHASE_TYPE, JSONConsts.PURCHASE_TYPE_VI);
	
	                purchasableObj.AddField(JSONConsts.PURCHASE_VI_ITEMID, ((PurchaseWithVirtualItem) PurchaseType).ItemId);
	                purchasableObj.AddField(JSONConsts.PURCHASE_VI_AMOUNT, ((PurchaseWithVirtualItem) PurchaseType).Amount);
	            }
	
	            jsonObject.AddField(JSONConsts.PURCHASABLE_ITEM, purchasableObj);
	        } catch (System.Exception e) {
	            StoreUtils.LogError(TAG, "An error occurred while generating JSON object. " + e.Message);
	        }

	        return jsonObject;
		}
	}
}


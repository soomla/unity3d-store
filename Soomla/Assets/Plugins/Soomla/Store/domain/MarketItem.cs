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
	/// This class represents an item in the market.
	/// <c>MarketItem</c> is only used for <c>PurchaseWithMarket</c> purchase type.
	/// </summary>
	public class MarketItem {

		/// <summary
		/// Each product in the catalog can be MANAGED, UNMANAGED, or SUBSCRIPTION.
		/// MANAGED means that the product can be purchased only once per user (such as a new level in
		/// a game). This purchase is remembered by the Market and can be restored if this
		/// application is uninstalled and then re-installed.
		/// UNMANAGED is used for products that can be used up and purchased multiple times (such as
		/// "gold coins"). It is up to the application to keep track of UNMANAGED products for the user.
		/// SUBSCRIPTION is just like MANAGED except that the user gets charged periodically (monthly
		/// or yearly).
		/// </summary>
		public enum Consumable{
			NONCONSUMABLE,
			CONSUMABLE,
			SUBSCRIPTION,
		}
		
		public string ProductId;
		public Consumable consumable;
		public double Price;

		public string MarketPrice;
		public string MarketTitle;
		public string MarketDescription;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="productId">The id of the current item in the market.</param>
		/// <param name="consumable">The type of the current item in the market.</param>
		/// <param name="price">The actual $$ cost of the current item in the market.</param>
		public MarketItem(string productId, Consumable consumable, double price){
			this.ProductId = productId;
			this.consumable = consumable;
			this.Price = price;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public MarketItem(AndroidJavaObject jniMarketItem) {
			ProductId = jniMarketItem.Call<string>("getProductId");
			Price = jniMarketItem.Call<double>("getPrice");
			int managedOrdinal = jniMarketItem.Call<AndroidJavaObject>("getManaged").Call<int>("ordinal");
			switch(managedOrdinal){
				case 0:
					this.consumable = Consumable.NONCONSUMABLE;
					break;
				case 1:
					this.consumable = Consumable.CONSUMABLE;
					break;
				case 2:
					this.consumable = Consumable.SUBSCRIPTION;
					break;
				default:
					this.consumable = Consumable.CONSUMABLE;
					break;
			}

			MarketPrice = jniMarketItem.Call<string>("getMarketPrice");
			MarketTitle = jniMarketItem.Call<string>("getMarketTitle");
			MarketDescription = jniMarketItem.Call<string>("getMarketDescription");
		}
#endif

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>MarketItem</c> from a <c>JSONObject<c>.
		/// </summary>
		/// <param name="jsonObject">A <c>JSONObject</c> representation of the wanted 
		/// <c>MarketItem</c>.</param>
		public MarketItem(JSONObject jsonObject) {
			string keyToLook = "";
#if UNITY_IOS && !UNITY_EDITOR
			keyToLook = JSONConsts.MARKETITEM_IOS_ID;
#elif UNITY_ANDROID && !UNITY_EDITOR
			keyToLook = JSONConsts.MARKETITEM_ANDROID_ID;
#endif
			if (!string.IsNullOrEmpty(keyToLook) && jsonObject.HasField(keyToLook)) {
				ProductId = jsonObject[keyToLook].str;
			} else {
				ProductId = jsonObject[JSONConsts.MARKETITEM_PRODUCT_ID].str;
			}
			Price = jsonObject[JSONConsts.MARKETITEM_PRICE].n;
			int cOrdinal = System.Convert.ToInt32(((JSONObject)jsonObject[JSONConsts.MARKETITEM_CONSUMABLE]).n);
			if (cOrdinal == 0) {
				this.consumable = Consumable.NONCONSUMABLE;
			} else if (cOrdinal == 1){
				this.consumable = Consumable.CONSUMABLE;
			} else {
				this.consumable = Consumable.SUBSCRIPTION;
			}

			if (jsonObject[JSONConsts.MARKETITEM_MARKETPRICE]) {
				this.MarketPrice = jsonObject[JSONConsts.MARKETITEM_MARKETPRICE].str;
			} else {
				this.MarketPrice = "";
			}
			if (jsonObject[JSONConsts.MARKETITEM_MARKETTITLE]) {
				this.MarketTitle = jsonObject[JSONConsts.MARKETITEM_MARKETTITLE].str;
			} else {
				this.MarketTitle = "";
			}
			if (jsonObject[JSONConsts.MARKETITEM_MARKETDESC]) {
				this.MarketDescription = jsonObject[JSONConsts.MARKETITEM_MARKETDESC].str;
			} else {
				this.MarketDescription = "";
			}
		}

		/// <summary>
		/// Converts the current <c>MarketItem</c> to a <c>JSONObject</c>.
		/// </summary>
		/// <returns>A <c>JSONObject</c> representation of the current 
		/// <c>MarketItem</c>.</returns>
		public JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.MARKETITEM_PRODUCT_ID, this.ProductId);
			obj.AddField(JSONConsts.MARKETITEM_CONSUMABLE, (int)(consumable));
			obj.AddField(JSONConsts.MARKETITEM_PRICE, (float)this.Price);

			obj.AddField(JSONConsts.MARKETITEM_MARKETPRICE, this.MarketPrice);
			obj.AddField(JSONConsts.MARKETITEM_MARKETTITLE, this.MarketTitle);
			obj.AddField(JSONConsts.MARKETITEM_MARKETDESC, this.MarketDescription);

			return obj;
		}

	}
}

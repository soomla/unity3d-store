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

/**
 * This class represents an item in Google Play.
 * Every PurchasableVirtualItem with PurchaseType of PurchaseWithMarket has an instance of this class which is a
 * representation of the same currency pack as an item on Google Play.
 */
	/// <summary>
	/// This class represents an item in Google Play or the App Store.
	/// Every PurchasableVirtualItem with PurchaseType of PurchaseWithMarket has an instance of this class which is a
	/// representation of the same currency pack as an item on Google Play or App Store.
	/// </summary>
	public class MarketItem {
		
		public enum Consumable{
			NONCONSUMABLE,
			CONSUMABLE,
			SUBSCRIPTION,
		}
		
		public string ProductId;
		public Consumable consumable;
		public double Price;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.MarketItem"/> class.
		/// </summary>
		/// <param name='productId'>
		/// The Id of the current item in Google Play or App Store.
		/// </param>
		/// <param name='consumable'>
		/// the Consumable type of the current item in Google Play or App Store.
		/// </param>
		/// <param name='price'>
		/// The actual $$ cost of the current item in Google Play or App Store.
		/// </param>
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
		}
#endif
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.MarketItem"/> class.
		/// </summary>
		public MarketItem(JSONObject jsonObject) {
			ProductId = jsonObject[JSONConsts.MARKETITEM_PRODUCT_ID].str;
			Price = jsonObject[JSONConsts.MARKETITEM_PRICE].n;
			int cOrdinal = System.Convert.ToInt32(((JSONObject)jsonObject[JSONConsts.MARKETITEM_CONSUMABLE]).n);
			if (cOrdinal == 0) {
				this.consumable = Consumable.NONCONSUMABLE;
			} else if (cOrdinal == 1){
				this.consumable = Consumable.CONSUMABLE;
			} else {
				this.consumable = Consumable.SUBSCRIPTION;
			}
		}
		
		public JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.MARKETITEM_PRODUCT_ID, this.ProductId);
			obj.AddField(JSONConsts.MARKETITEM_CONSUMABLE, (int)(consumable));
			obj.AddField(JSONConsts.MARKETITEM_PRICE, (float)this.Price);
			return obj;
		}

	}
}

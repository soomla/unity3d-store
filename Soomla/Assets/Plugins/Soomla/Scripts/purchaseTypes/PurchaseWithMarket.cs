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

namespace Soomla
{
	/// <summary>
	/// This type of Purchase is used to let users purchase PurchasableVirtualItems in the platform's mobile market
	/// (App Store, Google Play ...) (with real $$).
	/// </summary>
	public class PurchaseWithMarket : PurchaseType
	{
		public MarketItem MarketItem;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.PurchaseWithMarket"/> class.
		/// </summary>
		/// <param name='productId'>
		/// The productId to purchase in the platform's market.
		/// </param>
		/// <param name='price'>
		/// The price in the platform's market.
		/// </param>
		public PurchaseWithMarket (string productId, double price) :
			base()
		{
			this.MarketItem = new MarketItem(productId, MarketItem.Consumable.CONSUMABLE, price);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.PurchaseWithMarket"/> class.
		/// </summary>
		/// <param name='marketItem'>
		/// The representation of the item in the platform's market.
		/// </param>
		public PurchaseWithMarket (MarketItem marketItem) :
			base()
		{
			this.MarketItem = marketItem;
		}
	}
}


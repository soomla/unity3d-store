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

namespace Soomla.Store
{
	/// <summary>
	/// This type of Purchase is used to let users purchase <c>PurchasableVirtualItem</c>s in the platform's 
	/// market (App Store, Google Play ...) with real money $$$.
	/// 
	/// Real Game Example: Purchase a Sword for $1.99.
	/// </summary>
	public class PurchaseWithMarket : PurchaseType
	{
		public MarketItem MarketItem;
		
		/// <summary>
		/// Constructor.
		/// Constructs a <c>PurchaseWithMarket</c> object by constructing a new <c>MarketItem</c> object
		/// with the given <c>productId</c> and <c>price</c>, and declaring it as UNMANAGED.
		/// </summary>
		/// <param name="productId">Product id as it appears in the Market.</param>
		/// <param name="price">Price in the Market.</param>
		public PurchaseWithMarket (string productId, double price) :
			base()
		{
			this.MarketItem = new MarketItem(productId, MarketItem.Consumable.CONSUMABLE, price);
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="marketItem">Market item.</param>
		public PurchaseWithMarket (MarketItem marketItem) :
			base()
		{
			this.MarketItem = marketItem;
		}

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
		public override void Buy(string itemId)
		{
			StoreEvents.instance.onMarketPurchaseStarted(itemId);
		}
		
		public override void Success(string itemId, string payload) {
			Guid purchaseToken = System.Guid.NewGuid();
			string message = itemId + "#SOOM#" + payload + "#SOOM#" + purchaseToken.ToString();
			StoreEvents.instance.onMarketPurchase(message);
		}
#endif
	}
}


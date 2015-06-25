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
		private const string TAG = "SOOMLA PurchaseWithMarket";

		/// <summary>
		/// The market item associated with the item that needs to be purchased.
		/// </summary>
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
			this.MarketItem = new MarketItem(productId, price);
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

		/// <summary>
		/// Buys the purchasable virtual item.
		/// Implementation in subclasses will be according to specific type of purchase.
		/// </summary>
		/// <param name="payload">a string you want to be assigned to the purchase. This string
		/// is saved in a static variable and will be given bacl to you when the
		///  purchase is completed.</param>
		/// <exception cref="Soomla.Store.InsufficientFundsException">throws InsufficientFundsException</exception>
		public override void Buy(string payload)
		{
			SoomlaUtils.LogDebug(TAG, "Starting in-app purchase for productId: "
			                     + MarketItem.ProductId);

			JSONObject eventJSON = new JSONObject();
			eventJSON.AddField("itemId", AssociatedItem.ItemId);
			StoreEvents.Instance.onItemPurchaseStarted(eventJSON.print(), true);
			SoomlaStore.BuyMarketItem(MarketItem.ProductId, payload);
		}

		/// <summary>
		/// Checks if there is enough funds to afford the <code>PurchasableVirtualItem</code>.
		/// Implementation in subclasses will be according to specific type of purchase.
		/// </summary>
		/// <returns>True if there are enough funds to afford the virtual item with the given item id </returns>
		public override bool CanAfford() {
			// for market purchases, always assume it can be afforded
			return true;
		}
	}
}


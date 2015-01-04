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

namespace Soomla.Store {

	/// <summary>
	/// A representation of a <c>VirtualItem</c> that you can actually purchase.
	/// </summary>
	public abstract class PurchasableVirtualItem : VirtualItem {

		private const string TAG = "SOOMLA PurchasableVirtualItem";

		/// <summary>
		/// When we actually try to purchase this <c>PurchasableVirtualItem</c> this purchase type will be invoked.
		/// </summary>
		public PurchaseType PurchaseType;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type (the way by which this item is purchased).</param>
		protected PurchasableVirtualItem (string name, string description, string itemId, PurchaseType purchaseType) :
			base(name, description, itemId)
		{
			this.PurchaseType = purchaseType;

			if (this.PurchaseType != null) {
				this.PurchaseType.AssociatedItem = this;
			}
		}

		/// <summary>
		/// Checks if there is enough funds to afford the <code>PurchasableVirtualItem</code>.
		/// This action uses the associated <code>PurchaseType</code> to perform the check.
		/// </summary>
		/// <returns>True if there are enough funds to afford the virtual item with the given item id </returns>
		public bool CanAfford() {
			return PurchaseType.CanAfford();
		}
		
		/// <summary>
		/// Buys the <code>PurchasableVirtualItem</code>, after checking if the user is in a state that
		/// allows him/her to buy. This action uses the associated <code>PurchaseType</code> to perform
		/// the purchase.
		/// </summary>
		/// <param name="payload">a string you want to be assigned to the purchase. This string
		/// is saved in a static variable and will be given bacl to you when the
		///   purchase is completed..</param>
		/// <exception cref="Soomla.Store.InsufficientFundsException">InsufficientFundsException if the user does not have enough funds for buying.</exception>
		public void Buy(string payload) {
			if (!canBuy()) return;

			PurchaseType.Buy(payload);
		}

		/// <summary>
		/// Determines if user is in a state that allows him/her to buy a specific <code>VirtualItem</code>.
		/// </summary>
		protected abstract bool canBuy();

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>PurchasableVirtualItem</c> from a <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonItem">JSON object.</param>
		protected PurchasableVirtualItem(JSONObject jsonItem) :
			base(jsonItem)
		{
			JSONObject purchasableObj = (JSONObject)jsonItem[StoreJSONConsts.PURCHASABLE_ITEM];
			string purchaseType = purchasableObj[StoreJSONConsts.PURCHASE_TYPE].str;

	        if (purchaseType == StoreJSONConsts.PURCHASE_TYPE_MARKET) {
	            JSONObject marketItemObj = (JSONObject)purchasableObj[StoreJSONConsts.PURCHASE_MARKET_ITEM];
	
	            PurchaseType = new PurchaseWithMarket(new MarketItem(marketItemObj));
	        } else if (purchaseType == StoreJSONConsts.PURCHASE_TYPE_VI) {
	            string itemId = purchasableObj[StoreJSONConsts.PURCHASE_VI_ITEMID].str;
	            int amount = System.Convert.ToInt32(((JSONObject)purchasableObj[StoreJSONConsts.PURCHASE_VI_AMOUNT]).n);
				PurchaseType = new PurchaseWithVirtualItem(itemId, amount);
	        } else {
	            SoomlaUtils.LogError(TAG, "Couldn't determine what type of class is the given purchaseType.");
	        }

			if (this.PurchaseType != null) {
				this.PurchaseType.AssociatedItem = this;
			}
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>see parent</returns>
		public override JSONObject toJSONObject() {
			JSONObject jsonObject = base.toJSONObject();
	        try {
	            JSONObject purchasableObj = new JSONObject(JSONObject.Type.OBJECT);
	
	            if(PurchaseType is PurchaseWithMarket) {
	                purchasableObj.AddField(StoreJSONConsts.PURCHASE_TYPE, StoreJSONConsts.PURCHASE_TYPE_MARKET);
	
	                MarketItem  mi = ((PurchaseWithMarket) PurchaseType).MarketItem;
	                purchasableObj.AddField(StoreJSONConsts.PURCHASE_MARKET_ITEM, mi.toJSONObject());
	            } else if(PurchaseType is PurchaseWithVirtualItem) {
	                purchasableObj.AddField(StoreJSONConsts.PURCHASE_TYPE, StoreJSONConsts.PURCHASE_TYPE_VI);
	
	                purchasableObj.AddField(StoreJSONConsts.PURCHASE_VI_ITEMID, ((PurchaseWithVirtualItem) PurchaseType).TargetItemId);
	                purchasableObj.AddField(StoreJSONConsts.PURCHASE_VI_AMOUNT, ((PurchaseWithVirtualItem) PurchaseType).Amount);
	            }
	
	            jsonObject.AddField(StoreJSONConsts.PURCHASABLE_ITEM, purchasableObj);
	        } catch (System.Exception e) {
	            SoomlaUtils.LogError(TAG, "An error occurred while generating JSON object. " + e.Message);
	        }

	        return jsonObject;
		}
	}
}


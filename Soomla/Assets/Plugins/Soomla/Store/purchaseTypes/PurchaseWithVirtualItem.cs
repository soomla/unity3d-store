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
	/// This type of purchase allows users to purchase <c>PurchasableVirtualItems</c> with other 
	/// <c>VirtualItem</c>s.
	/// 
	/// Real Game Example: Purchase a 'Sword' in exchange for 100 'Gem's. 'Sword' is the item to be purchased,
	/// 'Gem' is the target item, and 100 is the amount.
	/// </summary>
	public class PurchaseWithVirtualItem : PurchaseType
	{
		public String ItemId;
		public int Amount;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="itemId">The itemId of the <c>VirtualItem</c> that is used to "pay" in order 
		/// 					to make the purchase.</param>
		/// <param name="amount">The number of items (with the given item id) needed in order to make the 
		/// 					purchase.</param>
		public PurchaseWithVirtualItem (String itemId, int amount) :
			base()
		{
			this.ItemId = itemId;
			this.Amount = amount;
		}

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
		public override void Buy(string itemId)
		{
			StoreEvents.instance.onItemPurchaseStarted(itemId);

			if (StoreInventory.GetItemBalance(ItemId) < Amount)
				throw new InsufficientFundsException(itemId);
			else
				StoreInventory.TakeItem(ItemId, Amount);
		}
		
		public override void Success(string itemId, string payload) {
			StoreEvents.instance.onItemPurchased(itemId + "#SOOM#" + payload);
		}
#endif
	}
}


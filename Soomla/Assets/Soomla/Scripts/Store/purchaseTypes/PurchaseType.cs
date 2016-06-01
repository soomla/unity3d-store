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
	/// A <c>PurchaseType</c> is a way to purchase a <c>PurchasableVirtualItem</c>. This
	/// abstract class describes basic features of the actual implementations of <c>PurchaseType<c>.
	/// </summary>
	public abstract class PurchaseType
	{
		/// <summary>
		/// Constructor.
		/// Implementation in subclasses will be according to specific type of purchase.
		/// </summary>
		public PurchaseType ()
		{
		}

		/// <summary>
		/// The item associated with this purchase type. 
		/// This item is the one actually being purchased and the assignment of this variable is from the
		/// <c>PurchasableItem</c>'s constructor.
		/// </summary>
		public PurchasableVirtualItem AssociatedItem;

		/// <summary>
		/// Buys the purchasable virtual item.
		/// Implementation in subclasses will be according to specific type of purchase.
		/// </summary>
		/// <param name="payload">a string you want to be assigned to the purchase. This string
		/// is saved in a static variable and will be given bacl to you when the
		///   purchase is completed.</param>
		/// <exception cref="Soomla.Store.InsufficientFundsException">throws InsufficientFundsException</exception>
		public abstract void Buy(string payload);

		/// <summary>
		/// Checks if there is enough funds to afford the <code>PurchasableVirtualItem</code>.
		/// Implementation in subclasses will be according to specific type of purchase.
		/// </summary>
		/// <returns>True if there are enough funds to afford the virtual item with the given item id </returns>
		public abstract bool CanAfford();

        public abstract string GetPrice();
	}

}


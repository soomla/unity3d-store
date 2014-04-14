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
	/// This type of Purchase allows users to purchase PurchasableVirtualItems with other VirtualItems.
	/// </summary>
	public class PurchaseWithVirtualItem : PurchaseType
	{
		public String ItemId;
		public int Amount;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.PurchaseWithVirtualItem"/> class.
		/// </summary>
		/// <param name='itemId'>
		/// The itemId of the VirtualItem that is used to "pay" in order to make the purchase.
		/// </param>
		/// <param name='amount'>
		/// The number of items to purchase.
		/// </param>
		public PurchaseWithVirtualItem (String itemId, int amount) :
			base()
		{
			this.ItemId = itemId;
			this.Amount = amount;
		}
	}
}


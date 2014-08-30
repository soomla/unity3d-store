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

namespace Soomla.Store {

	/// <summary>
	/// This exception is thrown when a user tries to make a purchase and does not have enough funds.
	/// 
	/// Real Game Example:
	/// Example Inventory: { currency_coin: 100, green_hat: 3, blue_hat: 5 }
	/// Say a blue_hat costs 200 currency_coins.
	/// Suppose that you have a user that wants to buy a blue_hat.
	/// You'll probably call <c>StoreInventory.buy("blue_hat")</c>.
	/// An <c>InsufficientFundsException</c> will be thrown.
	/// You can catch this exception in order to notify the user that he/she 
	/// doesn't have enough coins to buy a blue_hat.
	/// </summary>
	public class InsufficientFundsException : Exception {

		/// <summary>
		/// Constructor.
		/// </summary>
		public InsufficientFundsException()
			:base("You tried to buy something but you don't have enough funds to buy it.")
		{}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="itemId">Id of the item that was attempted to buy with.</param>
		public InsufficientFundsException (string itemId)
			:base("You tried to buy with itemId: " + itemId + " but you don't have enough funds to buy it.")
		{
		}
	}
}


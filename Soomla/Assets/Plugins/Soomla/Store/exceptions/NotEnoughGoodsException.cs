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
	/// This exception is thrown when a user tries to equip a virtual good which he/she does not own.
	/// 
	/// Real Game Example:
	/// Example Inventory: { currency_coin: 100, robot_character: 3 }
	/// Suppose that your user would like to equip (LOCAL) a robot_character.
	/// You'll probably call <c>equipVirtualGood("robot_character")</c>.
	/// <c>NotEnoughGoodException</c> will be thrown with "robot_character" as the itemId.
	/// You can catch this exception in order to notify the user that he/she does not
	/// own a robot_character (so he/she cannot equip it!).
	/// </summary>
	public class NotEnoughGoodsException : Exception {

		/// <summary>
		/// Constructor.
		/// </summary>
		public NotEnoughGoodsException()
			:base("You tried to equip virtual good but you don't have any of it.")
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="itemId">Id of the virtual good that was attempted to be equipped.</param>
		public NotEnoughGoodsException (string itemId)
			:base("You tried to equip virtual good with itemId: " + itemId + " but you don't have any of it.")
		{
		}
		
		
	}
}


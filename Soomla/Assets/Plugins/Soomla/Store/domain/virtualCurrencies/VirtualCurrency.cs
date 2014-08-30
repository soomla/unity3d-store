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
using System.Collections;

namespace Soomla.Store{

	/// <summary>
	/// This is a representation of a game's virtual currency.
	/// Each game can have multiple instances of a virtual currency, all kept in <c>StoreInfo</c>.
	/// 
	/// Real Game Examples: 'Coin', 'Gem', 'Muffin'
	/// 
	/// NOTE: This item is NOT purchasable!
	/// However, a <c>VirtualCurrencyPack</c> IS purchasable.
	/// For example, if the virtual currency in your game is a 'Coin' and you want to make a single 'Coin'
	/// available for purchase you will need to define a <c>VirtualCurrencyPack</c> of 1 'Coin'.
	/// </summary>
	public class VirtualCurrency : VirtualItem{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name of currency.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		public VirtualCurrency(string name, string description, string itemId)
			: base(name, description, itemId)
		{
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public VirtualCurrency(AndroidJavaObject jniVirtualCurrency) 
			: base(jniVirtualCurrency)
		{
		}
#endif

		/// <summary>
		/// Constructor.
		/// Initializes a new instance of VirtualCurrency from the given JSON object.
		/// </summary>
		/// <param name="jsonVc">JSON object.</param>
		public VirtualCurrency(JSONObject jsonVc)
			: base(jsonVc)
		{
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		public override JSONObject toJSONObject() {
			return base.toJSONObject();
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public override void save() 
		{
			save("VirtualCurrency");
		}
	}
}
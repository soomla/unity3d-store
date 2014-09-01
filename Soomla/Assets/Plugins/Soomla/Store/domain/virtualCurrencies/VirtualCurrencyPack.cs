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
	/// Every game has its virtual currencies. This class represents a pack of <c>VirtualCurrency</c>s.
	/// Real Game Example: If the virtual currency in your game is a 'Coin', you will sell packs of
	/// 'Coins' such as a "10 Coin Set" or "Super Saver Pack".
	/// 
	/// NOTE: In case you want this item to be available for purchase with real money you will need to
	/// define the item in the market (App Store, Google Play...).
	/// 
	/// Inheritance: VirtualCurrencyPack >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/> 
	/// </summary>
	public class VirtualCurrencyPack : PurchasableVirtualItem {
//		private static string TAG = "SOOMLA VirtualCurrencyPack";
		
		public int CurrencyAmount;
		public string CurrencyItemId;

		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="name">see parent.</param>
		/// <param name="description">see parent.</param>
		/// <param name="itemId">see parent.</param>
		/// <param name="currencyAmount">The amount of currency in the pack.</param>
		/// <param name="currencyItemId">The item id of the currency associated with this pack.</param>
		/// <param name="purchaseType">see parent.</param>
		public VirtualCurrencyPack(string name, string description, string itemId, int currencyAmount, string currencyItemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
			this.CurrencyAmount = currencyAmount;
			this.CurrencyItemId = currencyItemId;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public VirtualCurrencyPack(AndroidJavaObject jniVirtualCurrencyPack) 
			: base(jniVirtualCurrencyPack)
		{
			this.CurrencyAmount = jniVirtualCurrencyPack.Call<int>("getCurrencyAmount");

			// Virtual Currency
			CurrencyItemId = jniVirtualCurrencyPack.Call<string>("getCurrencyItemId");
		}
#endif

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonItem">see parent</param>
		public VirtualCurrencyPack(JSONObject jsonItem)
			: base(jsonItem)
		{
			this.CurrencyAmount = System.Convert.ToInt32(((JSONObject)jsonItem[JSONConsts.CURRENCYPACK_CURRENCYAMOUNT]).n);
			
			CurrencyItemId = jsonItem[JSONConsts.CURRENCYPACK_CURRENCYITEMID].str;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.CURRENCYPACK_CURRENCYAMOUNT, this.CurrencyAmount);
			obj.AddField(JSONConsts.CURRENCYPACK_CURRENCYITEMID, this.CurrencyItemId);
			return obj;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public override void save() 
		{
			save("VirtualCurrencyPack");
		}

	}
}
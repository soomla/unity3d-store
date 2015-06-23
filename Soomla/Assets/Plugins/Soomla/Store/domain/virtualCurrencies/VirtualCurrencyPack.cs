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
		private static string TAG = "SOOMLA VirtualCurrencyPack";

		/// <summary>
		/// The amount of instances of the associated <c>VirtualCurrency</c>.
		/// </summary>
		public int CurrencyAmount;
		/// <summary>
		/// The itemId of the <c>VirtualCurrency</c> associated with this pack.
		/// </summary>
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

#if UNITY_WP8 && !UNITY_EDITOR
		public VirtualCurrencyPack(SoomlaWpStore.domain.virtualCurrencies.VirtualCurrencyPack wpVirtualCurrencyPack)
            : base(wpVirtualCurrencyPack)
		{
            this.CurrencyAmount = wpVirtualCurrencyPack.getCurrencyAmount();

			// Virtual Currency
            CurrencyItemId = wpVirtualCurrencyPack.getCurrencyItemId();
		}
#endif
        /// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="jsonItem">see parent</param>
		public VirtualCurrencyPack(JSONObject jsonItem)
			: base(jsonItem)
		{
			this.CurrencyAmount = System.Convert.ToInt32(((JSONObject)jsonItem[StoreJSONConsts.CURRENCYPACK_CURRENCYAMOUNT]).n);
			
			CurrencyItemId = jsonItem[StoreJSONConsts.CURRENCYPACK_CURRENCYITEMID].str;
		}
		
		/// <summary>
		/// see parent.
		/// </summary>
		/// <returns>JSON object.</returns>
		public override JSONObject toJSONObject() {
			JSONObject obj = base.toJSONObject();
			obj.AddField(StoreJSONConsts.CURRENCYPACK_CURRENCYAMOUNT, this.CurrencyAmount);
			obj.AddField(StoreJSONConsts.CURRENCYPACK_CURRENCYITEMID, this.CurrencyItemId);
			return obj;
		}

		/// <summary>
		/// Gives a curtain amount of <c>VirtualCurrency</c> according to the given amount and the definition of this pack.
		/// </summary>
		/// <param name="amount">amount the amount of the specific item to be given.</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public override int Give(int amount, bool notify) {
			VirtualCurrency currency = null;
			try {
				currency = (VirtualCurrency) StoreInfo.GetItemByItemId(CurrencyItemId);
			} catch (VirtualItemNotFoundException) {
				SoomlaUtils.LogError(TAG, "VirtualCurrency with itemId: " + CurrencyItemId
				                     + " doesn't exist! Can't give this pack.");
				return 0;
			}
			return VirtualCurrencyStorage.Add(
				currency, CurrencyAmount * amount, notify);
		}

		/// <summary>
		/// Takes a curtain amount of <c>VirtualCurrency</c> according to the given amount and the definition of this pack.
		/// </summary>
		/// <param name="amount">the amount of the specific item to be taken.</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public override int Take(int amount, bool notify) {
			VirtualCurrency currency = null;
			try {
				currency = (VirtualCurrency)StoreInfo.GetItemByItemId(CurrencyItemId);
			} catch (VirtualItemNotFoundException) {
				SoomlaUtils.LogError(TAG, "VirtualCurrency with itemId: " + CurrencyItemId +
				                     " doesn't exist! Can't take this pack.");
				return 0;
			}
			return VirtualCurrencyStorage.Remove(currency, CurrencyAmount * amount, notify);
		}

		/// <summary>
		/// DON't APPLY FOR A PACK
		/// </summary>
		public override int ResetBalance(int balance, bool notify) {
			// Not supported for VirtualCurrencyPacks !
			SoomlaUtils.LogError(TAG, "Someone tried to reset balance of CurrencyPack. "
			                     + "That's not right.");
			return 0;
		}

		/// <summary>
		/// DON'T APPLY FOR A PACK
		/// </summary>
		public override int GetBalance() {
			// Not supported for VirtualCurrencyPacks !
			SoomlaUtils.LogError(TAG, "Someone tried to check balance of CurrencyPack. "
			                     + "That's not right.");
			return 0;
		}

		protected override bool canBuy() {
			return true;
		}

	}
}
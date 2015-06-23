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


namespace Soomla.Store {	
	
	/// <summary>
	/// An upgrade virtual good is one VG in a series of VGs that define an upgrade scale of an
	/// associated <c>VirtualGood</c>.
	/// 
	/// This type of virtual good is best explained with an example:
	/// Let's say there's a strength attribute to one of the characters in your game and that strength is
	/// on a scale of 1-5. You want to provide your users with the ability to upgrade that strength.
	/// 
	/// This is what you'll need to create:
	/// 1. <c>SingleUseVG</c> for 'strength'
	/// 2. <c>UpgradeVG</c> for strength 'level 1'
	/// 3. <c>UpgradeVG</c> for strength 'level 2'
	/// 4. <c>UpgradeVG</c> for strength 'level 3'
	/// 5. <c>UpgradeVG</c> for strength 'level 4'
	/// 6. <c>UpgradeVG</c> for strength 'level 5'
	/// 
	/// When the user buys this <c>UpgradeVG</c>, we check and make sure the appropriate conditions
	/// are met and buy it for you (which actually means we upgrade the associated <c>VirtualGood</c>).
	/// 
	/// NOTE: In case you want this item to be available for purchase with real money
	/// you will need to define the item in the market (App Store, Google Play...).
	/// 	
	/// Inheritance: UpgradeVG >
	/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class UpgradeVG : LifetimeVG {
		private static string TAG = "SOOMLA UpgradeVG";

		/// <summary>
		/// The itemId of the associated <c>VirtualGood</c>.
		/// </summary>
		public string GoodItemId;
		/// <summary>
		/// The itemId of the <c>UpgradeVG</c> that comes after this one (or null this is the last one)
		/// </summary>
		public string NextItemId;
		/// <summary>
		/// The itemId of the <c>UpgradeVG</c> that comes before this one (or null this is the first one)
		/// </summary>
		public string PrevItemId;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="goodItemId">The itemId of the <c>VirtualGood</c> associated with this upgrade.</param>
		/// <param name="nextItemId">The itemId of the <c>UpgradeVG</c> after, or if this is the last 
		/// 						<c>UpgradeVG</c> in the scale then the value is null.</param>
		/// <param name="prevItemId">The itemId of the <c>UpgradeVG</c> before, or if this is the first
		///                 		<c>UpgradeVG</c> in the scale then the value is null.</param>
		/// <param name="name">nName.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type.</param>
		public UpgradeVG(string goodItemId, string nextItemId, string prevItemId, string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
			this.GoodItemId = goodItemId;
			this.PrevItemId = prevItemId;
			this.NextItemId = nextItemId;
		}

#if UNITY_WP8 && !UNITY_EDITOR
		public UpgradeVG(SoomlaWpStore.domain.virtualGoods.UpgradeVG wpUpgradeVG)
            : base(wpUpgradeVG)
		{
            GoodItemId = wpUpgradeVG.getGoodItemId();
            NextItemId = wpUpgradeVG.getNextItemId();
            PrevItemId = wpUpgradeVG.getPrevItemId();
		}
#endif
        /// <summary>
		/// see parent.
		/// </summary>
		public UpgradeVG(JSONObject jsonItem)
			: base(jsonItem)
		{
			GoodItemId = jsonItem[StoreJSONConsts.VGU_GOOD_ITEMID].str;
	        PrevItemId = jsonItem[StoreJSONConsts.VGU_PREV_ITEMID].str;
			NextItemId = jsonItem[StoreJSONConsts.VGU_NEXT_ITEMID].str;
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public override JSONObject toJSONObject() 
		{
	        JSONObject jsonObject = base.toJSONObject();
            jsonObject.AddField(StoreJSONConsts.VGU_GOOD_ITEMID, this.GoodItemId);
            jsonObject.AddField(StoreJSONConsts.VGU_PREV_ITEMID, string.IsNullOrEmpty(this.PrevItemId) ? "" : this.PrevItemId);
			jsonObject.AddField(StoreJSONConsts.VGU_NEXT_ITEMID, string.IsNullOrEmpty(this.NextItemId) ? "" : this.NextItemId);
	
	        return jsonObject;
		}

		/// <summary>
		/// Determines if the user is in a state that allows him/her to buy an <code>UpgradeVG</code>
		///	This method enforces allowing/rejecting of upgrades here so users won't buy them when
     	/// they are not supposed to.
     	/// If you want to give your users free upgrades, use the <code>give</code> function.
		/// </summary>
		/// <returns><c>true</c>, if can buy, <c>false</c> otherwise.</returns>
		protected override bool canBuy() {
			VirtualGood good = null;
			try {
				good = (VirtualGood) StoreInfo.GetItemByItemId(GoodItemId);
			} catch (VirtualItemNotFoundException) {
				SoomlaUtils.LogError(TAG, "VirtualGood with itemId: " + GoodItemId +
				                     " doesn't exist! Returning NO (can't buy).");
				return false;
			}
			
			UpgradeVG upgradeVG = VirtualGoodsStorage.GetCurrentUpgrade(good);

			return ((upgradeVG == null && string.IsNullOrEmpty(PrevItemId)) ||
			        (upgradeVG != null && ((upgradeVG.NextItemId == this.ItemId) ||
			                       (upgradeVG.PrevItemId == this.ItemId))))
				&& base.canBuy();
		}

		/// <summary>
		/// Assigns the current upgrade to the associated <code>VirtualGood</code> (mGood).
		/// </summary>
		/// <param name="amount">NOT USED HERE!</param>
		/// <param name="notify">notify of change in user's balance of current virtual item.</param>
		public override int Give(int amount, bool notify) {
			SoomlaUtils.LogDebug(TAG, "Assigning " + Name + " to: " + GoodItemId);
			
			VirtualGood good = null;
			try {
				good = (VirtualGood) StoreInfo.GetItemByItemId(GoodItemId);
			} catch (VirtualItemNotFoundException) {
				SoomlaUtils.LogError(TAG, "VirtualGood with itemId: " + GoodItemId +
				                     " doesn't exist! Can't upgrade.");
				return 0;
			}
			
			VirtualGoodsStorage.AssignCurrentUpgrade(good, this, notify);
			
			return base.Give(amount, notify);
		}

		/// <summary>
		/// Takes upgrade from the user, or in other words DOWNGRADES the associated
		/// <code>VirtualGood</code> (mGood).
		/// Checks if the current Upgrade is really associated with the <code>VirtualGood</code> and:
		/// </summary>
		/// <param name="amount">NOT USED HERE!.</param>
		/// <param name="notify">see parent.</param>
		public override int Take(int amount, bool notify) {
			VirtualGood good = null;
			
			try {
				good = (VirtualGood) StoreInfo.GetItemByItemId(GoodItemId);
			} catch (VirtualItemNotFoundException) {
				SoomlaUtils.LogError(TAG, "VirtualGood with itemId: " + GoodItemId
				                     + " doesn't exist! Can't downgrade.");
				return 0;
			}
			
			UpgradeVG upgradeVG = VirtualGoodsStorage.GetCurrentUpgrade(good);
			
			// Case: Upgrade is not assigned to this Virtual Good
			if (upgradeVG != this) {
				SoomlaUtils.LogError(TAG, "You can't take an upgrade that's not currently assigned."
				                     + "The UpgradeVG " + Name + " is not assigned to " + "the VirtualGood: "
				                     + good.Name);
				return 0;
			}
			
			if (!string.IsNullOrEmpty(PrevItemId)) {
				UpgradeVG prevUpgradeVG = null;
				// Case: downgrade is not possible because previous upgrade does not exist
				try {
					prevUpgradeVG = (UpgradeVG)StoreInfo.GetItemByItemId(PrevItemId);
				} catch (VirtualItemNotFoundException) {
					SoomlaUtils.LogError(TAG, "Previous UpgradeVG with itemId: " + PrevItemId
					                     + " doesn't exist! Can't downgrade.");
					return 0;
				}
				// Case: downgrade is successful!
				SoomlaUtils.LogDebug(TAG, "Downgrading " + good.Name + " to: "
				                     + prevUpgradeVG.Name);
				VirtualGoodsStorage.AssignCurrentUpgrade(good, prevUpgradeVG, notify);
			}
			
			// Case: first Upgrade in the series - so we downgrade to NO upgrade.
			else {
				SoomlaUtils.LogDebug(TAG, "Downgrading " + good.Name + " to NO-UPGRADE");
				VirtualGoodsStorage.RemoveUpgrades(good, notify);
			}

			return base.Take(amount, notify);
		}
	}
}

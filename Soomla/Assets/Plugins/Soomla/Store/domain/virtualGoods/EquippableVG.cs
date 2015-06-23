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
	/// An Equippable virtual good is a special type of Lifetime virtual good that can be equipped 
	/// by your users. Equipping means that the user decides to currently use a specific virtual good.
	/// 
	/// The <c>EquippableVG</c>'s characteristics are:
	/// 1. Can be purchased only once.
	/// 2. Can be equipped by the user.
	/// 3. Inherits the definition of <c>LifetimeVG</c>.
	/// 
	/// There are 3 ways to equip an <c>EquippableVG</c>:
	/// 1. LOCAL    - The current <c>EquippableVG</c>'s equipping status doesn't affect any other
	/// <c>EquippableVG</c>.
	/// 2. CATEGORY - In the containing category, if this <c>EquippableVG</c> is equipped, all other
	/// <c>EquippableVG</c>s must stay unequipped.
	/// 3. GLOBAL   - In the whole game, if this <c>EquippableVG</c> is equipped, all other
	/// <c>EquippableVG</c>s must stay unequipped.
	/// 
	/// Real Game Examples:
	/// 1. LOCAL: Say your game offers 3 weapons: a sword, a gun, and an axe (<c>LifetimeVG</c>s).
	/// Suppose your user has already bought all 3. These are euippables that do not affect one another
	/// - your user can “carry” the sword, gun, and axe at the same time if he/she chooses to!
	/// 2. CATEGORY: Suppose your game offers “shirts” and “hats”. Say there are 4 available
	/// shirts and 2 available hats, and your user owns all of the clothing items available.
	/// He/she can equip a shirt and a hat at the same time, but cannot equip more than 1 shirt
	/// or more than 1 hat at the same time. In other words, he/she can equip at most one of each 
	/// clothing category (shirts, hats)!
	/// 3. GLOBAL: Suppose your game offers multiple characters (<c>LifetimeVGs</c>): RobotX and
	/// RobotY. Let’s say your user owns both characters. He/she will own them forever (because they are
	/// <c>LifetimeVG</c>s). Your user can only play as (i.e. Equip) one character
	/// at a time, either RobotX or RobotY, but never both at the same time!
	/// 
	/// NOTE: In case you want this item to be available for purchase with real money
	/// you will need to define it in the market (App Store, Google Play...).
	/// 
	/// Inheritance: EquippableVG >
	/// <see cref="com.soomla.store.domain.virtualGoods.LifetimeVG"/> >
	/// <see cref="com.soomla.store.domain.virtualGoods.VirtualGood"/> >
	/// <see cref="com.soomla.store.domain.PurchasableVirtualItem"/> >
	/// <see cref="com.soomla.store.domain.VirtualItem"/>
	/// </summary>
	public class EquippableVG : LifetimeVG {
		private static string TAG = "SOOMLA EquippableVG";

		/// <summary>
		/// Equipping model is described above in the class description.
		/// </summary>
		public sealed class EquippingModel {

    		private readonly string name;
    		private readonly int value;

		    public static readonly EquippingModel LOCAL = new EquippingModel (0, "local");
		    public static readonly EquippingModel CATEGORY = new EquippingModel (1, "category");
		    public static readonly EquippingModel GLOBAL = new EquippingModel (2, "global");        
		
		    private EquippingModel(int value, string name){
		        this.name = name;
		        this.value = value;
		    }
		
		    public override string ToString(){
		        return name;
		    }
			
			public int toInt() {
				return value;
			}
		
		}

		/// <summary>
		/// This is the current <c>EquippableVG</c>'s equipping model.
		/// </summary>
		public EquippingModel Equipping;
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="equippingModel">Equipping model: <c>LOCAL</c>, <c>GLOBAL</c>, or <c>CATEGORY</c>.</param>
		/// <param name="name">Name.</param>
		/// <param name="Description">description.</param>
		/// <param name="itemId">Item id.</param>
		/// <param name="purchaseType">Purchase type.</param>
		public EquippableVG(EquippingModel equippingModel, string name, string description, string itemId, PurchaseType purchaseType)
			: base(name, description, itemId, purchaseType)
		{
			this.Equipping = equippingModel;
		}

#if UNITY_WP8 && !UNITY_EDITOR
		public EquippableVG(SoomlaWpStore.domain.virtualGoods.EquippableVG wpEquippableVG)
            : base(wpEquippableVG)
		{   
			switch(wpEquippableVG.ToString()){
				case "local":
					this.Equipping = EquippingModel.LOCAL;
					break;
				case "category":
					this.Equipping = EquippingModel.CATEGORY;
					break;
				case "global":
					this.Equipping = EquippingModel.GLOBAL;
					break;
				default:
					this.Equipping = EquippingModel.CATEGORY;
					break;
			}
		}
#endif

        /// <summary>
		/// see parent.
		/// </summary>
		public EquippableVG(JSONObject jsonItem)
			: base(jsonItem)
		{
			string equippingStr = jsonItem[StoreJSONConsts.EQUIPPABLE_EQUIPPING].str;
			this.Equipping = EquippingModel.CATEGORY;
			switch(equippingStr){
				case "local":
					this.Equipping = EquippingModel.LOCAL;
					break;
				case "global":
					this.Equipping = EquippingModel.GLOBAL;
					break;
				default:
					this.Equipping = EquippingModel.CATEGORY;
					break;
			}
		}

		/// <summary>
		/// see parent.
		/// </summary>
		public override JSONObject toJSONObject() 
		{
			JSONObject obj = base.toJSONObject();
			obj.AddField(StoreJSONConsts.EQUIPPABLE_EQUIPPING, this.Equipping.ToString());
			
			return obj;
		}

		/// <summary>
		/// Equips the current <code>EquippableVG</code>
		/// </summary>
		/// <exception cref="Soomla.Store.NotEnoughGoodsException">Throws NotEnoughGoodsException</exception>
		public void Equip() {
			Equip(true);
		}

		/// <summary>
		/// Equips the current <code>EquippableVG</code>.
		/// The equipping is done according to the equipping model ('GLOBAL', 'CATEGORY', or 'LOCAL').
		/// </summary>
		/// <exception cref="Soomla.Store.NotEnoughGoodsException">Throws NotEnoughGoodsException</exception>
		/// <param name="notify">if true, the relevant event will be posted when equipped.</param>
		public void Equip(bool notify) {
			// only if the user has bought this EquippableVG, the EquippableVG is equipped.
			if (VirtualGoodsStorage.GetBalance(this) > 0){
				
				if (Equipping == EquippingModel.CATEGORY) {
					VirtualCategory category = null;
					try {
						category = StoreInfo.GetCategoryForVirtualGood(this.ItemId);
					} catch (VirtualItemNotFoundException) {
						SoomlaUtils.LogError(TAG,
						                     "Tried to unequip all other category VirtualGoods but there was no " +
						                     "associated category. virtual good itemId: " + this.ItemId);
						return;
					}
					
					foreach (string goodItemId in category.GoodItemIds) {
						EquippableVG equippableVG = null;
						try {
							equippableVG = (EquippableVG) StoreInfo.GetItemByItemId(goodItemId);
							
							if (equippableVG != null && equippableVG != this) {
								equippableVG.Unequip(notify);
							}
						} catch (VirtualItemNotFoundException) {
							SoomlaUtils.LogError(TAG, "On equip, couldn't find one of the itemIds "
							                     + "in the category. Continuing to the next one. itemId: "
							                     + goodItemId);
						} catch (System.InvalidCastException) {
							SoomlaUtils.LogDebug(TAG, "On equip, an error occurred. It's a debug "
							                     + "message b/c the VirtualGood may just not be an EquippableVG. "
							                     + "itemId: " + goodItemId);
						}
					}
				} else if (Equipping == EquippingModel.GLOBAL) {
					foreach(VirtualGood good in StoreInfo.Goods) {
						if (good != this &&
						    good is EquippableVG) {
							((EquippableVG)good).Unequip(notify);
						}
					}
				}

				VirtualGoodsStorage.Equip(this, notify);
			}
			else {
				throw new NotEnoughGoodsException(ItemId);
			}
		}

		/// <summary>
		/// Unequips the current <code>EquippableVG</code>
		/// </summary>
		public void Unequip() {
			Unequip(true);
		}

		/// <summary>
		/// Unequips the current <code>EquippableVG</code>
		/// </summary>
		/// <param name="notify">if true, the relevant event will be posted when unequipped.</param>
		public void Unequip(bool notify) {
			VirtualGoodsStorage.UnEquip(this, notify);
		}

	}
}

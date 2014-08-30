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
		
		///Equipping model is described above in the class description.
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
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public EquippableVG(AndroidJavaObject jniEquippableVG) 
			: base(jniEquippableVG)
		{
			int emOrdinal = jniEquippableVG.Call<AndroidJavaObject>("getEquippingModel").Call<int>("ordinal");
			switch(emOrdinal){
				case 0:
					this.Equipping = EquippingModel.LOCAL;
					break;
				case 1:
					this.Equipping = EquippingModel.CATEGORY;
					break;
				case 2:
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
			string equippingStr = jsonItem[JSONConsts.EQUIPPABLE_EQUIPPING].str;
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
			obj.AddField(JSONConsts.EQUIPPABLE_EQUIPPING, this.Equipping.ToString());
			
			return obj;
		}

		/// <summary>
		/// Saves this instance.
		/// </summary>
		public override void save() 
		{
			save("EquippableVG");
		}

	}
}

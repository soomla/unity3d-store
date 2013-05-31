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
using UnityEngine;
using System.Collections;


namespace com.soomla.unity{	
	
	/// <summary>
	/// An Equippable virtual good is a special type of Lifetime Virtual good.
	/// In addition to the fact that this virtual good can be purchased once, it can be equipped by your users.
	/// - Equipping means that the user decides to currently use a specific virtual good.
	///
	/// The EquippableVG's characteristics are:
	///  1. Can be purchased only once.
	///  2. Can be equipped by the user.
	///  3. Inherits the definition of LifetimeVG.
	///
	/// There are 3 ways to equip an EquippableVG:
	///  1. LOCAL    - The current EquippableVG's equipping status doesn't affect any other EquippableVG.
	///  2. CATEGORY - In the containing category, if this EquippableVG is equipped, all other EquippableVGs are unequipped.
	///  3. GLOBAL   - In the whole game, if this EquippableVG is equipped, all other EquippableVGs are unequipped.
	///
	/// - Example Usage: different characters (play with a specific character),
	///                  'binoculars' (users might only want to take them at night)
	///
	/// This VirtualItem is purchasable.
 	/// In case you purchase this item in Google Play or the App Store(PurchaseWithMarket), You need to define the item in Google
 	/// Play Developer Console or in iTunesConnect. (https://play.google.com/apps/publish) (https://itunesconnect.apple.com)
	/// </summary>
	public class EquippableVG : VirtualGood{
		
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
		/// Initializes a new instance of the <see cref="com.soomla.unity.EquippableVG"/> class.
		/// </summary>
		/// <param name='equippingModel'>
		/// The way this EquippableVG is equipped.
		/// </param>
		/// <param name='name'>
		/// see parent
		/// </param>
		/// <param name='description'>
		/// see parent
		/// </param>
		/// <param name='itemId'>
		/// see parent
		/// </param>
		/// <param name='purchaseType'>
		/// see parent
		/// </param>
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
		/// see parent
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
		/// see parent
		/// </summary>
		public override JSONObject toJSONObject() 
		{
			JSONObject obj = base.toJSONObject();
			obj.AddField(JSONConsts.EQUIPPABLE_EQUIPPING, this.Equipping.ToString());
			
			return obj;
		}

	}
}

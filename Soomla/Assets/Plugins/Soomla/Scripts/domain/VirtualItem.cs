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

namespace Soomla
{
	/// <summary>
	/// This class is the parent of all virtual items in the application.
	/// </summary>
	public abstract class VirtualItem
	{
		private const string TAG = "SOOMLA VirtualItem";
		
		public string Name;
		public string Description;
		public string ItemId;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.VirtualItem"/> class.
		/// </summary>
		/// <param name='name'> 
		/// The name of the virtual item.
		/// </param>
		/// <param name='description'> 
		/// The description of the virtual item. If you use SOOMLA's storefront, This will show up in the store in the description section.
		/// </param>
		/// <param name='itemId'>
		/// The itemId of the virtual item.
		/// </param>
		protected VirtualItem (string name, string description, string itemId)
		{
			this.Name = name;
			this.Description = description;
			this.ItemId = itemId;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		protected VirtualItem(AndroidJavaObject jniVirtualItem) {
			this.Name = jniVirtualItem.Call<string>("getName");
			this.Description = jniVirtualItem.Call<string>("getDescription");
			this.ItemId = jniVirtualItem.Call<string>("getItemId");
		}
#endif
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.VirtualItem"/> class.
		/// </summary>
		/// <param name='jsonItem'>
		/// A JSONObject representation of the wanted <see cref="com.soomla.unity.VirtualItem"/>.
		/// </param>
		protected VirtualItem(JSONObject jsonItem) {
			this.Name = jsonItem[JSONConsts.ITEM_NAME].str;
			if (jsonItem[JSONConsts.ITEM_DESCRIPTION]) {
				this.Description = jsonItem[JSONConsts.ITEM_DESCRIPTION].str;
			} else {
				this.Description = "";
			}
			this.ItemId = jsonItem[JSONConsts.ITEM_ITEMID].str;
		}
		
		/// <summary>
		/// Converts the current <see cref="com.soomla.unity.VirtualItem"/> to a JSONObject.
		/// </summary>
		/// <returns>
		/// A JSONObject representation of the current <see cref="com.soomla.unity.VirtualItem"/>.
		/// </returns>
		public virtual JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.ITEM_NAME, this.Name);
			obj.AddField(JSONConsts.ITEM_DESCRIPTION, this.Description);
			obj.AddField(JSONConsts.ITEM_ITEMID, this.ItemId);
			
			return obj;
		}
		
		public static VirtualItem factoryItemFromJSONObject(JSONObject jsonItem) {
			string className = jsonItem["className"].str;
			switch(className) {
			case "SingleUseVG":
				return new SingleUseVG((JSONObject)jsonItem[@"item"]);
			case "LifetimeVG":
				return new LifetimeVG((JSONObject)jsonItem[@"item"]);
			case "EquippableVG":
				return new EquippableVG((JSONObject)jsonItem[@"item"]);
			case "SingleUsePackVG":
				return new SingleUsePackVG((JSONObject)jsonItem[@"item"]);
			case "VirtualCurrency":
				return new VirtualCurrency((JSONObject)jsonItem[@"item"]);
			case "VirtualCurrencyPack":
				return new VirtualCurrencyPack((JSONObject)jsonItem[@"item"]);
			case "NonConsumableItem":
				return new NonConsumableItem((JSONObject)jsonItem[@"item"]);
			case "UpgradeVG":
				return new UpgradeVG((JSONObject)jsonItem[@"item"]);
			}
			
			return null;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		private static bool isInstanceOf(AndroidJavaObject jniItem, string classJniStr) {
			System.IntPtr cls = AndroidJNI.FindClass(classJniStr);
			return AndroidJNI.IsInstanceOf(jniItem.GetRawObject(), cls);
		}
		
		public static VirtualItem factoryItemFromJNI(AndroidJavaObject jniItem) {
			StoreUtils.LogDebug(TAG, "Trying to create VirtualItem with itemId: " + jniItem.Call<string>("getItemId"));
			
			if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/SingleUseVG")) {
				return new SingleUseVG(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/EquippableVG")) {
				return new EquippableVG(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/UpgradeVG")) {
				return new UpgradeVG(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/LifetimeVG")) {
				return new LifetimeVG(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualGoods/SingleUsePackVG")) {
				return new SingleUsePackVG(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualCurrencies/VirtualCurrency")) {
				return new VirtualCurrency(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/virtualCurrencies/VirtualCurrencyPack")) {
				return new VirtualCurrencyPack(jniItem);
			} else if (isInstanceOf(jniItem, "com/soomla/store/domain/NonConsumableItem")) {
				return new NonConsumableItem(jniItem);
			} else {
				StoreUtils.LogError(TAG, "Couldn't determine what type of class is the given jniItem.");
			}
			
			return null;
		}
#endif
	}
}


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
using System.Runtime.InteropServices;
using System;

namespace Soomla.Store {

	/// <summary>
	/// This is the parent class of all virtual items in the application.
	/// Almost every entity in your virtual economy will be a virtual item. There are many types
	/// of virtual items, each one will extend this class. Each one of the various types extends
	/// <c>VirtualItem</c> and adds its own behavior to it.
	/// </summary>
	public abstract class VirtualItem : SoomlaEntity<VirtualItem> {

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern int storeAssets_Save(string type, string viJSON);
#endif

		private const string TAG = "SOOMLA VirtualItem";

		public string ItemId {
			get { return this._id; }
			set { this._id = value; }

		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="description">Description.</param>
		/// <param name="itemId">Item id.</param>
		protected VirtualItem (string name, string description, string itemId)
			: base(itemId, name, description)
		{
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		protected VirtualItem(AndroidJavaObject jniVirtualItem) 
			: base(jniVirtualItem)
		{
		}
#endif
		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>VirtualItem</c> from the given <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonItem">A JSONObject representation of the wanted <c>VirtualItem</c>.</param>
		protected VirtualItem(JSONObject jsonItem) 
			: base(jsonItem)
		{
		}

		/// <summary>
		/// Creates relevant virtual item according to given JSON object's className.
		/// </summary>
		/// <returns>The relevant item according to given JSON object's className.</returns>
		/// <param name="jsonItem">Json item.</param>
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
		
		public static VirtualItem factoryItemFromJNI(AndroidJavaObject jniItem) {
			SoomlaUtils.LogDebug(TAG, "Trying to create VirtualItem with itemId: " + jniItem.Call<string>("getItemId"));
			
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
				SoomlaUtils.LogError(TAG, "Couldn't determine what type of class is the given jniItem.");
			}
			
			return null;
		}
#endif

		public abstract void save();

		/// <summary>
		/// Saves this instance according to type.
		/// </summary>
		/// <param name="type">type</param>
		protected void save(string type) 
		{
#if !UNITY_EDITOR
			string viJSON = this.toJSONObject().print();
#if UNITY_IOS
			storeAssets_Save(type, viJSON);
#elif UNITY_ANDROID
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStoreAssets = new AndroidJavaClass("com.soomla.unity.StoreAssets")) {
				jniStoreAssets.CallStatic("save", type, viJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#endif
#endif
		}
	}
}


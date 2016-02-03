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
/// See the License for the specific language governing perworlds and
/// limitations under the License.

using UnityEngine;
using System;

namespace Soomla.Store
{
	/// <summary>
	/// abstract <c>VirtualGoodsStorage</c> for Android.
	/// </summary>
	public class VirtualGoodsStorageAndroid : VirtualGoodsStorage {
#if UNITY_ANDROID && !UNITY_EDITOR

		protected override void _removeUpgrades(VirtualGood good, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					jniVGStorage.Call("removeUpgrades", good.ItemId, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		protected override void _assignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					jniVGStorage.Call("assignCurrentUpgrade", good.ItemId, upgradeVG.ItemId, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		protected override UpgradeVG _getCurrentUpgrade(VirtualGood good) {
			string upgradeVGItemId;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					upgradeVGItemId = jniVGStorage.Call<string>("getCurrentUpgrade", good.ItemId);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);

			if (!string.IsNullOrEmpty(upgradeVGItemId)) {
				return (UpgradeVG) StoreInfo.GetItemByItemId(upgradeVGItemId);
			}

			return null;
		}
		
		protected override bool _isEquipped(EquippableVG good){
			bool equipped;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					equipped = jniVGStorage.Call<bool>("isEquipped", good.ItemId);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			
			return equipped;
		}
		
		protected override void _equip(EquippableVG good, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					jniVGStorage.Call("equip", good.ItemId, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		
		protected override void _unequip(EquippableVG good, bool notify) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					jniVGStorage.Call("unequip", good.ItemId, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}


		protected override int _getBalance(VirtualItem item) {
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					retBalance = jniVGStorage.Call<int>("getBalance", item.ItemId);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
		
		protected override int _setBalance(VirtualItem item, int balance, bool notify) {
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					retBalance = jniVGStorage.Call<int>("setBalance", item.ItemId, balance, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
		
		protected override int _add(VirtualItem item, int amount, bool notify){
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					retBalance = jniVGStorage.Call<int>("add", item.ItemId, amount, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
		
		protected override int _remove(VirtualItem item, int amount, bool notify){
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVGStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualGoodsStorage")) {
					retBalance = jniVGStorage.Call<int>("remove", item.ItemId, amount, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
	
#endif
	}
}


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
	/// abstract <c>VirtualCurrencyStorage</c> for Android.
	/// </summary>
	public class VirtualCurrencyStorageAndroid : VirtualCurrencyStorage {
#if UNITY_ANDROID && !UNITY_EDITOR

		protected override int _getBalance(VirtualItem item) {
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVCStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualCurrencyStorage")) {
					retBalance = jniVCStorage.Call<int>("getBalance", item.ItemId);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
		
		protected override int _setBalance(VirtualItem item, int balance, bool notify) {
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVCStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualCurrencyStorage")) {
					retBalance = jniVCStorage.Call<int>("setBalance", item.ItemId, balance, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
		
		protected override int _add(VirtualItem item, int amount, bool notify){
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVCStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualCurrencyStorage")) {
					retBalance = jniVCStorage.Call<int>("add", item.ItemId, amount, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
		
		protected override int _remove(VirtualItem item, int amount, bool notify){
			int retBalance;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStorageManager = new AndroidJavaClass("com.soomla.store.data.StorageManager")) {
				using(AndroidJavaObject jniVCStorage = jniStorageManager.CallStatic<AndroidJavaObject>("getVirtualCurrencyStorage")) {
					retBalance = jniVCStorage.Call<int>("remove", item.ItemId, amount, notify);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return retBalance;
		}
	
#endif
	}
}


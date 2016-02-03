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
using System;
using System.Runtime.InteropServices;

namespace Soomla.Store {

	public class StoreEventPusherAndroid : StoreEvents.StoreEventPusher {

#if UNITY_ANDROID && !UNITY_EDITOR

		protected override void _pushEventSoomlaStoreInitialized(string message) {
			pushEvent("SoomlaStoreInitialized", message);
		}
		protected override void _pushEventUnexpectedStoreError(string message) {
			pushEvent("UnexpectedStoreError", message);
		}
		protected override void _pushEventCurrencyBalanceChanged(string message) {
			pushEvent("SoomlaStoreInitialized", message);
		}
		protected override void _pushEventGoodBalanceChanged(string message) {
			pushEvent("CurrencyBalanceChanged", message);
		}
		protected override void _pushEventGoodEquipped(string message) {
			pushEvent("GoodEquipped", message);
		}
		protected override void _pushEventGoodUnequipped(string message) {
			pushEvent("GoodUnequipped", message);
		}
		protected override void _pushEventGoodUpgrade(string message) {
			pushEvent("GoodUpgrade", message);
		}
		protected override void _pushEventItemPurchased(string message) {
			pushEvent("ItemPurchased", message);
		}
		protected override void _pushEventItemPurchaseStarted(string message) {
			pushEvent("ItemPurchaseStarted", message);
		}

		private void pushEvent(string name, string message) {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStoreEventsClass = new AndroidJavaClass("com.soomla.unity.StoreEventHandler")) {
				using(AndroidJavaObject jniStoreEvents = jniStoreEventsClass.CallStatic<AndroidJavaObject>("getInstance")) {
					jniStoreEvents.Call("pushEvent" + name, message);
				}
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

#endif
	}
}

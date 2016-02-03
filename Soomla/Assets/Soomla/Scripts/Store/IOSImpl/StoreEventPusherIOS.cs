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

	public class StoreEventPusherIOS : StoreEvents.StoreEventPusher {

#if UNITY_IOS && !UNITY_EDITOR

		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventSoomlaStoreInitialized(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventUnexpectedStoreError(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventCurrencyBalanceChanged(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventGoodBalanceChanged(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventGoodEquipped(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventGoodUnEquipped(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventGoodUpgrade(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventItemPurchased(string message);
		[DllImport ("__Internal")]
		private static extern void eventDispatcher_PushEventItemPurchaseStarted(string message);


		protected override void _pushEventSoomlaStoreInitialized(string message) {
			eventDispatcher_PushEventSoomlaStoreInitialized(message);
		}
		protected override void _pushEventUnexpectedStoreError(string message) {
			eventDispatcher_PushEventUnexpectedStoreError(message);
		}
		protected override void _pushEventCurrencyBalanceChanged(string message) {
			eventDispatcher_PushEventCurrencyBalanceChanged(message);
		}
		protected override void _pushEventGoodBalanceChanged(string message) {
			eventDispatcher_PushEventGoodBalanceChanged(message);
		}
		protected override void _pushEventGoodEquipped(string message) {
			eventDispatcher_PushEventGoodEquipped(message);
		}
		protected override void _pushEventGoodUnequipped(string message) {
			eventDispatcher_PushEventGoodUnEquipped(message);
		}
		protected override void _pushEventGoodUpgrade(string message) {
			eventDispatcher_PushEventGoodUpgrade(message);
		}
		protected override void _pushEventItemPurchased(string message) {
			eventDispatcher_PushEventItemPurchased(message);
		}
		protected override void _pushEventItemPurchaseStarted(string message) {
			eventDispatcher_PushEventItemPurchaseStarted(message);
		}

#endif
	}
}

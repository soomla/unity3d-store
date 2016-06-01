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
#if UNITY_WP8 && !UNITY_EDITOR
using SoomlaWpStore.events;
using SoomlaWpCore.events;
using SoomlaWpCore.util;
#endif

namespace Soomla.Store {

	public class StoreEventPusherWP : StoreEvents.StoreEventPusher {

#if UNITY_WP8 && !UNITY_EDITOR
		protected override void _pushEventSoomlaStoreInitialized(SoomlaStoreInitializedEvent _Event) {
            pushEvent(_Event);
		}
		protected override void _pushEventUnexpectedStoreError(UnexpectedStoreErrorEvent _Event) {
            pushEvent(_Event);
		}
        protected override void _pushEventCurrencyBalanceChanged(CurrencyBalanceChangedEvent _Event)
        {
            pushEvent(_Event);
		}
        protected override void _pushEventGoodBalanceChanged(GoodBalanceChangedEvent _Event)
        {
            pushEvent(_Event);
		}
        protected override void _pushEventGoodEquipped(GoodEquippedEvent _Event)
        {
            pushEvent(_Event);
		}
        protected override void _pushEventGoodUnequipped(GoodUnEquippedEvent _Event)
        {
            pushEvent(_Event);
		}
        protected override void _pushEventGoodUpgrade(GoodUpgradeEvent _Event)
        {
            pushEvent(_Event);
		}
        protected override void _pushEventItemPurchased(ItemPurchasedEvent _Event)
        {
            pushEvent(_Event);
		}
        protected override void _pushEventItemPurchaseStarted(ItemPurchaseStartedEvent _Event)
        {
            pushEvent(_Event);
		}

        private void pushEvent(SoomlaWpCore.events.SoomlaEvent _Event)
        {
            BusProvider.Instance.Post(_Event);
		}
#endif
	}
}

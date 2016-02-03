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
#if UNITY_WP8 && !UNITY_EDITOR
using SoomlaWpStore;
#endif

namespace Soomla.Store
{
	/// <summary>
	/// abstract <c>VirtualCurrencyStorage</c> for Android.
	/// </summary>
	public class VirtualCurrencyStorageWP : VirtualCurrencyStorage {
#if UNITY_WP8 && !UNITY_EDITOR

		protected override int _getBalance(VirtualItem item) {
			int retBalance;
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualCurrencyStorage().getBalance(SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId));			
			return retBalance;
		}
		
		protected override int _setBalance(VirtualItem item, int balance, bool notify) {
			int retBalance;
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualCurrencyStorage().setBalance(SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId),balance,notify);
			return retBalance;
		}
		
		protected override int _add(VirtualItem item, int amount, bool notify){
			int retBalance;
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualCurrencyStorage().add(SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId), amount, notify);
			return retBalance;
		}
		
		protected override int _remove(VirtualItem item, int amount, bool notify){
			int retBalance;
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualCurrencyStorage().remove(SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId), amount, notify);
			return retBalance;
		}
	
#endif
	}
}


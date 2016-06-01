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
using System.Runtime.InteropServices;

namespace Soomla.Store
{
	/// <summary>
	/// abstract <c>VirtualCurrencyStorage</c> for iOS.
	/// </summary>
	public class VirtualCurrencyStorageIOS : VirtualCurrencyStorage {
#if UNITY_IOS && !UNITY_EDITOR

		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern int vcStorage_GetBalance(string itemId, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int vcStorage_SetBalance(string itemId, int balance, bool notify, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int vcStorage_Add(string itemId, int amount, bool notify, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int vcStorage_Remove(string itemId, int amount, bool notify, out int outBalance);

		protected override int _getBalance(VirtualItem item) {
			int outBalance = 0;
			int err = vcStorage_GetBalance(item.ItemId, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}

		protected override int _setBalance(VirtualItem item, int balance, bool notify) {
			int outBalance = 0;
			int err = vcStorage_SetBalance(item.ItemId, balance, notify, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}

		protected override int _add(VirtualItem item, int amount, bool notify){
			int outBalance = 0;
			int err = vcStorage_Add(item.ItemId, amount, notify, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}

		protected override int _remove(VirtualItem item, int amount, bool notify){
			int outBalance = 0;
			int err = vcStorage_Remove(item.ItemId, amount, notify, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}

#endif
	}
}

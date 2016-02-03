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
	/// This class is an abstract definition of a Virtual Item Storage.
	/// </summary>
	public abstract class VirtualItemStorage
	{

		protected static string TAG = "SOOMLA VirtualItemStorage";


		/** Unity-Editor Functions **/

		protected virtual int _getBalance(VirtualItem item) {
#if UNITY_EDITOR
			string itemId = item.ItemId;
			string key = keyBalance(itemId);
			string val = PlayerPrefs.GetString(key);
			
			int balance = 0;
			if (!string.IsNullOrEmpty(val)) {
				balance = int.Parse(val);
			}
			
			SoomlaUtils.LogDebug(TAG, "the balance for " + item.ItemId + " is " + balance);
			
			return balance;
#else
			return 0;
#endif
		}

		protected virtual int _setBalance(VirtualItem item, int balance, bool notify) {
#if UNITY_EDITOR
			int oldBalance = _getBalance(item);
			if (oldBalance == balance) {
				return balance;
			}
			
			string itemId = item.ItemId;
			
			string balanceStr = "" + balance;
			string key = keyBalance(itemId);

			PlayerPrefs.SetString(key, balanceStr);

			if (notify) {
				postBalanceChangeEvent(item, balance, 0);
			}
			
			return balance;
#else
			return 0;
#endif
		}

		protected virtual int _add(VirtualItem item, int amount, bool notify){
#if UNITY_EDITOR
			string itemId = item.ItemId;
			int balance = _getBalance(item);
			if (balance < 0) { /* in case the user "adds" a negative value */
				balance = 0;
				amount = 0;
			}
			string balanceStr = "" + (balance + amount);
			string key = keyBalance(itemId);

			PlayerPrefs.SetString(key, balanceStr);
			
			if (notify) {
				postBalanceChangeEvent(item, balance+amount, amount);
			}
			
			return balance + amount;
#else
			return 0;
#endif
		}

		protected virtual int _remove(VirtualItem item, int amount, bool notify){
#if UNITY_EDITOR
			string itemId = item.ItemId;
			int balance = _getBalance(item) - amount;
			if (balance < 0) {
				balance = 0;
				amount = 0;
			}
			string balanceStr = "" + balance;
			string key = keyBalance(itemId);
			PlayerPrefs.SetString(key, balanceStr);
			
			if (notify) {
				postBalanceChangeEvent(item, balance, -1*amount);
			}
			
			return balance;
#else
			return 0;
#endif
		}


		/** Keys (protected helper functions if Unity Editor is being used.) **/

#if UNITY_EDITOR
		protected abstract string keyBalance(String itemId);

		protected abstract void postBalanceChangeEvent(VirtualItem item, int balance, int amountAdded);
#endif
	}
}


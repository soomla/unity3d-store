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
	/// abstract <c>VirtualGoodsStorage</c> for Android.
	/// </summary>
	public class VirtualGoodsStorageIOS : VirtualGoodsStorage {
#if UNITY_IOS && !UNITY_EDITOR

		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern int vgStorage_RemoveUpgrades(string itemId, bool notify);
		[DllImport ("__Internal")]
		private static extern int vgStorage_AssignCurrentUpgrade(string itemId, string upgradeItemId, bool notify);
		[DllImport ("__Internal")]
		private static extern int vgStorage_GetCurrentUpgrade(string itemId, out string outItemId);
		[DllImport ("__Internal")]
		private static extern int vgStorage_IsEquipped(string itemId, out bool outResult);
		[DllImport ("__Internal")]
		private static extern int vgStorage_Equip(string itemId, bool notify);
		[DllImport ("__Internal")]
		private static extern int vgStorage_UnEquip(string itemId, bool notify);
		[DllImport ("__Internal")]
		private static extern int vgStorage_GetBalance(string itemId, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int vgStorage_SetBalance(string itemId, int balance, bool notify, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int vgStorage_Add(string itemId, int amount, bool notify, out int outBalance);
		[DllImport ("__Internal")]
		private static extern int vgStorage_Remove(string itemId, int amount, bool notify, out int outBalance);


		protected override void _removeUpgrades(VirtualGood good, bool notify) {
			int err = vgStorage_RemoveUpgrades(good.ItemId, notify);
			IOS_ErrorCodes.CheckAndThrowException(err);
		}
		
		protected override void _assignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG, bool notify) {
			int err = vgStorage_AssignCurrentUpgrade(good.ItemId, upgradeVG.ItemId, notify);
			IOS_ErrorCodes.CheckAndThrowException(err);
		}
		
		protected override UpgradeVG _getCurrentUpgrade(VirtualGood good) {
			string upgradeVGItemId;
			int err = vgStorage_GetCurrentUpgrade(good.ItemId, out upgradeVGItemId);
			IOS_ErrorCodes.CheckAndThrowException(err);

			if (!string.IsNullOrEmpty(upgradeVGItemId)) {
				return (UpgradeVG) StoreInfo.GetItemByItemId(upgradeVGItemId);
			}

			return null;
		}
		
		protected override bool _isEquipped(EquippableVG good){
			bool res = false;
			int err = vgStorage_IsEquipped(good.ItemId, out res);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return res;
		}
		
		protected override void _equip(EquippableVG good, bool notify) {
			int err = vgStorage_Equip(good.ItemId, notify);
			IOS_ErrorCodes.CheckAndThrowException(err);
		}
		
		protected override void _unequip(EquippableVG good, bool notify) {
			int err = vgStorage_UnEquip(good.ItemId, notify);
			IOS_ErrorCodes.CheckAndThrowException(err);
		}
		
		protected override int _getBalance(VirtualItem item) {
			int outBalance = 0;
			int err = vgStorage_GetBalance(item.ItemId, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}
		
		protected override int _setBalance(VirtualItem item, int balance, bool notify) {
			int outBalance = 0;
			int err = vgStorage_SetBalance(item.ItemId, balance, notify, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}
		
		protected override int _add(VirtualItem item, int amount, bool notify){
			int outBalance = 0;
			int err = vgStorage_Add(item.ItemId, amount, notify, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}
		
		protected override int _remove(VirtualItem item, int amount, bool notify){
			int outBalance = 0;
			int err = vgStorage_Remove(item.ItemId, amount, notify, out outBalance);
			IOS_ErrorCodes.CheckAndThrowException(err);
			return outBalance;
		}
	
#endif
	}
}


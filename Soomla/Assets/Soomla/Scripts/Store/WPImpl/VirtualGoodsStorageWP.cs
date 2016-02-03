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
	/// abstract <c>VirtualGoodsStorage</c> for Android.
	/// </summary>
	public class VirtualGoodsStorageWP : VirtualGoodsStorage {
#if UNITY_WP8 && !UNITY_EDITOR

		protected override void _removeUpgrades(VirtualGood good, bool notify) {
            SoomlaWpStore.domain.virtualGoods.VirtualGood vg = (SoomlaWpStore.domain.virtualGoods.VirtualGood)SoomlaWpStore.data.StoreInfo.getVirtualItem(good.ItemId);
            SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().removeUpgrades(vg, notify);			
		}
		
		protected override void _assignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG, bool notify) {
            SoomlaWpStore.domain.virtualGoods.VirtualGood vg = (SoomlaWpStore.domain.virtualGoods.VirtualGood)SoomlaWpStore.data.StoreInfo.getVirtualItem(good.ItemId);
            SoomlaWpStore.domain.virtualGoods.UpgradeVG uvg = (SoomlaWpStore.domain.virtualGoods.UpgradeVG)SoomlaWpStore.data.StoreInfo.getVirtualItem(upgradeVG.ItemId);
            SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().assignCurrentUpgrade(vg, uvg, notify);
		}
		
		protected override UpgradeVG _getCurrentUpgrade(VirtualGood good) {
			SoomlaWpStore.domain.virtualGoods.UpgradeVG uvg = null;
            SoomlaWpStore.domain.virtualGoods.VirtualGood vg = (SoomlaWpStore.domain.virtualGoods.VirtualGood)SoomlaWpStore.data.StoreInfo.getVirtualItem(good.ItemId);
            if (vg is SoomlaWpStore.domain.virtualGoods.UpgradeVG)
            {
                uvg = SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().getCurrentUpgrade(vg);
            }
            
			if (uvg!=null) {
				return (UpgradeVG) StoreInfo.GetItemByItemId(uvg.GetId());
			}

			return null;
		}
		
		protected override bool _isEquipped(EquippableVG good){
			bool equipped;
            SoomlaWpStore.domain.virtualGoods.EquippableVG evg = (SoomlaWpStore.domain.virtualGoods.EquippableVG)SoomlaWpStore.data.StoreInfo.getVirtualItem(good.ItemId);
            equipped = SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().isEquipped(evg);
			return equipped;
		}
		
		protected override void _equip(EquippableVG good, bool notify) {
            SoomlaWpStore.domain.virtualGoods.EquippableVG evg = (SoomlaWpStore.domain.virtualGoods.EquippableVG)SoomlaWpStore.data.StoreInfo.getVirtualItem(good.ItemId);
            SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().equip(evg,notify);
		}
		
		protected override void _unequip(EquippableVG good, bool notify) {
            SoomlaWpStore.domain.virtualGoods.EquippableVG evg = (SoomlaWpStore.domain.virtualGoods.EquippableVG)SoomlaWpStore.data.StoreInfo.getVirtualItem(good.ItemId);
            SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().unequip(evg,notify);
		}


		protected override int _getBalance(VirtualItem item) {
			int retBalance;
			SoomlaWpStore.domain.VirtualItem vi = SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId);
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().getBalance(vi);
			return retBalance;
		}
		
		protected override int _setBalance(VirtualItem item, int balance, bool notify) {
			int retBalance;
			SoomlaWpStore.domain.VirtualItem vi = SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId);
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().setBalance(vi,balance,notify);
			return retBalance;
		}
		
		protected override int _add(VirtualItem item, int amount, bool notify){
			int retBalance;
            SoomlaWpStore.domain.VirtualItem vi = SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId);
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().add(vi, amount, notify);
			return retBalance;
		}
		
		protected override int _remove(VirtualItem item, int amount, bool notify){
			int retBalance;
            SoomlaWpStore.domain.VirtualItem vi = SoomlaWpStore.data.StoreInfo.getVirtualItem(item.ItemId);
            retBalance = SoomlaWpStore.data.StorageManager.getVirtualGoodsStorage().remove(vi, amount, notify);
			return retBalance;
		}
	
#endif
	}
}


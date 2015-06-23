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

using System;
using SoomlaWpCore;
using SoomlaWpCore.data;
using SoomlaWpStore.domain;
using SoomlaWpStore.domain.virtualGoods;
using SoomlaWpStore;
using SoomlaWpStore.events;
using SoomlaWpStore.exceptions;
using SoomlaWpCore.util;

namespace SoomlaWpStore.data 
{
 
/**
 * This class provides basic storage operations on virtual goods.
 */
public class VirtualGoodsStorage : VirtualItemStorage{

    /**
     * Constructor
     */
    public VirtualGoodsStorage() {
        mTag = "SOOMLA VirtualGoodsStorage";
    }

    /**
     * Removes any upgrade associated with the given <code>VirtualGood</code>.
     *
     * @param good the VirtualGood to remove upgrade from
     */
    public void removeUpgrades(VirtualGood good) {
        removeUpgrades(good, true);
    }

    /**
     * Removes any upgrade associated with the given VirtualGood.
     *
     * @param good the virtual good to remove the upgrade from
     * @param notify if true post event to bus
     */
    public void removeUpgrades(VirtualGood good, bool notify) {
        SoomlaUtils.LogDebug(mTag, "Removing upgrade information from virtual good: " +
                good.getName());

        String itemId = good.getItemId();
        String key = keyGoodUpgrade(itemId);

        KeyValueStorage.DeleteKeyValue(key);

        if (notify) {
            BusProvider.Instance.Post(new GoodUpgradeEvent(good, null));
        }
    }

    /**
     * Assigns a specific upgrade to the given virtual good.
     *
     * @param good the virtual good to upgrade
     * @param upgradeVG the upgrade to assign
     */
    public void assignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG) {
        assignCurrentUpgrade(good, upgradeVG, true);
    }

    /**
     * Assigns a specific upgrade to the given virtual good.
     *
     * @param good the VirtualGood to upgrade
     * @param upgradeVG the upgrade to assign
     * @param notify if true post event to bus
     */
    public void assignCurrentUpgrade(VirtualGood good, UpgradeVG upgradeVG, bool notify) {
        if (getCurrentUpgrade(good) != null && getCurrentUpgrade(good).getItemId() == upgradeVG.getItemId()) {
            return;
        }

        SoomlaUtils.LogDebug(mTag, "Assigning upgrade " + upgradeVG.getName() + " to virtual good: "
                + good.getName());

        String itemId = good.getItemId();
        String key = keyGoodUpgrade(itemId);
        String upItemId = upgradeVG.getItemId();

        KeyValueStorage.SetValue(key, upItemId);

        if (notify) {
			BusProvider.Instance.Post(new GoodUpgradeEvent(good,upgradeVG));
        }
    }

    /**
     * Retrieves the current upgrade for the given virtual good.
     *
     * @param good the virtual good to retrieve upgrade for
     * @return the current upgrade for the given virtual good
     */
    public UpgradeVG getCurrentUpgrade(VirtualGood good) {
        SoomlaUtils.LogDebug(mTag, "Fetching upgrade to virtual good: " + good.getName());

        String itemId = good.getItemId();
        String key = keyGoodUpgrade(itemId);

        String upItemId = KeyValueStorage.GetValue(key);

        if (upItemId == null) {
            SoomlaUtils.LogDebug(mTag, "You tried to fetch the current upgrade of " + good.getName()
                    + " but there's not upgrade to it.");
            return null;
        }

        try {
            return (UpgradeVG) StoreInfo.getVirtualItem(upItemId);
        } catch (VirtualItemNotFoundException e) {
            SoomlaUtils.LogError(mTag,
                    "The current upgrade's itemId from the DB is not found in StoreInfo." + " " + e.Message);
        } catch (InvalidCastException e) {
            SoomlaUtils.LogError(mTag,
                    "The current upgrade's itemId from the DB is not an UpgradeVG." + " " + e.Message);
        }

        return null;
    }

    /**
     * Checks if the given <code>EquippableVG</code> is currently equipped or not.
     *
     * @param good the <code>EquippableVG</code> to check the status for
     * @return true if the given good is equipped, false otherwise
     */
    public bool isEquipped(EquippableVG good){
        SoomlaUtils.LogDebug(mTag, "checking if virtual good with itemId: " + good.getItemId() +
                " is equipped.");

        String itemId = good.getItemId();
        String key = keyGoodEquipped(itemId);
        String val = KeyValueStorage.GetValue(key);

        return val != null;
    }

    /**
     * Equips the given <code>EquippableVG</code>.
     *
     * @param good the <code>EquippableVG</code> to equip
     */
    public void equip(EquippableVG good) {
        equip(good, true);
    }

    /**
     * Equips the given <code>EquippableVG</code>.
     *
     * @param good the EquippableVG to equip
     * @param notify if notify is true post event to bus
     */
    public void equip(EquippableVG good, bool notify) {
        if (isEquipped(good)) {
            return;
        }
        equipPriv(good, true, notify);
    }

    /**
     * UnEquips the given <code>EquippableVG</code>.
     *
     * @param good the <code>EquippableVG</code> to unequip
     */
    public void unequip(EquippableVG good) {
        unequip(good, true);
    }

    /**
     * UnEquips the given <code>EquippableVG</code>.
     *
     * @param good the <code>EquippableVG</code> to unequip
     * @param notify if true post event to bus
     */
    public void unequip(EquippableVG good, bool notify) {
        if (!isEquipped(good)) {
            return;
        }
        equipPriv(good, false, notify);
    }

    /**
     * @{inheritDoc}
     */
    protected override String keyBalance(String itemId) {
        return keyGoodBalance(itemId);
    }

    /**
     * @{inheritDoc}
     */
    protected override void postBalanceChangeEvent(VirtualItem item, int balance, int amountAdded) {
        BusProvider.Instance.Post(new GoodBalanceChangedEvent((VirtualGood)item,balance, amountAdded));
    }

    /**
     * Helper function for <code>equip</code> and <code>unequip</code> functions.
     */
    private void equipPriv(EquippableVG good, bool equip, bool notify){
        SoomlaUtils.LogDebug(mTag, (!equip ? "unequipping " : "equipping ") + good.getName() + ".");

        String itemId = good.getItemId();
        String key = keyGoodEquipped(itemId);

        if (equip) {
            KeyValueStorage.SetValue(key, "");
            if (notify) {
                BusProvider.Instance.Post(new GoodEquippedEvent(good));
            }
        } else {
            KeyValueStorage.DeleteKeyValue(key);
            if (notify) {
                BusProvider.Instance.Post(new GoodUnEquippedEvent(good));
            }
        }
    }


    private static String keyGoodBalance(String itemId) {
        return "good." + itemId + ".balance";
    }

    private static String keyGoodEquipped(String itemId) {
        return "good." + itemId + ".equipped";
    }

    private static String keyGoodUpgrade(String itemId) {
        return "good." + itemId + ".currentUpgrade";
    }

}
}
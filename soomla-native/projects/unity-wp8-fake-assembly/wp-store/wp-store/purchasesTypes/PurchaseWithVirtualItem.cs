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
using SoomlaWpStore;
using SoomlaWpCore;
using SoomlaWpCore.util;
using SoomlaWpStore.data;
using SoomlaWpStore.events;
using SoomlaWpStore.domain;
using SoomlaWpStore.exceptions;
using System.Diagnostics;

namespace SoomlaWpStore.purchasesTypes
{
/**
 * This type of purchase allows users to purchase <code>PurchasableVirtualItems</code> with other
 * <code>VirtualItems</code>.
 *
 * Real Game Example: Purchase a Sword in exchange for 100 Gems. 'Sword' is the item to be
 * purchased, 'Gem' is the target item, and 100 is the amount.
 */
public class PurchaseWithVirtualItem : PurchaseType {

    /**
     * Constructor
     *
     * @param targetItemId the itemId of the <code>VirtualItem</code> that is used to "pay" in
     *                     order to make the purchase.
     * @param amount the number of target items needed in order to make the purchase.
     */
    public PurchaseWithVirtualItem(String targetItemId, int amount) {
        mTargetItemId = targetItemId;
        mAmount = amount;
    }

    /**
     * Buys the virtual item with other virtual items.
     *
     * @throws com.soomla.store.exceptions.InsufficientFundsException
     */
    public override void buy(String payload){

        SoomlaUtils.LogDebug(TAG, "Trying to buy a " + getAssociatedItem().getName() + " with "
                + mAmount + " pieces of " + mTargetItemId);

        VirtualItem item = null;
        try {
            item = StoreInfo.getVirtualItem(mTargetItemId);
        } catch (VirtualItemNotFoundException e) {
            SoomlaUtils.LogError(TAG, "Target virtual item doesn't exist !" + " " + e.Message);
            return;
        }

		BusProvider.Instance.Post(new ItemPurchaseStartedEvent(getAssociatedItem()));

        VirtualItemStorage storage = StorageManager.getVirtualItemStorage(item);

        Debug.Assert(storage != null);
        int balance = storage.getBalance(item);
        if (balance < mAmount){
            throw new InsufficientFundsException(mTargetItemId);
        }

        storage.remove(item, mAmount);

        getAssociatedItem().give(1);
        //BusProvider.getInstance().post(new OnItemPurchasedEvent(getAssociatedItem(), payload));
		BusProvider.Instance.Post(new ItemPurchasedEvent(getAssociatedItem(), payload));
    }


    /** Setters and Getters */

    public String getTargetItemId() {
        return mTargetItemId;
    }

    public int getAmount() {
        return mAmount;
    }

    public void setAmount(int mAmount) {
        this.mAmount = mAmount;
    }


    /** Private Members */

    //used for Log messages
    private const String TAG = "SOOMLA PurchaseWithVirtualItem";

    //the itemId of the VirtualItem that is used to "pay" with in order to make the purchase
    private String mTargetItemId;

    private int mAmount; //the number of items to purchase.
}
}
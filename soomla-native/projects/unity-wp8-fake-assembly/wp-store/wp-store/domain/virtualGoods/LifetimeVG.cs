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
using SoomlaWpCore.util;
using SoomlaWpStore.data;
using SoomlaWpStore.purchasesTypes;
//using Newtonsoft.Json.Linq;
/**
 * A <code>LifetimeVG</code> is a virtual good that is bought once and kept forever.
 *
 * The <code>LifetimeVG</code>'s characteristics are:
 *  1. Can only be purchased once.
 *  2. Your users cannot have more than one of this item.
 *
 * Real Games Examples: 'No Ads', 'Double Coins'
 *
 * This <code>VirtualItem</code> is purchasable.
 * In case you want this item to be available for purchase in the market (PurchaseWithMarket),
 * you will need to define the item in the market (Google Play, Amazon App Store, etc...).
 *
 * Inheritance: LifetimeVG >
 * {@link com.soomla.store.domain.virtualGoods.VirtualGood} >
 * {@link com.soomla.store.domain.PurchasableVirtualItem} >
 * {@link com.soomla.store.domain.VirtualItem}
 */
namespace SoomlaWpStore.domain.virtualGoods
{

    public class LifetimeVG : VirtualGood{

    /** Constructor
     *
     * @param mName see parent
     * @param mDescription see parent
     * @param mItemId see parent
     * @param purchaseType see parent
     */
    public LifetimeVG(String mName, String mDescription,
                      String mItemId,
                      PurchaseType purchaseType) : base(mName, mDescription, mItemId, purchaseType) {
        
    }

    /**
     * Constructor
     *
     * @param jsonObject see parent
     * @throws JSONException
     */
    public LifetimeVG(object jsonObject) : base(jsonObject) {
        
    }

    /**
     * see parent
     *
     * @return see parent
     */
    public override JSONObject toJSONObject()
    {
        return base.toJSONObject();
    }

    /**
     * Gives your user exactly one <code>LifetimeVG</code>.
     *
     * @param amount the amount of the specific item to be given - if this input is greater than 1,
     *               we force the amount to equal 1, because a <code>LifetimeVG</code> can only be given once.
     * @return 1 to indicate that the user was given the good
     */
    public override int give(int amount, bool notify) {
        if(amount > 1) {
            SoomlaUtils.LogDebug(TAG, "You tried to give more than one LifetimeVG."
                    + "Will try to give one anyway.");
            amount = 1;
        }

        int balance = StorageManager.getVirtualGoodsStorage().getBalance(this);

        if (balance < 1) {
            return StorageManager.getVirtualGoodsStorage().add(this, amount, notify);
        }
        return 1;
    }

    /**
     * Takes from your user exactly one <code>LifetimeVG</code>.
     *
     * @param amount the amount of the specific item to be taken - if this input is greater than 1,
     *               we force amount to equal 1, because a <code>LifetimeVG</code> can only be
     *               given once and therefore, taken once.
     * @return 1 to indicate that the user was given the good
     */
    public override int take(int amount, bool notify) {
        if (amount > 1) {
            amount = 1;
        }

        int balance = StorageManager.getVirtualGoodsStorage().getBalance(this);

        if (balance > 0) {
            return StorageManager.getVirtualGoodsStorage().remove(this, amount, notify);
        }
        return 0;
    }

    /**
     * Determines if the user is in a state that allows him/her to buy a <code>LifetimeVG</code>,
     * by checking his/her balance of <code>LifetimeVG</code>s.
     * From the definition of a <code>LifetimeVG</code>:
     * If the user has a balance of 0 - he/she can buy.
     * If the user has a balance of 1 or more - he/she cannot buy more.
     *
     * @return true if buying is allowed, false otherwise
     */
    protected override bool CanBuy() {
        int balance = StorageManager.getVirtualGoodsStorage().getBalance(this);

        return balance < 1;
    }


    /** Private Members **/

    private const String TAG = "SOOMLA LifetimeVG"; //used for Log messages
}
}
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
using SoomlaWpStore.domain;
using SoomlaWpStore.exceptions;
using SoomlaWpStore.purchasesTypes;
//using Newtonsoft.Json.Linq;
namespace SoomlaWpStore.domain.virtualCurrencies
{
/**
 * Every game has its virtual currencies. This class represents a pack of a specific
 * {@link com.soomla.store.domain.virtualCurrencies.VirtualCurrency}.
 *
 * Real Game Example: If the virtual currency in your game is a 'Coin', you will sell packs of
 * 'Coins' such as "10 Coins Set" or "Super Saver Pack".
 *
 * NOTE: In case you want this item to be available for purchase with real money  you will need to
 * define the item in the market (Google Play, Amazon App Store, etc...).
 *
 * Inheritance: VirtualCurrencyPack >
 * {@link com.soomla.store.domain.PurchasableVirtualItem} >
 * {@link com.soomla.store.domain.VirtualItem}
 */
public class VirtualCurrencyPack : PurchasableVirtualItem {

    /**
     * Constructor
     *
     * @param mName see parent
     * @param mDescription see parent
     * @param mItemId see parent
     * @param mCurrencyAmount the amount of currency in the pack
     * @param mCurrencyItemId the item id of the currency associated with this pack
     * @param purchaseType see parent
     */
    public VirtualCurrencyPack(String mName, String mDescription, String mItemId,
                               int mCurrencyAmount,
                               String mCurrencyItemId,
                               PurchaseType purchaseType) : base(mName, mDescription, mItemId, purchaseType){
        
        this.mCurrencyItemId = mCurrencyItemId;
        this.mCurrencyAmount = mCurrencyAmount;
    }

    /**
     * Constructor
     *
     * @param jsonObject see parent
     * @throws JSONException
     */
    public VirtualCurrencyPack(object jsonObject) : base(jsonObject) {
        
    }

    /**
     * @{inheritDoc}
     */
    public override JSONObject toJSONObject()
    {

        return new JSONObject();
    }

    /**
     * @{inheritDoc}
     */
    public override int give(int amount, bool notify) {
        VirtualCurrency currency = null;
        try {
            currency = (VirtualCurrency)StoreInfo.getVirtualItem(mCurrencyItemId);
        } catch (VirtualItemNotFoundException e) {
            SoomlaUtils.LogError(TAG, "VirtualCurrency with itemId: " + mCurrencyItemId
                    + " doesn't exist! Can't give this pack." + " " + e.Message);
            return 0;
        }
        return StorageManager.getVirtualCurrencyStorage().add(
                currency, mCurrencyAmount * amount, notify);
    }

    /**
     * @{inheritDoc}
     */
    public override int take(int amount, bool notify) {
        VirtualCurrency currency = null;
        try {
            currency = (VirtualCurrency)StoreInfo.getVirtualItem(mCurrencyItemId);
        } catch (VirtualItemNotFoundException e) {
            SoomlaUtils.LogError(TAG, "VirtualCurrency with itemId: " + mCurrencyItemId +
                    " doesn't exist! Can't take this pack." + " " + e.Message);
            return 0;
        }
        return StorageManager.getVirtualCurrencyStorage().remove(currency,
                mCurrencyAmount * amount, notify);
    }

    /**
     * @{inheritDoc}
     */
    public override int resetBalance(int balance, bool notify) {
        // Not supported for VirtualCurrencyPacks !
        SoomlaUtils.LogError(TAG, "Someone tried to reset balance of CurrencyPack. "
                + "That's not right.");
        return 0;
    }

    /**
     * @{inheritDoc}
     */
    protected override bool CanBuy() {
        return true;
    }


    /** Setters and Getters **/

    public int getCurrencyAmount() {
        return mCurrencyAmount;
    }

    public String getCurrencyItemId() {
        return mCurrencyItemId;
    }


    /** Private Members **/

    private const String TAG = "SOOMLA VirtualCurrencyPack"; //used for Log messages

    private int mCurrencyAmount; //the amount of currency in the pack

    private String mCurrencyItemId; //the itemId of the currency associated with this pack
}
}
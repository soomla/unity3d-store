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
using SoomlaWpStore.domain;
using SoomlaWpStore.data;
//using Newtonsoft.Json.Linq;
namespace SoomlaWpStore.domain.virtualCurrencies
{
/**
 * This is a representation of a game's virtual currency.
 * Each game can have multiple instances of a virtual currency, all kept in
 * {@link com.soomla.store.data.StoreInfo}
 *
 * Real Game Examples: 'Coin', 'Gem', 'Muffin'
 *
 * NOTE: This item is NOT purchasable!
 * However, a <code>VirtualCurrencyPack</code> IS purchasable.
 * For example, if the virtual currency in your game is a 'Coin' and you want to make a single
 * 'Coin' available for purchase you will need to define a <code>VirtualCurrencyPack</code> of 1
 * 'Coin'.
 */
public class VirtualCurrency : VirtualItem {

    /**
     * Constructor
     * see parent
     *
     * @param mName the name of the virtual item
     * @param mDescription the description of the virtual item
     * @param itemId the itemId of the virtual item
     */
    public VirtualCurrency(String mName, String mDescription, String itemId) : base(mName, mDescription, itemId) {
        
    }

    /**
     * Constructor
     *
     * @param jsonObject a JSONObject representation of the wanted VirtualItem
     * @throws JSONException
     */
    public VirtualCurrency(object jsonObject) : base(jsonObject) {
        
    }

    /**
     * see parent
     *
     * @return see parent
     */
    public override JSONObject toJSONObject(){
        return base.toJSONObject();
    }

    /**
     * @{inheritDoc}
     */
    public override int give(int amount, bool notify) {
        return StorageManager.getVirtualCurrencyStorage().add(this, amount, notify);
    }

    /**
     * @{inheritDoc}
     */
    public override int take(int amount, bool notify) {
        return StorageManager.getVirtualCurrencyStorage().remove(this, amount, notify);
    }

    /**
     * @{inheritDoc}
     */
    public override int resetBalance(int balance, bool notify) {
        return StorageManager.getVirtualCurrencyStorage().setBalance(this, balance, notify);
    }
}
}
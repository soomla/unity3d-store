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
////using Newtonsoft.Json.Linq;
/**
 * Single use virtual goods are the most common type of <code>VirtualGood</code>.
 *
 * The <code>SingleUseVG</code>'s characteristics are:
 *  1. Can be purchased an unlimited number of times.
 *  2. Has a balance that is saved in the database. Its balance goes up when you "give" it or
 *     "buy" it. The balance goes down when you "take" or (unfriendly) "refund" it.
 *
 * Real Game Examples: 'Hat', 'Sword', 'Muffin'
 *
 * NOTE: In case you want this item to be available for purchase with real money
 * you will need to define the item in the market (Google Play, Amazon App Store, etc...).
 *
 * Inheritance: SingleUseVG} >
 * {@link com.soomla.store.domain.virtualGoods.VirtualGood} >
 * {@link com.soomla.store.domain.PurchasableVirtualItem} >
 * {@link com.soomla.store.domain.VirtualItem}
 */
namespace SoomlaWpStore.domain.virtualGoods
{

public class SingleUseVG : VirtualGood{

    /**
     * Constructor
     *
     * @param mName see parent
     * @param mDescription see parent
     * @param mItemId see parent
     * @param purchaseType see parent
     */
    public SingleUseVG(String mName, String mDescription, String mItemId, PurchaseType purchaseType) : base(mName, mDescription, mItemId, purchaseType) {
        
    }

    /**
     * Constructor
     *
     * @param jsonObject see parent
     * @throws JSONException
     */
    public SingleUseVG(object jsonObject) : base(jsonObject) {
        
    }

    /**
     * @{inheritDoc}
     */
    public override JSONObject toJSONObject()
    {
        return base.toJSONObject();
    }

    /**
     * @{inheritDoc}
     */
    public override int give(int amount, bool notify) {
        return StorageManager.getVirtualGoodsStorage().add(this, amount, notify);
    }

    /**
     * @{inheritDoc}
     */
    public override int take(int amount, bool notify) {
        return StorageManager.getVirtualGoodsStorage().remove(this, amount, notify);
    }

    /**
     * Determines if user is in a state that allows him/her to buy a <code>SingleUseVG</code>.
     *
     * @return true - users can ALWAYS purchase <code>SingleUseVG</code>s.
     */
    protected override bool CanBuy() {
        return true;
    }
}
}
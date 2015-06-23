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
using SoomlaWpStore.domain;
using SoomlaWpStore.domain.virtualCurrencies;
using SoomlaWpStore.domain.virtualGoods;

namespace SoomlaWpStore.data 
{
/**
 * In this class all the relevant storage classes are created.
 * This class contains static methods for you to retrieve the various storage bases.
 */
public class StorageManager {

    /** Setters and Getters **/

    public static VirtualCurrencyStorage getVirtualCurrencyStorage() {

        return mVirtualCurrencyStorage;
    }

    public static VirtualGoodsStorage getVirtualGoodsStorage() {
        return mVirtualGoodsStorage;
    }

    /*public static NonConsumableItemsStorage getNonConsumableItemsStorage() {
        return mNonConsumableItemsStorage;
    }*/

    /**
     * Checks whether the given item belongs to <code>VirtualGoodStorage</code> or
     * <code>VirtualCurrencyStorage</code>.
     *
     * @param item the item to check what type of storage it belongs to.
     * @return the type of VirtualItemStorage.
     */
    public static VirtualItemStorage getVirtualItemStorage(VirtualItem item) {
        VirtualItemStorage storage = null;
        if (item is VirtualGood) {
            storage = getVirtualGoodsStorage();
        } else if (item is VirtualCurrency) {
            storage = getVirtualCurrencyStorage();
        }
        return storage;
    }


    /** Private Members **/

    private const String TAG = "SOOMLA StorageManager"; //used for Log messages

    // storage of all virtual goods
    private static VirtualGoodsStorage mVirtualGoodsStorage = new VirtualGoodsStorage();

    // storage of all virtual currencies
    private static VirtualCurrencyStorage mVirtualCurrencyStorage = new VirtualCurrencyStorage();

    // storage of all non-consumable items
    /*private static NonConsumableItemsStorage mNonConsumableItemsStorage =
            new NonConsumableItemsStorage();*/

}
}
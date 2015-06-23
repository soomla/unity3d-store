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

namespace SoomlaWpStore.data 
{
/**
 * This class is an abstract definition of a Virtual Item Storage.
 */
public abstract class VirtualItemStorage {

    /**
     * Retrieves the balance of the given virtual item.
     *
     * @param item the required virtual item
     * @return the balance of the required virtual item
     */
    public int getBalance(VirtualItem item){
        SoomlaUtils.LogDebug(mTag, "fetching balance for virtual item with itemId: "
                + item.getItemId());

        String itemId = item.getItemId();
        String key = keyBalance(itemId);
        String val = KeyValueStorage.GetValue(key);

        int balance = 0;
        if (val != null) {
            try
            {
                balance = int.Parse(val);
            }
            catch (Exception e)
            {
                SoomlaUtils.LogError(mTag, "Error casting string to int value: "+val+" "+e.Message);
                return 0;
            }
        }

        SoomlaUtils.LogDebug(mTag, "the balance for " + item.getItemId() + " is " + balance);

        return balance;
    }

    /**
     * Sets the balance of the given virtual item to be the given balance.
     *
     * @param item the required virtual item
     * @param balance the new balance to be set
     * @return the balance of the required virtual item
     */
    public int setBalance(VirtualItem item, int balance) {
        return setBalance(item, balance, true);
    }

    /**
     * Sets the balance of the given virtual item to be the given balance, and if notify is true
     * posts the change in the balance to the event bus.
     *
     * @param item the required virtual item
     * @param balance the new balance to be set
     * @param notify if notify is true post balance change event
     * @return the balance of the required virtual item
     */
    public int setBalance(VirtualItem item, int balance, bool notify) {
        SoomlaUtils.LogDebug(mTag, "setting balance " + balance + " to " + item.getName() + ".");

        int oldBalance = getBalance(item);
        if (oldBalance == balance) {
            return balance;
        }

        String itemId = item.getItemId();

        String balanceStr = balance.ToString();
        String key = keyBalance(itemId);

        KeyValueStorage.SetValue(key, balanceStr);

        if (notify) {
            postBalanceChangeEvent(item, balance, 0);
        }

        return balance;
    }

    /**
     * Adds the given amount of items to the storage.
     *
     * @param item the required virtual item
     * @param amount the amount of items to add
     * @return new balance
     */
    public int add(VirtualItem item, int amount){
        return add(item, amount, true);
    }

    /**
     * Adds the given amount of items to the storage, and if notify is true
     * posts the change in the balance to the event bus.
     *
     * @param item the required virtual item
     * @param amount the amount of items to add
     * @param notify if true posts balance change event
     * @return new balance
     */
    public int add(VirtualItem item, int amount, bool notify){
        SoomlaUtils.LogDebug(mTag, "adding " + amount + " " + item.getName());

        String itemId = item.getItemId();
        int balance = getBalance(item);
        if (balance < 0) { /* in case the user "adds" a negative value */
            balance = 0;
            amount = 0;
        }
        String balanceStr = (balance + amount).ToString();
        String key = keyBalance(itemId);
        KeyValueStorage.SetValue(key, balanceStr);

        if (notify) {
            postBalanceChangeEvent(item, balance+amount, amount);
        }

        return balance + amount;
    }

    /**
     * Removes the given amount from the given virtual item's balance.
     *
     * @param item is the virtual item to remove the given amount from
     * @param amount is the amount to remove
     * @return new balance
     */
    public int remove(VirtualItem item, int amount){
        return remove(item, amount, true);
    }

    /**
     * Removes the given amount from the given virtual item's balance, and if notify is true
     * posts the change in the balance to the event bus.
     *
     * @param item is the virtual item to remove the given amount from
     * @param amount is the amount to remove
     * @param notify if notify is true post balance change event
     * @return new balance
     */
    public int remove(VirtualItem item, int amount, bool notify){
        SoomlaUtils.LogDebug(mTag, "Removing " + amount + " " + item.getName() + ".");

        String itemId = item.getItemId();
        int balance = getBalance(item) - amount;
        if (balance < 0) {
            balance = 0;
            amount = 0;
        }
        String balanceStr = balance.ToString();
        String key = keyBalance(itemId);
        KeyValueStorage.SetValue(key, balanceStr);

        if (notify) {
            postBalanceChangeEvent(item, balance, -1*amount);
        }

        return balance;
    }

    /**
     * Retrieves the balance of the virtual item with the given itemId from the
     * <code>KeyValDatabase</code>.
     *
     * @param itemId id of the virtual item whose balance is to be retrieved
     * @return String containing name of storage base, itemId, and balance
     */
    protected abstract String keyBalance(String itemId);

    /**
     * Posts the given amount changed in the given balance of the given virtual item.
     *
     * @param item virtual item whose balance has changed
     * @param balance the balance that has changed
     * @param amountAdded the amount added to the item's balance
     */
    protected abstract void postBalanceChangeEvent(VirtualItem item, int balance, int amountAdded);


    /** Private Members */

    protected String mTag = "SOOMLA VirtualItemStorage"; //used for Log messages
}
}
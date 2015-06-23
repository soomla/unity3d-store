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

namespace SoomlaWpStore.exceptions
{

    /**
     * This exception is thrown when a user tries to make a purchase and does not have enough funds.
     *
     * Real Game Example:
     *  Example Inventory: { currency_coin: 100, green_hat: 3, blue_hat: 5 }
     *  Say a blue_hat costs 200 currency_coin.
     *  Suppose that you have a user that wants to buy a blue_hat.
     *  You'll probably call <code>StoreInventory.buy("blue_hat")</code>.
     *  <code>InsufficientFundsException</code> will be thrown with "blue_hat" as the itemId.
     *  You can just catch this exception in order to notify the user that they don't have enough
     *  coins to buy a blue_hat.
     */
    public class InsufficientFundsException : Exception
    {

        /**
         * Constructor
         *
         * @param itemId id of the item that was attempted to buy with.
         */
        public InsufficientFundsException(String itemId) : base("You tried to buy with itemId: " + itemId + " but you don't have enough of it.")
        {
            mItemId = itemId;
        }

        /** Setters and Getters */

        public String getItemId()
        {
            return mItemId;
        }


        /** Private Members */

        private String mItemId;
    }
}
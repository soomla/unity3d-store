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
using SoomlaWpCore.util;
using SoomlaWpStore.events;


/**
 * This class provides basic storage operations on virtual currencies.
 */
namespace SoomlaWpStore.data 
{
public class VirtualCurrencyStorage : VirtualItemStorage{

    /**
     * Constructor
     */
    public VirtualCurrencyStorage() {
        mTag = "SOOMLA VirtualCurrencyStorage";
    }

    /**
     * @{inheritDoc}
     */
    protected override String keyBalance(String itemId) {
        return keyCurrencyBalance(itemId);
    }

    /**
     * @{inheritDoc}
     */
    protected override void postBalanceChangeEvent(VirtualItem item, int balance, int amountAdded) {
		BusProvider.Instance.Post(new CurrencyBalanceChangedEvent((VirtualCurrency) item,balance, amountAdded));
    }

    private static String keyCurrencyBalance(String itemId) {
        return "currency." + itemId + ".balance";
    }
}
}
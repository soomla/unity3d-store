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
namespace SoomlaWpStore.data 
{

/**
 * This class contains all const String names of the keys/vals in the JSON being parsed all
 * around the sdk.
 */
public class StoreJSONConsts {

    public const String STORE_CURRENCIES         = "currencies";
    public const String STORE_CURRENCYPACKS      = "currencyPacks";
    public const String STORE_GOODS              = "goods";
    public const String STORE_CATEGORIES         = "categories";
    public const String STORE_NONCONSUMABLES     = "nonConsumables";
    public const String STORE_GOODS_SU           = "singleUse";
    public const String STORE_GOODS_PA           = "goodPacks";
    public const String STORE_GOODS_UP           = "goodUpgrades";
    public const String STORE_GOODS_LT           = "lifetime";
    public const String STORE_GOODS_EQ           = "equippable";

    public const String ITEM_NAME                = "name";
    public const String ITEM_DESCRIPTION         = "description";
    public const String ITEM_ITEMID              = "itemId";

    public const String CATEGORY_NAME            = "name";
    public const String CATEGORY_GOODSITEMIDS    = "goods_itemIds";

    public const String MARKETITEM_PRODUCT_ID    = "productId";
    public const String MARKETITEM_ANDROID_ID    = "androidId";
    public const String MARKETITEM_MANAGED       = "consumable";
    public const String MARKETITEM_PRICE         = "price";
    public const String MARKETITEM_MARKETPRICE   = "marketPrice";
    public const String MARKETITEM_MARKETTITLE   = "marketTitle";
    public const String MARKETITEM_MARKETDESC    = "marketDesc";
    public const String MARKETITEM_MARKETCURRENCYCODE = "marketCurrencyCode";
    public const String MARKETITEM_MARKETPRICEMICROS = "marketPriceMicros";

    public const String EQUIPPABLE_EQUIPPING     = "equipping";

    // VGP = SingleUsePackVG
    public const String VGP_GOOD_ITEMID          = "good_itemId";
    public const String VGP_GOOD_AMOUNT          = "good_amount";

    // VGU = UpgradeVG
    public const String VGU_GOOD_ITEMID          = "good_itemId";
    public const String VGU_PREV_ITEMID          = "prev_itemId";
    public const String VGU_NEXT_ITEMID          = "next_itemId";

    public const String CURRENCYPACK_CURRENCYAMOUNT = "currency_amount";
    public const String CURRENCYPACK_CURRENCYITEMID = "currency_itemId";

    /** IabPurchase Type **/
    public const String PURCHASABLE_ITEM         = "purchasableItem";

    public const String PURCHASE_TYPE            = "purchaseType";
    public const String PURCHASE_TYPE_MARKET     = "market";
    public const String PURCHASE_TYPE_VI         = "virtualItem";

    public const String PURCHASE_MARKET_ITEM     = "marketItem";

    public const String PURCHASE_VI_ITEMID       = "pvi_itemId";
    public const String PURCHASE_VI_AMOUNT       = "pvi_amount";

    // VIR = VirtualItemReward
    public const String VIR_ASSOCITEMID = "associatedItemId";
    public const String VIR_AMOUNT = "amount";

}
}

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

namespace Soomla.Store {

	/// <summary>
	/// This class contains all string names of the keys/vals in the JSON being parsed all around the SDK.
	/// </summary>
	public static class StoreJSONConsts
	{
	    public const string STORE_CURRENCIES         = "currencies";
	    public const string STORE_CURRENCYPACKS      = "currencyPacks";
	    public const string STORE_GOODS              = "goods";
	    public const string STORE_CATEGORIES         = "categories";
	    public const string STORE_GOODS_SU           = "singleUse";
	    public const string STORE_GOODS_PA           = "goodPacks";
	    public const string STORE_GOODS_UP           = "goodUpgrades";
	    public const string STORE_GOODS_LT           = "lifetime";
	    public const string STORE_GOODS_EQ           = "equippable";

	    public const string CATEGORY_NAME            = "name";
	    public const string CATEGORY_GOODSITEMIDS    = "goods_itemIds";

	    public const string MARKETITEM_PRODUCT_ID    = "productId";
#if UNITY_IOS && !UNITY_EDITOR
		public const string MARKETITEM_IOS_ID    	 = "iosId";
#elif UNITY_ANDROID && !UNITY_EDITOR
		public const string MARKETITEM_ANDROID_ID    = "androidId";
#endif
	    public const string MARKETITEM_PRICE         = "price";
		public const string MARKETITEM_MARKETPRICE   = "marketPrice";
		public const string MARKETITEM_MARKETTITLE   = "marketTitle";
		public const string MARKETITEM_MARKETDESC    = "marketDesc";
		public const string MARKETITEM_MARKETCURRENCYCODE   = "marketCurrencyCode";
		public const string MARKETITEM_MARKETPRICEMICROS    = "marketPriceMicros";

	    public const string EQUIPPABLE_EQUIPPING     = "equipping";

	    /// VGP = SingleUsePackVG
	    public const string VGP_GOOD_ITEMID          = "good_itemId";
	    public const string VGP_GOOD_AMOUNT          = "good_amount";

	    /// VGU = UpgradeVG
	    public const string VGU_NEXT_ITEMID          = "next_itemId";
	    public const string VGU_GOOD_ITEMID          = "good_itemId";
	    public const string VGU_PREV_ITEMID          = "prev_itemId";

	    public const string CURRENCYPACK_CURRENCYAMOUNT = "currency_amount";
	    public const string CURRENCYPACK_CURRENCYITEMID = "currency_itemId";

	    /// Purchase Type
	    public const string PURCHASABLE_ITEM         = "purchasableItem";

	    public const string PURCHASE_TYPE            = "purchaseType";
	    public const string PURCHASE_TYPE_MARKET     = "market";
	    public const string PURCHASE_TYPE_VI         = "virtualItem";

	    public const string PURCHASE_MARKET_ITEM     = "marketItem";

	    public const string PURCHASE_VI_ITEMID       = "pvi_itemId";
	    public const string PURCHASE_VI_AMOUNT       = "pvi_amount";

		// VIR = VirtualItemReward
		public const string VIR_ASSOCITEMID          = "associatedItemId";
		public const string VIR_AMOUNT               = "amount";
	}
}

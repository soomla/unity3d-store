using System;

namespace Soomla
{
	public static class JSONConsts
	{
	    public const string STORE_CURRENCIES         = "currencies";
	    public const string STORE_CURRENCYPACKS      = "currencyPacks";
	    public const string STORE_GOODS              = "goods";
	    public const string STORE_CATEGORIES         = "categories";
	    public const string STORE_NONCONSUMABLES     = "nonConsumables";
	    public const string STORE_GOODS_SU           = "singleUse";
	    public const string STORE_GOODS_PA           = "goodPacks";
	    public const string STORE_GOODS_UP           = "goodUpgrades";
	    public const string STORE_GOODS_LT           = "lifetime";
	    public const string STORE_GOODS_EQ           = "equippable";
	
	    public const string ITEM_NAME                = "name";
	    public const string ITEM_DESCRIPTION         = "description";
	    public const string ITEM_ITEMID              = "itemId";
	
	    public const string CATEGORY_NAME            = "name";
	    public const string CATEGORY_GOODSITEMIDS    = "goods_itemIds";
	
	    public const string MARKETITEM_PRODUCT_ID    = "productId";
#if UNITY_IOS && !UNITY_EDITOR
		public const string MARKETITEM_IOS_ID    	 = "iosId";
#elif UNITY_ANDROID && !UNITY_EDITOR
		public const string MARKETITEM_ANDROID_ID    = "androidId";
#endif
	    public const string MARKETITEM_CONSUMABLE    = "consumable";
	    public const string MARKETITEM_PRICE         = "price";
	
	    public const string EQUIPPABLE_EQUIPPING     = "equipping";
	
	    /// VGP = SingleUsePackVG
	    public const string VGP_GOOD_ITEMID          = "good_itemId";
	    public const string VGP_GOOD_AMOUNT          = "good_amount";
	
	    // VGU = UpgradeVG
	    public const string VGU_NEXT_ITEMID          = "next_itemId";
	    public const string VGU_GOOD_ITEMID          = "good_itemId";
	    public const string VGU_PREV_ITEMID          = "prev_itemId";
	
	    public const string CURRENCYPACK_CURRENCYAMOUNT = "currency_amount";
	    public const string CURRENCYPACK_CURRENCYITEMID = "currency_itemId";
	
	    // Purchase Type
	    public const string PURCHASABLE_ITEM         = "purchasableItem";
	
	    public const string PURCHASE_TYPE            = "purchaseType";
	    public const string PURCHASE_TYPE_MARKET     = "market";
	    public const string PURCHASE_TYPE_VI         = "virtualItem";
	
	    public const string PURCHASE_MARKET_ITEM     = "marketItem";
	
	    public const string PURCHASE_VI_ITEMID       = "pvi_itemId";
	    public const string PURCHASE_VI_AMOUNT       = "pvi_amount";
	}
}


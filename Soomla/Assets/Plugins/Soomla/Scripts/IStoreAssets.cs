using System;

namespace Soomla
{
	/// <summary>
	/// This interface represents a single game's metadata.
 	/// Use this interface to create your assets class that will be transferred to StoreInfo
 	/// upon initialization.
	/// </summary>
	public interface IStoreAssets
	{
		
		/// <summary>
		/// This value will determine if the saved data in the database will be deleted or not.
		/// Bump the version every time you want to delete the old data in the DB.
		/// If you don't bump this value, you won't be able to see changes you've made to the objects in this file.
		/// 
		/// For example: If you previously created a VirtualGood with name "Hat" and you published your application,
		/// the name "Hat will be saved in any of your users' databases. If you want to change the name to "Green Hat"
		/// than you'll also have to bump the version (from 0 to 1). Now the new "Green Hat" name will replace the old one.
		/// </summary>
		/// <returns>
		/// the version of your specific IStoreAssets.
		/// </returns>
		int GetVersion();
		
		/**
		 * NOTE: The order of the items in the array will be their order when shown to the user (if you're using a store generated from designer.soom.la).
		 **/
		
		/// <summary>
		/// A representation of your game's virtual currencies.
		/// </summary>
		/// <returns>
		/// The virtual currencies.
		/// </returns>
	    VirtualCurrency[] GetCurrencies();
	

		/// <summary>
		/// An array of all virtual goods served by your store.
		/// </summary>
		/// <returns>
		/// The virtual goods.
		/// </returns>
	    VirtualGood[] GetGoods();
	
		/// <summary>
		/// An array of all virtual currency packs served by your store.
		/// </summary>
		/// <returns>
		/// The virtual currency packs.
		/// </returns>
	    VirtualCurrencyPack[] GetCurrencyPacks();
	
		/// <summary>
		/// An array of all virtual categories served by your store.
		/// </summary>
		/// <returns>
		/// The virtual categories.
		/// </returns>
	    VirtualCategory[] GetCategories();
	
		/// <summary>
		/// You can define NON-CONSUMABLE items that you'd like to use for your needs.
		/// NON-CONSUMABLE items are usually used to let users purchase something like a "no-ads" token.
		/// 
		/// NOTE: CONSUMABLE items are usually just currency packs. If you use SOOMLA's storefront, it'll take care of
		/// the CONSUMABLE items for you in the UI.
		/// </summary>
		/// <returns>
		/// The Non Consumable items.
		/// </returns>
	    NonConsumableItem[] GetNonConsumableItems();
	}
}


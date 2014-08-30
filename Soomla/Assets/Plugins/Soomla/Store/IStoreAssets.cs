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
	/// This interface represents a single game's economy. Use this interface to create your assets 
	/// class that will be transferred to <c>StoreInfo</c> upon initialization.
	/// </summary>
	public interface IStoreAssets {
		
		/// <summary>
		/// Retrieves the current version of your <c>IStoreAssets</c>.
		/// This value will determine if the saved data in the database will be deleted or not.
		/// Bump the version every time you want to delete the old data in the DB.
		/// 
		/// Real Game Example:
		/// Suppose that your game has a <c>VirtualGood</c> called "Hat".
		/// Let's say your game's <c>IStoreAssets</c> version is currently 0.
		/// Now you want to change the name "Hat" to "Green Hat" - you will need to bump the version
		/// from 0 to 1, in order for the new name, "Green Hat" to replace the old one, "Hat".
		/// 
		/// Explanation: The local database on every one of your users' devices keeps your economy's
		/// metadata, such as the <c>VirtualGood</c>'s name "Hat". When you change
		/// <c>IStoreAssets</c>, you must bump the version in order for the data to change in
		/// your users' local databases.
		/// 
		/// You need to bump the version after ANY change in <c>IStoreAssets</c> for the local
		/// database to realize it needs to refresh its data.
		/// </summary>
		/// <returns>the version of your specific <c>IStoreAssets</c>.</returns>
		int GetVersion();

		/// <summary>
		/// Retrieves the array of your game's virtual currencies.
		/// </summary>
		/// <returns>All virtual currencies in your game.</returns>
	    VirtualCurrency[] GetCurrencies();
	
		/// <summary>
		/// Retrieves the array of all virtual goods served by your store (all kinds in one array).
		/// </summary>
		/// <returns>All virtual goods in your game.</returns>
	    VirtualGood[] GetGoods();
	
		/// <summary>
		/// Retrieves the array of all virtual currency packs served by your store.
		/// </summary>
		/// <returns>All virtual currency packs in your game.</returns>
	    VirtualCurrencyPack[] GetCurrencyPacks();
	
		/// <summary>
		/// Retrieves the array of all virtual categories handled in your store.
		/// </summary>
		/// <returns>All virtual categories in your game.</returns>
	    VirtualCategory[] GetCategories();
	
		/// <summary>
		/// Retrieves the array of all non-consumable items served by your store.
		/// </summary>
		/// <returns>All non consumable items served in your game.</returns>
	    NonConsumableItem[] GetNonConsumableItems();
	}
}


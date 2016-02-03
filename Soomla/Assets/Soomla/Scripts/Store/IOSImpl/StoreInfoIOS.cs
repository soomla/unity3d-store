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

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace Soomla.Store {

	/// <summary>
	/// <c>StoreInfo</c> for iOS.
	/// This class holds the store's meta data including:
	/// virtual currencies definitions,
	/// virtual currency packs definitions,
	/// virtual goods definitions,
	/// virtual categories definitions, and
	/// virtual non-consumable items definitions
	/// </summary>
	public class StoreInfoIOS : StoreInfo {

#if UNITY_IOS && !UNITY_EDITOR

		/// Functions that call iOS-store functions.
		[DllImport ("__Internal")]
		private static extern int storeInfo_SetStoreAssets(string storeMetaJSON, int version);
		[DllImport ("__Internal")]
		private static extern int storeInfo_LoadFromDB();

		/// <summary>
		/// Initializes <c>StoreInfo</c>.
		/// On first initialization, when the database doesn't have any previous version of the store
		/// metadata, <c>StoreInfo</c> gets loaded from the given <c>IStoreAssets</c>.
		/// After the first initialization, <c>StoreInfo</c> will be initialized from the database.
		///
		/// IMPORTANT: If you want to override the current <c>StoreInfo</c>, you'll have to bump
		/// the version of your implementation of <c>IStoreAssets</c> in order to remove the
		/// metadata when the application loads. Bumping the version is done by returning a higher
		/// number in <c>IStoreAssets</c>'s <c>getVersion</c>.
		/// </summary>
		/// <param name="storeAssets">your game's economy</param>
		override protected void _setStoreAssets(IStoreAssets storeAssets) {
			SoomlaUtils.LogDebug(TAG, "pushing IStoreAssets to StoreInfo on iOS side");
			string storeAssetsJSON = IStoreAssetsToJSON(storeAssets);
			int version = storeAssets.GetVersion();
			int err = storeInfo_SetStoreAssets(storeAssetsJSON, version);
			IOS_ErrorCodes.CheckAndThrowException(err);
			SoomlaUtils.LogDebug(TAG, "done! (pushing data to StoreAssets on iOS side)");
		}

		protected override void loadNativeFromDB() {
			int err = storeInfo_LoadFromDB();
			IOS_ErrorCodes.CheckAndThrowException(err);
		}

#endif
	}
}

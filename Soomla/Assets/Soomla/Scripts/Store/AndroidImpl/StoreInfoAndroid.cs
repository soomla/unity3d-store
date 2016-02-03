/// Copyright (C) 2012-2014 Soomla Inc
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
	/// <c>StoreInfo</c> for Android.
	/// This class holds the store's meta data including:
	/// virtual currencies definitions,
	/// virtual currency packs definitions,
	/// virtual goods definitions,
	/// virtual categories definitions, and
	/// virtual non-consumable items definitions
	/// </summary>
	public class StoreInfoAndroid : StoreInfo {

#if UNITY_ANDROID && !UNITY_EDITOR

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
		override protected void _setStoreAssets(IStoreAssets storeAssets) {
			SoomlaUtils.LogDebug(TAG, "pushing IStoreAssets to StoreInfo on java side");
			AndroidJNI.PushLocalFrame(100);
			string storeAssetsJSON = IStoreAssetsToJSON(storeAssets);
			int version = storeAssets.GetVersion();
			using(AndroidJavaClass jniStoreInfoClass = new AndroidJavaClass("com.soomla.store.data.StoreInfo")) {
				jniStoreInfoClass.CallStatic("setStoreAssets", version, storeAssetsJSON);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			SoomlaUtils.LogDebug(TAG, "done! (pushing data to StoreAssets on java side)");
		}

		protected override void loadNativeFromDB() {
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniStoreInfoClass = new AndroidJavaClass("com.soomla.store.data.StoreInfo")) {
				jniStoreInfoClass.CallStatic<bool>("loadFromDB");
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

#endif
	}
}

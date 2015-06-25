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
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace Soomla.Store
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class StoreManifestTools : ISoomlaManifestTools
    {
#if UNITY_EDITOR
		static StoreManifestTools instance = new StoreManifestTools();
		static StoreManifestTools()
		{
			SoomlaManifestTools.ManTools.Add(instance);
		}

		public void UpdateManifest() {
			if (StoreSettings.GPlayBP) {
				SoomlaManifestTools.SetPermission("com.android.vending.BILLING");
				SoomlaManifestTools.AddActivity("com.soomla.store.billing.google.GooglePlayIabService$IabActivity",
				                             new Dictionary<string, string>() { 
													{"theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen"} 
											 });
				SoomlaManifestTools.AddMetaDataTag("billing.service", "google.GooglePlayIabService");
			} else {
				// removing BILLING permission
				SoomlaManifestTools.RemovePermission("com.android.vending.BILLING");
				// removing Iab Activity
				SoomlaManifestTools.RemoveActivity("com.soomla.store.billing.google.GooglePlayIabService$IabActivity");
			}

			if (StoreSettings.TapClashBP) {
				SoomlaManifestTools.SetPermission("com.gametree.vending.BILLING");
				SoomlaManifestTools.AddActivity("com.soomla.store.billing.tapclash.TapClashIabService$IabActivity",
				                                new Dictionary<string, string>() { 
					{"theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen"} 
				});
				SoomlaManifestTools.AddMetaDataTag("billing.service", "tapclash.TapClashIabService");
			} else {
				// removing BILLING permission
				SoomlaManifestTools.RemovePermission("com.gametree.vending.BILLING");
				// removing Iab Activity
				SoomlaManifestTools.RemoveActivity("com.soomla.store.billing.tapclash.TapClashIabService$IabActivity");
			}

		
			if (StoreSettings.AmazonBP) {
				XmlElement receiverElement = SoomlaManifestTools.AppendApplicationElement("receiver", "com.amazon.device.iap.ResponseReceiver", null);
				receiverElement.InnerText = "\n    ";
				XmlElement intentElement = SoomlaManifestTools.AppendElementIfMissing("intent-filter", null, null, receiverElement);
				XmlElement actionElement = SoomlaManifestTools.AppendElementIfMissing("action", "com.amazon.inapp.purchasing.NOTIFY", 
                                                new Dictionary<string, string>() {
													{"permission", "com.amazon.inapp.purchasing.Permission.NOTIFY"}
												}, 
												intentElement);
				actionElement.InnerText = "\n    ";
				SoomlaManifestTools.AddMetaDataTag("billing.service", "amazon.AmazonIabService");
			} else {
				SoomlaManifestTools.RemoveApplicationElement("receiver", "com.amazon.inapp.purchasing.ResponseReceiver");
			}
		}

#endif
	}
}
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

			if (StoreSettings.AmazonBP) {
				XmlElement receiverElement = SoomlaManifestTools.AppendApplicationElement("receiver", "com.amazon.inapp.purchasing.ResponseReceiver", null);
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
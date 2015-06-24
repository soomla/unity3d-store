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
using System.IO;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soomla.Store
{

#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class StoreSettings : ISoomlaSettings
	{

#if UNITY_EDITOR

		static StoreSettings instance = new StoreSettings();
		static StoreSettings()
		{
			SoomlaEditorScript.addSettings(instance);
		}

		bool showAndroidSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android);
		bool showIOSSettings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone);

		GUIContent noneBPLabel = new GUIContent("You have your own Billing Service");
		GUIContent playLabel = new GUIContent("Google Play");
		GUIContent amazonLabel = new GUIContent("Amazon");
		GUIContent bazaarLabel = new GUIContent("Bazaar");
		GUIContent publicKeyLabel = new GUIContent("API Key [?]:", "The API key from Google Play dev console (just in case you're using Google Play as billing provider).");
		GUIContent bazaar_publicKeyLabel = new GUIContent("API Key [?]:", "The API key from Bazaar dev console (just in case you're using Bazaar as billing provider).");
		GUIContent testPurchasesLabel = new GUIContent("Test Purchases [?]:", "Check if you want to allow purchases of Google's test product ids.");
		GUIContent bazaar_testPurchasesLabel = new GUIContent("Test Purchases [?]:", "Check if you want to allow purchases of Bazaar's test product ids.");
		GUIContent packageNameLabel = new GUIContent("Package Name [?]", "Your package as defined in Unity.");

		GUIContent iosSsvLabel = new GUIContent("Receipt Validation [?]:", "Check if you want your purchases validated with SOOMLA Server Side Protection Service.");

		GUIContent frameworkVersion = new GUIContent("Store Version [?]", "The SOOMLA Framework Store Module version. ");
		GUIContent buildVersion = new GUIContent("Store Build [?]", "The SOOMLA Framework Store Module build.");

		public void OnEnable() {
			// Generating AndroidManifest.xml
//			ManifestTools.GenerateManifest();
		}

		public void OnModuleGUI() {
			AndroidGUI();
			EditorGUILayout.Space();
			IOSGUI();
		}

		public void OnInfoGUI() {
			SoomlaEditorScript.SelectableLabelField(frameworkVersion, "1.7.16");
			SoomlaEditorScript.SelectableLabelField(buildVersion, "1");
			EditorGUILayout.Space();
		}

		public void OnSoomlaGUI() {

		}

		private void IOSGUI()
		{
			showIOSSettings = EditorGUILayout.Foldout(showIOSSettings, "iOS Build Settings");
			if (showIOSSettings)
			{
				IosSSV = EditorGUILayout.Toggle(iosSsvLabel, IosSSV);
			}
			EditorGUILayout.Space();
		}

		private void AndroidGUI()
		{
			showAndroidSettings = EditorGUILayout.Foldout(showAndroidSettings, "Android Settings");
			if (showAndroidSettings)
			{
				EditorGUILayout.BeginHorizontal();
				SoomlaEditorScript.SelectableLabelField(packageNameLabel, PlayerSettings.bundleIdentifier);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("Billing Service Selection", MessageType.None);
	
				if (!GPlayBP && !AmazonBP && !BazaarBP && !NoneBP) {
					GPlayBP = true;
				}

				NoneBP = EditorGUILayout.ToggleLeft(noneBPLabel, NoneBP);

				bool update;
				bpUpdate.TryGetValue("none", out update);
				if (NoneBP && !update) {
					setCurrentBPUpdate("none");

					AmazonBP = false;
					GPlayBP = false;
					BazaarBP = false;
					SoomlaManifestTools.GenerateManifest();
					handlePlayBPJars(true);
					handleAmazonBPJars(true);
					handleBazaarBPJars(true);
				}


				GPlayBP = EditorGUILayout.ToggleLeft(playLabel, GPlayBP);

				if (GPlayBP) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(publicKeyLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
					AndroidPublicKey = EditorGUILayout.TextField(AndroidPublicKey, SoomlaEditorScript.FieldHeight);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
					AndroidTestPurchases = EditorGUILayout.Toggle(testPurchasesLabel, AndroidTestPurchases);
					EditorGUILayout.EndHorizontal();
				}

				bpUpdate.TryGetValue("play", out update);
				if (GPlayBP && !update) {
					setCurrentBPUpdate("play");

					AmazonBP = false;
					NoneBP = false;
					BazaarBP = false;
					SoomlaManifestTools.GenerateManifest();
					handlePlayBPJars(false);
					handleAmazonBPJars(true);
					handleBazaarBPJars(true);
				}

				BazaarBP = EditorGUILayout.ToggleLeft(bazaarLabel, BazaarBP);
				
				if (BazaarBP) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(bazaar_publicKeyLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
					BazaarPublicKey = EditorGUILayout.TextField(BazaarPublicKey, SoomlaEditorScript.FieldHeight);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
					BazaarTestPurchases = EditorGUILayout.Toggle(bazaar_testPurchasesLabel, BazaarTestPurchases);
					EditorGUILayout.EndHorizontal();
				}
				
				bpUpdate.TryGetValue("bazaar", out update);
				if (BazaarBP && !update) {
					setCurrentBPUpdate("bazaar");
					
					AmazonBP = false;
					NoneBP = false;
					GPlayBP = false;
					SoomlaManifestTools.GenerateManifest();
					handleBazaarBPJars(false);
					handlePlayBPJars(true);
					handleAmazonBPJars(true);
				}

				AmazonBP = EditorGUILayout.ToggleLeft(amazonLabel, AmazonBP);
				bpUpdate.TryGetValue("amazon", out update);
				if (AmazonBP && !update) {
					setCurrentBPUpdate("amazon");

					GPlayBP = false;
					NoneBP = false;
					BazaarBP = false;
					SoomlaManifestTools.GenerateManifest();
					handlePlayBPJars(true);
					handleAmazonBPJars(false);
					handleBazaarBPJars(true);
				}
			}
			EditorGUILayout.Space();
		}







		/** Billing Providers util functions **/

		private void setCurrentBPUpdate(string bpKey) {
			bpUpdate[bpKey] = true;
			var buffer = new List<string>(bpUpdate.Keys);
			foreach(string key in buffer) {
				if (key != bpKey) {
					bpUpdate[key] = false;
				}
			}
		}

		private Dictionary<string, bool> bpUpdate = new Dictionary<string, bool>();
		private static string bpRootPath = Application.dataPath + "/WebPlayerTemplates/SoomlaConfig/android/android-billing-services/";

		public static void handlePlayBPJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreGooglePlay.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreGooglePlay.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(bpRootPath + "google-play/AndroidStoreGooglePlay.jar",
					                             Application.dataPath + "/Plugins/Android/AndroidStoreGooglePlay.jar");
				}
			}catch {}
		}

 		public static void handleBazaarBPJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreBazaar.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreBazaar.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(bpRootPath + "bazaar/AndroidStoreBazaar.jar",
					                             Application.dataPath + "/Plugins/Android/AndroidStoreBazaar.jar");
				}
			}catch {}
		}

		public static void handleAmazonBPJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreAmazon.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/AndroidStoreAmazon.jar.meta");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/in-app-purchasing-2.0.1.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/in-app-purchasing-2.0.1.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(bpRootPath + "amazon/AndroidStoreAmazon.jar",
					                             Application.dataPath + "/Plugins/Android/AndroidStoreAmazon.jar");
					FileUtil.CopyFileOrDirectory(bpRootPath + "amazon/in-app-purchasing-2.0.1.jar",
					                             Application.dataPath + "/Plugins/Android/in-app-purchasing-2.0.1.jar");
				}
			}catch {}
		}



#endif







		/** Store Specific Variables **/


		public static string AND_PUB_KEY_DEFAULT = "YOUR GOOGLE PLAY PUBLIC KEY";

		public static string AndroidPublicKey
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("AndroidPublicKey", out value) ? value : AND_PUB_KEY_DEFAULT;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("AndroidPublicKey", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("AndroidPublicKey",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool AndroidTestPurchases
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("AndroidTestPurchases", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("AndroidTestPurchases", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("AndroidTestPurchases",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string BAZAAR_PUB_KEY_DEFAULT = "YOUR BAZAAR RSA KEY";
		
		public static string BazaarPublicKey
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("BazaarPublicKey", out value) ? value : BAZAAR_PUB_KEY_DEFAULT;
			}
			set 
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("BazaarPublicKey", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("BazaarPublicKey",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}
		
		public static bool BazaarTestPurchases
		{
			get { 
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("BazaarTestPurchases", out value) ? Convert.ToBoolean(value) : false;
			}
			set 
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("BazaarTestPurchases", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("BazaarTestPurchases",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool IosSSV
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("IosSSV", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("IosSSV", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("IosSSV",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool NoneBP
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("NoneBP", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("NoneBP", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("NoneBP",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool GPlayBP
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("GPlayBP", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("GPlayBP", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("GPlayBP",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool AmazonBP
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("AmazonBP", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("AmazonBP", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("AmazonBP",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool BazaarBP
		{
			get { 
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("BazaarBP", out value) ? Convert.ToBoolean(value) : false;
			}
			set 
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("BazaarBP", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("BazaarBP",value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}
	}
}

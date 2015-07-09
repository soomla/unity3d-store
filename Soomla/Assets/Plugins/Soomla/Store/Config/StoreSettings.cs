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
        	bool showWP8Settings = (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WP8Player);

		GUIContent noneBPLabel = new GUIContent("You have your own Billing Service");
		GUIContent playLabel = new GUIContent("Google Play");
		GUIContent playSsvLabel = new GUIContent("Receipt Validation [?]:", "Check if you want your purchases validated with SOOMLA Server Side Protection Service.");
		GUIContent playClientIdLabel = new GUIContent("Client ID");
		GUIContent playClientSecretLabel = new GUIContent("Client Secret");
		GUIContent playRefreshTokenLabel = new GUIContent("Refresh Token");
		GUIContent playVerifyOnServerFailureLabel = new GUIContent("Verify On Server Failure [?]:", "Check if you want your purchases get validated if server failure happens.");


		GUIContent amazonLabel = new GUIContent("Amazon");
		GUIContent publicKeyLabel = new GUIContent("API Key [?]:", "The API key from Google Play dev console (just in case you're using Google Play as billing provider).");
		GUIContent testPurchasesLabel = new GUIContent("Test Purchases [?]:", "Check if you want to allow purchases of Google's test product ids.");
		GUIContent packageNameLabel = new GUIContent("Package Name [?]", "Your package as defined in Unity.");
        	GUIContent wp8SimulatorModeLabel = new GUIContent("Run in Simulator (x86 build)");
        	GUIContent wp8TestModeLabel = new GUIContent("Simulate Store. (Don't forget to adapt IAPMock.xml to fit your IAPs)");

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
            		EditorGUILayout.Space();
            		WP8GUI();
		}

		public void OnInfoGUI() {
			SoomlaEditorScript.SelectableLabelField(frameworkVersion, "1.8.0");
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

				if (!GPlayBP && !AmazonBP && !NoneBP) {
					GPlayBP = true;
				}

				NoneBP = EditorGUILayout.ToggleLeft(noneBPLabel, NoneBP);

				bool update;
				bpUpdate.TryGetValue("none", out update);
				if (NoneBP && !update) {
					setCurrentBPUpdate("none");

					AmazonBP = false;
					GPlayBP = false;
					SoomlaManifestTools.GenerateManifest();
					handlePlayBPJars(true);
					handleAmazonBPJars(true);
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

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
					PlaySsvValidation = EditorGUILayout.Toggle(playSsvLabel, PlaySsvValidation);
					EditorGUILayout.EndHorizontal();

					if (PlaySsvValidation) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						EditorGUILayout.LabelField(playClientIdLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
						PlayClientId = EditorGUILayout.TextField(PlayClientId, SoomlaEditorScript.FieldHeight);
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						EditorGUILayout.LabelField(playClientSecretLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
						PlayClientSecret = EditorGUILayout.TextField(PlayClientSecret, SoomlaEditorScript.FieldHeight);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						EditorGUILayout.LabelField(playRefreshTokenLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
						PlayRefreshToken = EditorGUILayout.TextField(PlayRefreshToken, SoomlaEditorScript.FieldHeight);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.Space();
						EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
						PlayVerifyOnServerFailure = EditorGUILayout.Toggle(playVerifyOnServerFailureLabel, PlayVerifyOnServerFailure);
						EditorGUILayout.EndHorizontal();
					}
				}

				bpUpdate.TryGetValue("play", out update);
				if (GPlayBP && !update) {
					setCurrentBPUpdate("play");

					AmazonBP = false;
					NoneBP = false;
					SoomlaManifestTools.GenerateManifest();
					handlePlayBPJars(false);
					handleAmazonBPJars(true);
				}


				AmazonBP = EditorGUILayout.ToggleLeft(amazonLabel, AmazonBP);
				bpUpdate.TryGetValue("amazon", out update);
				if (AmazonBP && !update) {
					setCurrentBPUpdate("amazon");

					GPlayBP = false;
					NoneBP = false;
					SoomlaManifestTools.GenerateManifest();
					handlePlayBPJars(true);
					handleAmazonBPJars(false);
				}
			}
			EditorGUILayout.Space();
		}


        private void WP8GUI()
        {
            showWP8Settings = EditorGUILayout.Foldout(showWP8Settings, "WP8 Settings");
            if (showWP8Settings)
            {
                WP8SimulatorBuild = EditorGUILayout.ToggleLeft(wp8SimulatorModeLabel, WP8SimulatorBuild);
                EditorGUILayout.Space();
                WP8TestMode = EditorGUILayout.ToggleLeft(wp8TestModeLabel, WP8TestMode);
            }
            
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
        private static string wp8RootPath = Application.dataPath + "/WebPlayerTemplates/SoomlaConfig/wp8/";

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

		public static string PLAY_CLIENT_ID_DEFAULT = "YOUR CLIENT ID";
		public static string PLAY_CLIENT_SECRET_DEFAULT = "YOUR CLIENT SECRET";
		public static string PLAY_REFRESH_TOKEN_DEFAULT = "YOUR REFRESH TOKEN";

		
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

		public static string PlayClientId
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayClientId", out value) ? value : PLAY_CLIENT_ID_DEFAULT;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayClientId", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("PlayClientId",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}
		
		public static string PlayClientSecret
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayClientSecret", out value) ? value : PLAY_CLIENT_SECRET_DEFAULT;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayClientSecret", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("PlayClientSecret",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}
		
		public static string PlayRefreshToken
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayRefreshToken", out value) ? value : PLAY_REFRESH_TOKEN_DEFAULT;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayRefreshToken", out v);
				if (v != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("PlayRefreshToken",value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool PlayVerifyOnServerFailure
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayVerifyOnServerFailure", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlayVerifyOnServerFailure", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("PlayVerifyOnServerFailure", value.ToString());
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

		public static bool PlaySsvValidation
		{
			get {
				string value;
				return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlaySsvValidation", out value) ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v;
				SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("PlaySsvValidation", out v);
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.Instance.setSettingsValue("PlaySsvValidation",value.ToString());
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

        public static bool WP8SimulatorBuild
        {
            get
            {
                string value;
                return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("WP8SimulatorBuild", out value) ? Convert.ToBoolean(value) : false;
            }
            set
            {
                string v;
                SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("WP8SimulatorBuild", out v);
                if (Convert.ToBoolean(v) != value)
                {
                    SoomlaEditorScript.Instance.setSettingsValue("WP8SimulatorBuild", value.ToString());
                    SoomlaEditorScript.DirtyEditor();
#if UNITY_EDITOR
                    if (value == true)
                    {
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "x86/sqlite3.soomladll",Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "x86/Sqlite.soomladll",Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "x86/Sqlite.soomlawinmd",Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                    }
                    else
                    {
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "ARM/sqlite3.soomladll",Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "ARM/Sqlite.soomlawinmd",Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "ARM/Sqlite.soomladll",Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                    }
#endif
                }
            }
        }

        public static bool WP8TestMode
        {
            get
            {
                string value;
                return SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("WP8TestMode", out value) ? Convert.ToBoolean(value) : false;
            }
            set
            {
                string v;
                SoomlaEditorScript.Instance.SoomlaSettings.TryGetValue("WP8TestMode", out v);
                if (Convert.ToBoolean(v) != value)
                {
                    SoomlaEditorScript.Instance.setSettingsValue("WP8TestMode", value.ToString());
                    SoomlaEditorScript.DirtyEditor();
                }
            }
        }
	}
}

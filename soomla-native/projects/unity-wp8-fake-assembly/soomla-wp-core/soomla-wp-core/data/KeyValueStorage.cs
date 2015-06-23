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
/// 
using System;
using System.Collections.Generic;
using System.Linq;


namespace SoomlaWpCore.data
{
    public class KeyValueStorage
    {
        private static KeyValDatabase Kvd;
        private const String TAG = "SOOMLA KeyValueStorage"; //used for Log Messages
        private static Dictionary<string,string> cache;

        public KeyValueStorage()
        {
        }

        public static Dictionary<string, string> GetCache()
        {
            if (cache == null)
            {
                cache = new Dictionary<string, string>();
            }
            return cache;
        }
        
        public static String GetValue(String Key, bool EncryptedKey = true)
        {
            if (GetCache().Keys.Contains(Key))
            {
                return GetCache()[Key];
            }

            return null;
        }

        public static void SetValue(String Key, String Value, bool EncryptedKey = true)
        {
            GetCache()[Key] = Value;
            
        }

        public static int DeleteKeyValue(String key, bool EncryptedKey = true)
        {
            int c = 0;
            if (GetCache().Keys.Contains(key))
            {
                GetCache().Remove(key);
                c++;
            }
            return c;
        }

        public static KeyValDatabase GetDatabase()
        {
            if (Kvd == null)
            {
                Kvd = new KeyValDatabase();
            }
            return Kvd;
        }

        public static String GetNonEncryptedKeyValue(String Key)
        {
            return "";
        }

        public static List<KeyValue> GetNonEncryptedQueryValues(String query)
        {
            return new List<KeyValue>();
        }

        public static String GetOneNonEncryptedQueryValues(String query)
        {
            return "";
        }

        public static int GetCountNonEncryptedQueryValues(String query)
        {
            return 0;
        }

        /// <summary>
        /// Gets all KeyValue keys in the storage with no encryption
        /// </summary>
        /// <returns></returns>
        public static List<KeyValue> GetEncryptedKeys()
        {
            return new List<KeyValue>();
        }

        public static void SetNonEncryptedKeyValue(String Key, String Value)
        {
            
        }
    }
}

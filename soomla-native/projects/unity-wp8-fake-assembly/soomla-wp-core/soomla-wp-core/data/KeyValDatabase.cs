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
using System.Collections.Generic;
using System.Threading;

namespace SoomlaWpCore.data
{
    public class KeyValDatabase
    {

        private const String TAG = "SOOMLA KeyValDatabase"; //used for Log messages

        private Dictionary<string,string> database;

        public KeyValDatabase()
        {
            database = new Dictionary<string,string>();
        }

        public void SetKeyVal(String Key, String Value)
        {
            database[Key] = Value;
        }

        public String GetKeyVal(String Key)
        {
            return database[Key];
        }

        public void DeleteKeyVal(String Key)
        {
            database.Remove(Key);
        }

        public void PurgeDatabse()
        {
            
        }

        public void PurgeDatabaseEntries()
        {
            
        }

        public List<KeyValue> GetQueryVals(String query)
        {
            
            return new List<KeyValue>();
        }

        public string GetQueryOne(String query)
        {
            return "";
        }

        public int GetQueryCount(String query)
        {
            return 0;
        }

        /// <summary>
        /// Get all Keys from KeyValue without the Values
        /// </summary>
        /// <returns></returns>
        public List<KeyValue> GetAllKeys()
        {
            return new List<KeyValue>();
        }

    }

    public sealed class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

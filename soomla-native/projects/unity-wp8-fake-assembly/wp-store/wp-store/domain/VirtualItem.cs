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
using System.Linq;
using System.Text;
using SoomlaWpCore.data;
using SoomlaWpCore;
//using Newtonsoft.Json.Linq;
namespace SoomlaWpStore.domain
{
    public abstract class VirtualItem : SoomlaEntity<VirtualItem>
    {
        public VirtualItem(String Name, String Description, String ItemId) : base(Name, Description, ItemId)
        {

        }

        public VirtualItem(object jsonObject) : base(jsonObject)
        {
        }

        /*public virtual new object toJSONObject()
        {
            return base.toJSONObject();
        }*/

        public int give(int amount)
        {
            return give(amount, true);
        }

        public abstract int give(int amount, bool notify);

        public int take(int amount)
        {
            return take(amount, true);
        }

        public abstract int take(int amount, bool notify);

        public int resetBalance(int balance)
        {
            return resetBalance(balance, true);
        }

        public abstract int resetBalance(int balance, bool notify);

        public String getItemId()
        {
            return mID;
        }

        public String getName()
        {
            return mName;
        }

        private const String TAG = "SOOMLA VirtualItem"; //used for Log messages
    }
}

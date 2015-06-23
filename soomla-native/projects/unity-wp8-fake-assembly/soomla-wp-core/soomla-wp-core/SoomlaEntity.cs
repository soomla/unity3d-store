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
using SoomlaWpCore.data;
using SoomlaWpCore.util;
////using Newtonsoft.Json.Linq;
namespace SoomlaWpCore
{
    public abstract class SoomlaEntity<T>
    {
        public SoomlaEntity(String Name, String Description, String ID)
        {
            mName = Name;
            mDescription = Description;
            mID = ID.Trim();
        }

        public SoomlaEntity(object jsonObject) {
            /*mName = jsonObject.Value<string>(JSONConsts.SOOM_ENTITY_NAME);
            mDescription = jsonObject.Value<string>(JSONConsts.SOOM_ENTITY_DESCRIPTION);
            mID = jsonObject.Value<string>(JSONConsts.SOOM_ENTITY_ID);*/
        }

        public virtual JSONObject toJSONObject()
        {
            if (mID == null)
            {
                SoomlaUtils.LogError(TAG, "This is BAD! We don't have ID in the this SoomlaEntity. Stopping here.");
                return null;
            }
            JSONObject jsonObject = new JSONObject();
            /*jsonObject[JSONConsts.SOOM_ENTITY_NAME] = mName;
            jsonObject[JSONConsts.SOOM_ENTITY_DESCRIPTION] = mDescription;
            jsonObject[JSONConsts.SOOM_ENTITY_ID] = mID;*/
           
            return jsonObject;
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            SoomlaEntity<T> g = obj as SoomlaEntity<T>;
            if ((System.Object)g == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (mID == g.mID);
        }

        public bool Equals(SoomlaEntity<T> g)
        {
            // If parameter is null return false:
            if ((object)g == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (mID == g.mID);
        }

        public override int GetHashCode()
        {
            return mID.GetHashCode();
        }

        public static bool operator ==(SoomlaEntity<T> a, SoomlaEntity<T> b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.mID == b.mID;
        }

        public static bool operator !=(SoomlaEntity<T> a, SoomlaEntity<T> b)
        {
            return !(a == b);
        }

        public virtual T Clone(string newId)
        {
            JSONObject obj = this.toJSONObject();
            obj.SetField(JSONConsts.SOOM_ENTITY_ID, JSONObject.CreateStringObject(newId));
            return (T)Activator.CreateInstance(this.GetType(), new object[] { obj });
        }

        public int hashCode()
        {
            return mID != null ? mID.GetHashCode() : 0;
        }

        private const String TAG = "SOOMLA SoomlaEntity"; //used for Log messages

        public String GetName()
        {
            return mName;
        }

        public String GetDescription()
        {
            return mDescription;
        }

        public String GetId()
        {
            return mID;
        }

        protected String mName;
        protected String mDescription;
        protected String mID;
    }
}

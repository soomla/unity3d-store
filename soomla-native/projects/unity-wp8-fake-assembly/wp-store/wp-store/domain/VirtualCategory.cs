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
using System.Collections;
using System.Collections.Generic;
using SoomlaWpCore;
using SoomlaWpStore.data;
//using Newtonsoft.Json.Linq;
namespace SoomlaWpStore.domain
{
/**
 * This class is a definition of a category. A single category can be associated with many virtual
 * goods. You can use it to arrange virtual goods to their specific categories.
 */
public class VirtualCategory {

    /**
     * Constructor.
     *
     * @param name the category's name
     * @param goodsItemIds list of item ids of the virtual goods in this category
     */
    public VirtualCategory(String name, List<String> goodsItemIds) {
        mName = name;
        mGoodsItemIds = goodsItemIds;
    }

    /**
     * Constructor.
     * Generates an instance of <code>VirtualCategory</code> from the given <code>JSONObject</code>.
     *
     * @param jsonObject A JSONObject representation of the wanted <code>VirtualCategory</code>.
     * @throws JSONException
     */
    public VirtualCategory(object jsonObject) {
        
    }

    /**
     * Converts the current <code>VirtualCategory</code> to a <code>JSONObject</code>.
     *
     * @return A JSONObject representation of the current <code>VirtualCategory</code>.
     */
    public object toJSONObject(){
        return new object();
    }


    /** Setters and Getters **/

    public String getName() {
        return mName;
    }

    public List<String> getGoodsItemIds() {
        return mGoodsItemIds;
    }


    /** Private members **/

    private const String TAG = "SOOMLA VirtualCategory"; //used for Log messages

    //the list of itemIds of the VirtualGoods in this category
    private List<String> mGoodsItemIds = new List<String>();

    private String  mName; //the category's name

}
}
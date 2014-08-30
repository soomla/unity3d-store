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
using System.Collections.Generic;
using System;
using System.Collections;

namespace Soomla.Store {

	/// <summary>
	/// This class is a definition of a category. A single category can be associated with many virtual goods.
 	/// The purposes of virtual categories are:
 	/// 1. You can use it to arrange virtual goods to their specific categories.
 	/// 2. SOOMLA's storefront uses this to show the goods in their categories on the UI (for supported themes only).
	/// </summary>
	public class VirtualCategory {
		
		private const string TAG = "SOOMLA VirtualCategory";
		public string Name;
		public List<String> GoodItemIds = new List<String>();
		
		/// <summary>
		/// Constructor. 
		/// </summary>
		/// <param name="name">Name of category.</param>
		/// <param name="goodItemIds">List of item ids of the virtual goods in this category.</param>
		public VirtualCategory(string name, List<String> goodItemIds){
			this.Name = name;
			this.GoodItemIds = goodItemIds;
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public VirtualCategory(AndroidJavaObject jniVirtualCategory) {
			this.Name = jniVirtualCategory.Call<string>("getName");
			
			using(AndroidJavaObject jniItemIds = jniVirtualCategory.Call<AndroidJavaObject>("getGoodsItemIds")) {
				for(int i=0; i<jniItemIds.Call<int>("size"); i++) {
					using(AndroidJavaObject jniItemId = jniItemIds.Call<AndroidJavaObject>("get", i)) {
						GoodItemIds.Add(jniItemId.Call<string>("toString"));
					}
				}
			}
		}
#endif

		/// <summary>
		/// Constructor.
		/// Generates an instance of <c>VirtualCategory</c> from the given <c>JSONObject</c>.
		/// </summary>
		/// <param name="jsonItem">A JSONObject representation of the wanted <c>VirtualCategory</c>.</param>
		public VirtualCategory(JSONObject jsonItem) {
			this.Name = jsonItem[JSONConsts.CATEGORY_NAME].str;

	        JSONObject goodsArr = (JSONObject)jsonItem[JSONConsts.CATEGORY_GOODSITEMIDS];
			
	        foreach(JSONObject obj in goodsArr.list) {
	            GoodItemIds.Add(obj.str);
	        }
		}
		
		/// <summary>
		/// Converts the current <c>VirtualCategory</c> to a <c>JSONObject</c>.
		/// </summary>
		/// <returns>A JSONObject representation of the current <c>VirtualCategory</c>.</returns>
		public JSONObject toJSONObject() {
			JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
			obj.AddField(JSONConsts.CATEGORY_NAME, this.Name);
			
			JSONObject goodsArr = new JSONObject(JSONObject.Type.ARRAY);
			foreach(string goodItemId in this.GoodItemIds) {
				goodsArr.Add(goodItemId);
			}
			
			obj.AddField(JSONConsts.CATEGORY_GOODSITEMIDS, goodsArr);
			
			return obj;
		}

	}
}

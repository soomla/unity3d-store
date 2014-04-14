/*
 * Copyright (C) 2012 Soomla Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using UnityEngine;
using System.Collections;

namespace Soomla{
	/// <summary>
	/// This is a representation of a game's virtual currency.
	/// Each game can have multiple instances of a virtual currency, all kept in StoreInfo;
	/// </summary>
	public class VirtualCurrency : VirtualItem{
		
		/// <summary>
		/// Initializes a new instance of the <see cref="com.soomla.unity.VirtualCurrency"/> class.
		/// </summary>
		/// <param name='name'>
		/// see parent
		/// </param>
		/// <param name='description'>
		/// see parent
		/// </param>
		/// <param name='itemId'>
		/// see parent
		/// </param>
		public VirtualCurrency(string name, string description, string itemId)
			: base(name, description, itemId)
		{
		}
		
#if UNITY_ANDROID && !UNITY_EDITOR
		public VirtualCurrency(AndroidJavaObject jniVirtualCurrency) 
			: base(jniVirtualCurrency)
		{
		}
#endif
		/// <summary>
		/// see parent
		/// </summary>
		public VirtualCurrency(JSONObject jsonVc)
			: base(jsonVc)
		{
		}
		
		/// <summary>
		/// see parent
		/// </summary>
		public override JSONObject toJSONObject() {
			return base.toJSONObject();
		}
		
	}
}
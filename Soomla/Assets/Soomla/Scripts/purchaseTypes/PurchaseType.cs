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
using System;

namespace Soomla
{
	/// <summary>
	/// A PurchaseType is a way to purchase a PurchasableVirtualItem. This abstract class describes basic features
 	/// of the actual implementations of PurchaseType.
	/// </summary>
	public abstract class PurchaseType
	{
		public PurchaseType ()
		{
		}

#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
		public abstract void Buy(string itemId);
		public abstract void Success(string itemId);
#endif
	}
	
	
}


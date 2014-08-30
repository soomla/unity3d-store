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

namespace Soomla.Store {

	/// <summary>
	/// This exception is thrown when looking for a <c>VirtualItem</c> that cannot be found.
	/// </summary>
	public class VirtualItemNotFoundException : Exception {

		/// <summary>
		/// Constructor.
		/// </summary>
		public VirtualItemNotFoundException ()
			: base("Virtual item was not found !")
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="lookupBy">The field of the virtual item to look for.</param>
		/// <param name="lookupVal">The value of the field to look for.</param>
		public VirtualItemNotFoundException (string lookupBy, string lookupVal)
			: base("Virtual item was not found when searching with " + lookupBy + "=" + lookupVal)
		{
		}
	}
}


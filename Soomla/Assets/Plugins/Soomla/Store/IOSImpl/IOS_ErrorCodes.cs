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
using UnityEngine;

namespace Soomla.Store {

	/// <summary>
	/// This class provides error codes for each of the errors available in iOS-store. 
	/// </summary>
	public static class IOS_ErrorCodes {

		public static int NO_ERROR = 0;

		public static int EXCEPTION_ITEM_NOT_FOUND = -101;

		public static int EXCEPTION_INSUFFICIENT_FUNDS = -102;

		public static int EXCEPTION_NOT_ENOUGH_GOODS = -103;

		/// <summary>
		/// Checks the error code and throws the relevant exception.
		/// </summary>
		/// <param name="error">Error code.</param>
		public static void CheckAndThrowException(int error) {
			if (error == EXCEPTION_ITEM_NOT_FOUND) {
				Debug.Log("SOOMLA/UNITY Got VirtualItemNotFoundException exception from 'extern C'");
				throw new VirtualItemNotFoundException();
			} 
			
			if (error == EXCEPTION_INSUFFICIENT_FUNDS) {
				Debug.Log("SOOMLA/UNITY Got InsufficientFundsException exception from 'extern C'");
				throw new InsufficientFundsException();
			} 
			
			if (error == EXCEPTION_NOT_ENOUGH_GOODS) {
				Debug.Log("SOOMLA/UNITY Got NotEnoughGoodsException exception from 'extern C'");
				throw new NotEnoughGoodsException();
			}
		}
	}
}


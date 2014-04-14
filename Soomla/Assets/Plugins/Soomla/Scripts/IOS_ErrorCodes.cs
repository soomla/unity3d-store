using System;
using UnityEngine;

namespace Soomla
{
	public static class IOS_ErrorCodes
	{
		public static int NO_ERROR = 0;
		public static int EXCEPTION_ITEM_NOT_FOUND = -1;
		public static int EXCEPTION_INSUFFICIENT_FUNDS = -2;
		public static int EXCEPTION_NOT_ENOUGH_GOODS = -3;
		
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


using System;

namespace Soomla
{
	public class NotEnoughGoodsException : Exception
	{
		public NotEnoughGoodsException()
			:base("You tried to equip virtual good but you don't have any of it.")
		{
		}
		
		public NotEnoughGoodsException (string itemId)
			:base("You tried to equip virtual good with itemId: " + itemId + " but you don't have any of it.")
		{
		}
		
		
	}
}


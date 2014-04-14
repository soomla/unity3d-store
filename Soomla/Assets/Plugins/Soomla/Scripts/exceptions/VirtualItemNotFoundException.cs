using System;

namespace Soomla
{
	public class VirtualItemNotFoundException : Exception
	{
		public VirtualItemNotFoundException ()
			: base("Virtual item was not found !")
		{
		}
		
		public VirtualItemNotFoundException (string lookupBy, string lookupVal)
			: base("Virtual item was not found when searching with " + lookupBy + "=" + lookupVal)
		{
		}
	}
}


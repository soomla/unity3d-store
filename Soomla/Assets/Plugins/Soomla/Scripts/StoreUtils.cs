using UnityEngine;

namespace Soomla
{
	public static class StoreUtils
	{
		public static void LogDebug(string tag, string message)
		{
			if (Debug.isDebugBuild) {
				Debug.Log(string.Format("{0} {1}", tag, message));
			}
		}
		
		public static void LogError(string tag, string message) {
			Debug.LogError(string.Format("{0} {1}", tag, message));
		}
	}
}


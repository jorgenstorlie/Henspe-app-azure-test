using System;

namespace Henspe.iOS.Util
{
	public class DebugUtil
	{
		public DebugUtil () {
		}

		public static void ShowDebugTime(string message) 
		{
			String timeStr = DateTime.Now.ToString("hh.mm.ss.ffffff");
			Console.WriteLine (message + ": " + timeStr);
		}
	}
}
using System;
using Foundation;

namespace Henspe.iOS.Util
{
	public class LangUtil
	{
		public LangUtil () 
		{
		}

		public static string Get(string key)
		{
            string value = NSBundle.MainBundle.GetLocalizedString(key);
            return value;
		}
	}
}
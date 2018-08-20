using System;
using UIKit;

namespace Henspe.iOS.Util
{
	public class ErrorUtil
	{
		public ErrorUtil () 
		{
		}

		// Must be run on mainthread
		public static void ShowError(string error) 
		{
            UIAlertView alert = new UIAlertView (LangUtil.Get("Alert.Title.Error"), 
				error,
				null,
                LangUtil.Get ("Alert.OK"), 
				null);

			alert.Show ();
		}
	}
}
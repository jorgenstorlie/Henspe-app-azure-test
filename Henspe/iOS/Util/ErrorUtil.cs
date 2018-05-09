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
			UIAlertView alert = new UIAlertView (Foundation.NSBundle.MainBundle.LocalizedString ("Alert.Title.Error", null), 
				error,
				null,
				Foundation.NSBundle.MainBundle.LocalizedString ("Alert.OK", null), 
				null);

			alert.Show ();
		}
	}
}
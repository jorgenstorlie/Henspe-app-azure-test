using System;
using UIKit;

namespace Henspe.iOS.Util
{
	public class DeviceUtil
	{
		public DeviceUtil () 
		{
		}

		// Must be run on mainthread
		public static bool isIpad() 
		{
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				return true;
			else
				return false;
		}

		// Must be run on mainthread
		public static bool IsIPhone5() 
		{
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && UIScreen.MainScreen.Bounds.Size.Height
				* UIScreen.MainScreen.Scale >= 1136)
				return true;
			else
				return false;
		}

		// Must be run on mainthread
		public static bool IsIPhoneOlderThanIPhone5() 
		{
			if(UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && UIScreen.MainScreen.Bounds.Size.Height
				* UIScreen.MainScreen.Scale < 1100)
				return true;
			else
				return false;
		}

        public static bool IsIPhoneX()
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                if ((UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale) == 2436)
                {
                    return true;
                }
            }

            return false;
        }
	}
}
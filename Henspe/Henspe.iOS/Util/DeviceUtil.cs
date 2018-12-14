using System;
using CoreTelephony;
using ObjCRuntime;
using UIKit;

namespace Henspe.iOS.Util
{
	public class DeviceUtil
	{
		public DeviceUtil () 
		{
		}

        static public bool CanDevicePlaceCall()
        {
            if (UIApplication.SharedApplication.CanOpenUrl(new Foundation.NSUrl("tel://")))
            {
                CTTelephonyNetworkInfo networkInfo = new CTTelephonyNetworkInfo();
                CTCarrier ctCarrier = networkInfo.SubscriberCellularProvider;
                string mnc = ctCarrier.MobileNetworkCode;
                if (mnc == null || mnc.Length == 0 || mnc.Equals("65535"))
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }

        static public bool IsRunningOnSimulator()
        {
            if (ObjCRuntime.Runtime.Arch == Arch.SIMULATOR)
                return true;
            else
                return false;
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

        static public bool HasExtraTop()
        {
            // Like iPhone X
            nfloat top = 0;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                // iOS 11 or later
                if (UIApplication.SharedApplication.KeyWindow != null && UIApplication.SharedApplication.KeyWindow.SafeAreaInsets != null)
                {
                    top = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Top;
                }
            }

            if (top > 0)
                return true;
            else
                return false;
        }
    }
}
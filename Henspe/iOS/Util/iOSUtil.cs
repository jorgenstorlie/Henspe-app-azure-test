using System;
using UIKit;

namespace Henspe.iOS.Util
{
	public class iOSUtil
	{
		public iOSUtil () 
		{
		}

		// Must be run on mainthread
		public static Version GetVersion() 
		{
			Version v = new Version(UIDevice.CurrentDevice.SystemVersion);
			return v;
		}
	}
}
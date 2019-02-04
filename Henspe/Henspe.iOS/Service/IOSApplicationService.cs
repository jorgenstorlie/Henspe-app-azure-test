using System;
using Foundation;
using SNLA.Core.Service;

namespace Henspe.iOS
{
	public class IOSApplicationService : ApplicationService
	{
		public IOSApplicationService() : base() {}

		protected override void GetVersion()
		{
			base.GetVersion();

			NSObject thisVersionObject = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString");
			version = thisVersionObject.ToString();
		}
	}
}

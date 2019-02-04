using Foundation;
using SNLA.Core.Util;
using SNLA.Core.Service;

namespace Henspe.iOS
{
	public class IOSApplicationService : ApplicationService
	{
		public IOSApplicationService() : base() {}

		public string prodUrlString = "https://snla-apps.no/apps/henspe/";
		public string testUrlTest = "https://snla-apps.no/apps/henspetest/";
		public string plistFile = "Henspe.plist";

		protected override void GetVersion()
		{
			NSObject thisVersionObject = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString");
			string thisVersionString = thisVersionObject.ToString();
			version = ConvertUtil.ConvertStringToFloat(thisVersionString);
		}
	}
}

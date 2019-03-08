using SNLA.Core.Service;

namespace Henspe.iOS
{
	public class IOSApplicationService : ApplicationService
    {
        public string prodUrlString = "https://snla-apps.no/apps/henspe/";
		public string testUrlTest = "https://snla-apps.no/apps/henspetest/";
		public string plistFile = "Henspe.plist";

        public IOSApplicationService(string baseUrl, string databaseName = "snladata") : base(baseUrl, databaseName)
        {
        }

	}
}

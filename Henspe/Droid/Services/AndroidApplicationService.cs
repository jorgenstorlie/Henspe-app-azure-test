using SNLA.Core.Service;
namespace Henspe.Droid.Services

{
    public class AndroidApplicationService : ApplicationService
    {
        public string prodUrlString = "https://snla-apps.no/apps/henspe/";
        public string testUrlTest = "https://snla-apps.no/apps/henspetest/";
        public string plistFile = "Henspe.plist";

        public AndroidApplicationService(string baseUrl, string databaseName = "snladata") : base(baseUrl, databaseName)
        {
        }
    }
}

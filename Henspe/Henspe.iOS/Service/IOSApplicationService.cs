using SNLA.Core.Service;

namespace Henspe.iOS
{
	public class IOSApplicationService : ApplicationService
    {
        public IOSApplicationService(string baseUrl, string plistLocation, string plistName, string databaseName = "snladata") : base(baseUrl, plistLocation, plistName, databaseName)
        {
        }

	}
}

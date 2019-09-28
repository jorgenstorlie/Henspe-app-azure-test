using Henspe.Core.Services;

namespace Henspe.Droid.Services
{
    public class AndroidApplicationService : ApplicationService
    {
		public AndroidApplicationService(string databaseName = "snladata") : base()
        {
			CoordinateService = new CoordinateService();
        }
    }
}

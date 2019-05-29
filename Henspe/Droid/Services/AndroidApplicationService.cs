using SNLA.Core.Service;
namespace Henspe.Droid.Services

{
    public class AndroidApplicationService : ApplicationService
    {
        public AndroidApplicationService(string baseUrl, string plistLocation, string plistName, string databaseName = "snladata") : base(baseUrl, plistLocation, plistName, databaseName)
        {
        }
    }
}

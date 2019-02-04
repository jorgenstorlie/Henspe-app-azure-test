using System.Collections.Generic;
using System.Threading.Tasks;
using Henspe.Core.Model;

namespace Henspe.Core.Communication
{
    public interface IHjertestarterService
    {
        Task<bool> SyncHjertestartersSinceLastSync();
        IEnumerable<Hjertestarter> GetAllHjertestarters();
        IEnumerable<Hjertestarter> GetAllHjertestartersInBox(double northEastLat, double northEastLong, double southWestLat, double southWestLong);
        HjertestartersInBoxIfNeededResultDto GetAllHjertestartersInBoxIfNeeded(double northEastLat, double northEastLong, double southWestLat, double southWestLong, double lastNorthEastLat, double lastNorthEastLong, double lastSouthWestLat, double lastSouthWestLong);
        bool IsPositionInsideSafeAreaForBox(double latitude, double longitude, double northEastLat, double northEastLong, double southWestLat, double southWestLong);
    }
}
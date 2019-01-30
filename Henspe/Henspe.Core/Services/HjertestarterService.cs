using System;
using Henspe.Core.Storage;
using System.Threading.Tasks;
using Henspe.Core.Communication.Dto;
using SNLA.Core.Util;
using System.Collections.Generic;
using Henspe.Core.Model;
using System.Linq;
using Xamarin.Essentials;
using static SNLA.Core.Util.UserUtil;

namespace Henspe.Core.Communication
{

    public enum HjertestarterServiceLanguageKey
    {
        Hjertestarter_NotActive,
        Hjertestarter_AlwaysOpen,
        Hjertestarter_ClosingSoon,
        Hjertestarter_OpenAtTime,
        Hjertestarter_Closed,
        Hjertestarter_ClosedOpensSoon
    }

    public class HjertestarterService : IHjertestarterService
    {
        private readonly CxHttpClient client;
        private readonly Repository repository;
        private readonly Settings settings;
        private readonly string version;
        private readonly string os;
        private CallHjertestarter callHjertestarter;
        private static double oneMinuteMilliseconds = 1000 * 60;
        private static double oneHourMilliseconds = oneMinuteMilliseconds * 60;
        private static double oneDayMilliseconds = oneHourMilliseconds * 24;
        private static double timeSevenDaysInMilliseconds = oneDayMilliseconds * 7;
        private static double radiusCached = 10000; // 50km radius
        private static double radiusInner = 5000; // 25km radius
        public Dictionary<string, string> translations = new Dictionary<string, string>();
        public static double RadiusCached => radiusCached;

        public enum GetHjertestartersResultStatus
        {
            failed,
            cacheOK,
            replaceValues
        };

        public HjertestarterService(CxHttpClient client, Repository repository, Settings settings, string version, string os)
        {
            this.client = client;
            this.repository = repository;
            this.settings = settings;
            this.version = version;
            this.os = os;

            callHjertestarter = new CallHjertestarter(client, repository, settings, version, os);
        }

        /// <summary>
        /// Sync all new definbrillators since last syncId to local database
        /// </summary>
        public async Task<bool> SyncHjertestartersSinceLastSync()
        {
            GetHjertestartersResultDto getHjertestartersResultDto = new GetHjertestartersResultDto();

            try
            {
                getHjertestartersResultDto = await callHjertestarter.GetHjertestartersSinceLastSync(Current.CameraLastSyncId);
                StoreDefibrillatorList(getHjertestartersResultDto);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void AddLanguageValue(HjertestarterServiceLanguageKey key, string value)
        {
            translations.Add(key.ToString(), value);
        }

        private bool StoreDefibrillatorList(GetHjertestartersResultDto getHjertestartersResultDto)
        {
            if (getHjertestartersResultDto != null && getHjertestartersResultDto.hjerte != null)
            {
                if (getHjertestartersResultDto.hjerte.Count() > 0)
                {
                    DeleteDefibrillatorsWithIds(getHjertestartersResultDto.slettes);
                    SaveDefibrillators(getHjertestartersResultDto);
                    Current.CameraLastSyncId = getHjertestartersResultDto.siste_synk_id;
                }

                return true;
            }

            return false;
        }

        private void DeleteDefibrillatorsWithIds(IEnumerable<double> slettes)
        {
            if (slettes != null && slettes.Any())
            {
                IEnumerable<double> slettesDictinct = slettes.Distinct();

                IEnumerable<Hjertestarter> toDelete = repository.GetItemList<Hjertestarter>()
                    .Join(slettesDictinct, defibrillator => defibrillator.assetId, defibrillatorDelete => defibrillatorDelete,
                    (defibrillator, defibrillatorDelete) => defibrillator
                );

                repository.DeleteItems(toDelete);
            }
        }

        private void SaveDefibrillators(GetHjertestartersResultDto getHjertestartersResultDto)
        {
            List<double> existingDefibrillatorsToDeleteList = new List<double>();

            foreach (Hjertestarter hjertestarter in getHjertestartersResultDto.hjerte)
                existingDefibrillatorsToDeleteList.Add(hjertestarter.assetId);

            DeleteDefibrillatorsWithIds(existingDefibrillatorsToDeleteList);
            repository.SaveItems(getHjertestartersResultDto.hjerte);
        }

        /// <summary>
        /// Returns all definbrillators from local database
        /// </summary>
        public IEnumerable<Hjertestarter> GetAllHjertestarters()
        {
            IEnumerable<Hjertestarter> hjertestarters = repository.GetItemList<Hjertestarter>();
            return hjertestarters;
        }

        /// <summary>
        /// Returns all definbrillators from local database within a box
        /// </summary>
        /// <param name="northEastLat">Latitude for upper right corner</param>
        /// <param name="northEastLong">Longitude for upper right corner</param>
        /// <param name="southWestLat">Latitude for bottom left corner</param>
        /// <param name="southWestLong">Longitude for bottom left corner</param>
        public IEnumerable<Hjertestarter> GetAllHjertestartersInBox(double northEastLat, double northEastLong, double southWestLat, double southWestLong)
        {
            IEnumerable<Hjertestarter> hjertestarters = repository.GetItemList<Hjertestarter>()
                .Where(x => x.siteLatitude >= southWestLat && x.siteLatitude <= northEastLat && x.siteLongitude >= southWestLong && x.siteLongitude <= northEastLong);
            return hjertestarters;
        }

        /// <summary>
        /// Returns all definbrillators from local database within a box
        /// </summary>
        /// <param name="northEastLat">Latitude for upper right corner</param>
        /// <param name="northEastLong">Longitude for upper right corner</param>
        /// <param name="southWestLat">Latitude for bottom left corner</param>
        /// <param name="southWestLong">Longitude for bottom left corner</param>
        /// <param name="lastNorthEastLat">Latitude for last upper right corner. This value comes in result and should be stored for next input. Send 0 if unknown</param>
        /// <param name="lastNorthEastLong">Longitude for last upper right corner. This value comes in result and should be stored for next input. Send 0 if unknown</param>
        /// <param name="lastSouthWestLat">Latitude for last bottom left corner. This value comes in result and should be stored for next input. Send 0 if unknown</param>
        /// <param name="lastSouthWestLong">Longitude for last bottom left corner. This value comes in result and should be stored for next input. Send 0 if unknown</param>
        public HjertestartersInBoxIfNeededResultDto GetAllHjertestartersInBoxIfNeeded(double northEastLat, double northEastLong, double southWestLat, double southWestLong, double lastNorthEastLat, double lastNorthEastLong, double lastSouthWestLat, double lastSouthWestLong)
        {
            double distanceOutsideBoxInMeters = 5000;
            double distanceShowHjertestarters = 10;

            Xamarin.Essentials.Location Northeast = new Xamarin.Essentials.Location(northEastLat, northEastLong);
            Xamarin.Essentials.Location Southwest = new Xamarin.Essentials.Location(southWestLat, southWestLong);
            double kilometers = Xamarin.Essentials.Location.CalculateDistance(Northeast, Southwest, DistanceUnits.Kilometers);

            HjertestartersInBoxIfNeededResultDto hjertestartersInBoxIfNeededResultDto = new HjertestartersInBoxIfNeededResultDto();

            if (kilometers > distanceShowHjertestarters)
            {
                hjertestartersInBoxIfNeededResultDto.state = State.removeAll;
            }
            else
            {
                distanceOutsideBoxInMeters = kilometers * 1000;
                double decimalDegrees = ConvertUtil.MetersToDecimalDegrees(distanceOutsideBoxInMeters, northEastLat);
                double safeAreaNorthEastLat = northEastLat + decimalDegrees;
                double safeAreaNorthEastLong = northEastLong + decimalDegrees;
                double safeAreaSouthWestLat = southWestLat - decimalDegrees;
                double safeAreaSouthWestLong = southWestLong - decimalDegrees;

                bool isRedrawNeeded = false;
                bool isCleanupNeeded = false;// IsCleanupNeeded(safeAreaNorthEastLat, safeAreaSouthWestLat, lastNorthEastLat, lastSouthWestLat);

                if (isCleanupNeeded == true)
                {
                    hjertestartersInBoxIfNeededResultDto.state = State.cleanup;
                    return hjertestartersInBoxIfNeededResultDto;
                }

                if (NumberUtil.DoubleIsZero(lastNorthEastLat) && NumberUtil.DoubleIsZero(lastNorthEastLong) && NumberUtil.DoubleIsZero(lastSouthWestLat) && NumberUtil.DoubleIsZero(lastSouthWestLong))
                {
                    // Last area is not given as parameters
                    isRedrawNeeded = true;
                }
                else
                {
                    // Last area is given as parameters
                    if (northEastLat <= lastNorthEastLat
                        && northEastLong <= lastNorthEastLong
                        && southWestLat >= lastSouthWestLat
                        && southWestLong >= lastSouthWestLong)
                    {
                        isRedrawNeeded = false;
                    }
                    else
                    {
                        isRedrawNeeded = true;
                    }
                }

                if (isRedrawNeeded == true)
                {
                    hjertestartersInBoxIfNeededResultDto.resultNorthEastLat = safeAreaNorthEastLat;
                    hjertestartersInBoxIfNeededResultDto.resultNorthEastLong = safeAreaNorthEastLong;
                    hjertestartersInBoxIfNeededResultDto.resultSouthWestLat = safeAreaSouthWestLat;
                    hjertestartersInBoxIfNeededResultDto.resultSouthWestLong = safeAreaSouthWestLong;

                    object[] paramArray = new object[] { safeAreaSouthWestLat, safeAreaNorthEastLat, safeAreaSouthWestLong, safeAreaNorthEastLong };
                    string sql = "select * from Hjertestarter where siteLatitude BETWEEN ? and? and siteLongitude BETWEEN ? and ?";
                    List<Hjertestarter> hjertestarters = repository.QueryParams<Hjertestarter>(sql, paramArray);
                    hjertestartersInBoxIfNeededResultDto.hjertestarters = hjertestarters;
                }

                if (isRedrawNeeded == true)
                    hjertestartersInBoxIfNeededResultDto.state = State.replace;
                else
                    hjertestartersInBoxIfNeededResultDto.state = State.noAction;
            }
            return hjertestartersInBoxIfNeededResultDto;
        }

        private bool IsCleanupNeeded(double safeAreaNorthEastLat, double safeAreaSouthWestLat, double lastNorthEastLat, double lastSouthWestLat)
        {
            double percentSmaller = 0.5;

            double latLast = lastNorthEastLat - lastSouthWestLat;
            double latSafeArea = safeAreaNorthEastLat - safeAreaSouthWestLat;

            double factor = latSafeArea / latLast;

            if (factor < percentSmaller)
                return true;
            else
                return false;
        }

        private static double positionInsideSafeAreaLastNorthEastLat = 0;
        private static double positionInsideSafeAreaLastNorthEastLong = 0;
        private static double positionInsideSafeAreaLastSouthWestLat = 0;
        private static double positionInsideSafeAreaLastSouthWestLong = 0;
        private static double positionInsideSafeAreaLastDecimalDegrees = 0;

        /// <summary>
        /// Is this position inside safe area
        /// </summary>
        /// <param name="latitude">Latitude for check</param>
        /// <param name="longitude">Longintude for check</param>
        /// <param name="northEastLat">Latitude for upper right corner</param>
        /// <param name="northEastLong">Longitude for upper right corner</param>
        /// <param name="southWestLat">Latitude for bottom left corner</param>
        /// <param name="southWestLong">Longitude for bottom left corner</param>
        public bool IsPositionInsideSafeAreaForBox(double latitude, double longitude, double northEastLat, double northEastLong, double southWestLat, double southWestLong)
        {
            double distanceOutsideBoxInMeters = 50000;

            HjertestartersInBoxIfNeededResultDto hjertestartersInBoxIfNeededResultDto = new HjertestartersInBoxIfNeededResultDto();

            double decimalDegrees;

            if (northEastLat == positionInsideSafeAreaLastNorthEastLat
               && northEastLong == positionInsideSafeAreaLastNorthEastLong
               && southWestLat == positionInsideSafeAreaLastSouthWestLat
               && southWestLong == positionInsideSafeAreaLastSouthWestLong)
            {
                // Same area as last time
                decimalDegrees = positionInsideSafeAreaLastDecimalDegrees;
            }
            else
            {
                // Different area as last time
                decimalDegrees = ConvertUtil.MetersToDecimalDegrees(distanceOutsideBoxInMeters, northEastLat);

                positionInsideSafeAreaLastNorthEastLat = northEastLat;
                positionInsideSafeAreaLastNorthEastLong = northEastLong;
                positionInsideSafeAreaLastSouthWestLat = southWestLat;
                positionInsideSafeAreaLastSouthWestLong = southWestLong;
            }

            double safeAreaNorthEastLat = northEastLat + decimalDegrees;
            double safeAreaNorthEastLong = northEastLong + decimalDegrees;
            double safeAreaSouthWestLat = southWestLat - decimalDegrees;
            double safeAreaSouthWestLong = southWestLong - decimalDegrees;

            if (latitude >= safeAreaSouthWestLat
               && latitude <= safeAreaNorthEastLat
               && longitude >= safeAreaSouthWestLong
               && longitude <= safeAreaNorthEastLong)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
using System;
using System.Diagnostics;
using CoreLocation;
using Foundation;
using Henspe.Core.Const;
using Henspe.Core.Util;
using Henspe.iOS.AppModel;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using SNLA.Core.Util;
using UIKit;

namespace Henspe.iOS
{
    public class LocationManager
    {
        public NSDictionary parametersSetupPosition;
        public GPSObject gpsCurrentPositionObject = new GPSObject();
        public GPSObject gpsStoredPositionObject = new GPSObject();

        private CLLocationManager _locationManager;
        public CLLocationManager locationManager
        {
            get
            {
                if (_locationManager == null)
                {
                    _locationManager = new CLLocationManager
                    {
                        ActivityType = CLActivityType.OtherNavigation,
                    };
                }

                return _locationManager;
            }
        }

        public CLLocation lastKnownLocation = null;
        public CLGeocoder geocoder = null;

        // GPS
        public const double gpsAccuracyRequirement = 200;
        public const double whenToShowCheckAsDirectionLessThanMeters = 10;
        public const double highAndLowAccuracyDivider = 200;

        public bool mapZoomDone = false;
        public bool gpsEventOccured = false;
        public bool gpsHeadingEventOccured = false;
        public bool gotReception = false;
        public int gpsCoverage = GpsCoverageConst.none;
        public int gpsStoredCoverage = GpsCoverageConst.none;
        public bool storedLocation = false;
        public double northRad = 0;
        public bool gpsStarted = false;

        public bool gpsPosFound = false;
        public const double distanceToUpdateAddress = 50;
        public const double distanceToResentPosition = 50;
        public double desiredAccuracy = 10;
        public double distanceFilter = 10;
        public const double inEmergencyCallAccuracy = 10;
        public const double inEmergencyCallFilter = 25;
        public const double inBackgroundAccuracy = 0; // Max
        public const double inBackgroundFilter = 100; // Max
        public bool lastLocationWasInNorway = true;

        // Flash text
        public string lastNorthText = "";
        public string lastEastText = "";

        public int lastPositionType = PositionTypeConst.off;
        public CLAuthorizationStatus gpsStatus;

        public LocationManager()
        {
        }

        public bool HasLocationPermission()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                if (HasAllowAlways() || HasAllowWhenInUse())
                    return true;
                else
                    return false;
            }
            else
            {
                // iOS9
                if (HasAllowIos9())
                    return true;
                else
                    return false;
            }
        }

        public bool HasAllowIos9()
        {
            if (CLLocationManager.Status == CLAuthorizationStatus.Authorized)
                return true;
            else
                return false;
        }

        public bool HasAllowAlways()
        {
            //GetLocationServiceAccess();
            if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedAlways)
                return true;
            else
                return false;
        }

        public bool HasAllowWhenInUse()
        {
            //GetLocationServiceAccess();
            if (CLLocationManager.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                return true;
            else
                return false;
        }

        public bool HasDenied()
        {
            //GetLocationServiceAccess();
            if (CLLocationManager.Status == CLAuthorizationStatus.Denied)
                return true;
            else
                return false;
        }

        private void SetGPSSettingsForEmergencyCall()
        {
            Debug.WriteLine("Setting GPS.Accuracy to: " + inEmergencyCallAccuracy);
            Debug.WriteLine("Setting GPS.DistanceFilter to: " + inEmergencyCallFilter);

            locationManager.DesiredAccuracy = inEmergencyCallAccuracy;
            locationManager.DistanceFilter = inEmergencyCallFilter;

            if (UserUtil.Current.onboardingCompleted)
                locationManager.AllowsBackgroundLocationUpdates = true;
        }

        public LocationServiceAccess GetLocationServiceAccess()
        {
            if (HasAllowWhenInUse())
                return LocationServiceAccess.onlyWhenInUse;
            else if (HasAllowAlways())
                return LocationServiceAccess.always;
            else if (HasDenied())
                return LocationServiceAccess.notAllowed;
            else
                return LocationServiceAccess.notSet;
        }

        public void StartGPSTracking()
        {
            AppDelegate.current.mainViewController.InvokeOnMainThread(delegate
            {
                AppDelegate.current.locationManager.lastPositionType = PositionTypeConst.finding;
                NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.setupPosition, null);
            });

            var location = locationManager.Location;
            if (location != null)
                UpdateLocation(location);

            //TODO: Look into this
            // Tracking group or groups should include current posistion
            //AppDelegate.current.LocationManager.AuthorizationChanged -= OnAuthorizationChanged;
            //AppDelegate.current.LocationManager.AuthorizationChanged += OnAuthorizationChanged;

            locationManager.StopUpdatingLocation();

            Debug.WriteLine("Setting GPS.Accuracy to: " + desiredAccuracy);
            Debug.WriteLine("Setting GPS.DistanceFilter to: " + distanceFilter);

            locationManager.DesiredAccuracy = desiredAccuracy;
            locationManager.DistanceFilter = distanceFilter;

            locationManager.LocationsUpdated -= HandleLocationsUpdated;

            if (UserUtil.Current.onboardingCompleted)
            {
                locationManager.LocationsUpdated += HandleLocationsUpdated;
                locationManager.RequestAlwaysAuthorization();
            //    locationManager.AllowsBackgroundLocationUpdates = true;
                locationManager.StartUpdatingLocation();

                AppDelegate.current.locationManager.gpsStarted = true;
            }
        }

        private void HandleLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            //if (mapZoomDone)
            UpdateLocation(e);
        }

        private void UpdateLocation(CLLocationsUpdatedEventArgs e)
        {
            var location = e.Locations[e.Locations.Length - 1];
            UpdateLocation(location);
        }

        public void UpdateLocation(CLLocation newLocation)
        {
            double roundedLatitude = Math.Floor(newLocation.Coordinate.Latitude);
            double roundedLongitude = Math.Floor(newLocation.Coordinate.Longitude);

            // If location is cupertino, we are in simulator. Lets set it to Norway to speed things up
            /*if (roundedLatitude == POIConst.cupertinoLatitude && roundedLongitude == POIConst.cupertinoLongitude)
            {
                newLocation = new CLLocation(POIConst.stabekkLatitude, POIConst.stabekkLongitude);
            }*/

            var previousLocation = lastKnownLocation;

            lastKnownLocation = newLocation;
            gpsPosFound = true;

            int previousGpsCoverage = gpsCoverage;
            int newGpsCoverage = GpsCoverageConst.none;

            newGpsCoverage = iOSMapUtil.GetAccuracyType(newLocation.HorizontalAccuracy,
                                                        highAndLowAccuracyDivider,
                                                        GpsCoverageConst.low,
                                                        GpsCoverageConst.high);

            gpsCoverage = newGpsCoverage;

            GPSObject gpsObject = new GPSObject();
            gpsObject.accuracy = newLocation.HorizontalAccuracy;
            gpsObject.gpsCoordinates = newLocation.Coordinate;
            gpsObject.storedDateTime = DateTime.Now;
            gpsCurrentPositionObject = gpsObject;

            gpsEventOccured = true;

            if (newLocation.HorizontalAccuracy <= gpsAccuracyRequirement)
            {
                AppDelegate.current.mainViewController.InvokeOnMainThread(delegate
                {
                    AppDelegate.current.locationManager.lastPositionType = PositionTypeConst.found;
                    NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.setupPosition, null);
                });
            }
            else
            {
                AppDelegate.current.mainViewController.InvokeOnMainThread(delegate
                {
                    AppDelegate.current.locationManager.lastPositionType = PositionTypeConst.finding;
                    NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.setupPosition, null);
                });
            }

            UpdateGPSPositionIfSignificantChange(newLocation, previousLocation);
        }

        private bool UpdateGPSPositionIfSignificantChange(CLLocation newLocation, CLLocation previousLocation)
        {
            double lat1 = newLocation.Coordinate.Latitude;
            double lon1 = newLocation.Coordinate.Longitude;

            if (previousLocation == null || iOSMapUtil.Distance(lat1, lon1, previousLocation.Coordinate.Latitude, previousLocation.Coordinate.Longitude) > distanceToUpdateAddress)
            {
                AppDelegate.current.mainViewController.InvokeOnMainThread(delegate
                {
                    AppDelegate.current.locationManager.lastPositionType = PositionTypeConst.found;
                    NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.setupPosition, null);

                    NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.centerCurrentLocation, null);
                });

                return true;
            }

            return false;
        }
    }
}
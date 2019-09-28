using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Gms.Tasks;
using Android.Locations;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using AndroidX.LocalBroadcastManager.Content;
using Java.Lang;
using Task = Android.Gms.Tasks.Task;

namespace Henspe.Droid
{
    public enum LocationEventType
    {
        Unnassigned = 0,
        Location = 1,
        Availability = 2,
        ConnectionSuspended = 3,
        RequestLocation = 4,
        ServiceError = 5
    }

    public enum LocationIsLocationAvailable
    {
        Waiting = 0,
        OK = 1,
        NotFound = 2
    }

    public enum LocationStatus
    {
        WaitingForSignal = 0,
        OK = 1,
        LastLocation = 2
    }

    public enum LocationErrors
    {
        OK = 0,
        GooglePlayServicesNotInstalled = 1,
        GooglePlayServicesFails = 2,
        OtherErrors = 3,
        MissingPermission = 4
    }


    /**
     * A bound and started service that is promoted to a foreground service when location updates have
     * been requested and all clients unbind.
     *
     * For apps running in the background on "O" devices, location is computed only once every 10
     * minutes and delivered batched every 30 minutes. This restriction applies even to apps
     * targeting "N" or lower which are run on "O" devices.
     *
     * This sample show how to use a long-running service for location updates. When an activity is
     * bound to this service, frequent location updates are permitted. When the activity is removed
     * from the foreground, the service promotes itself to a foreground service, and location updates
     * continue. When the activity comes back to the foreground, the foreground service stops, and the
     * notification assocaited with that service is removed.
     */
    // [Service(Label = "LocationUpdatesService", Enabled = true, Exported = true)]
    //  [IntentFilter(new string[] { "com.xamarin.LocUpdFgService.LocationUpdatesService" })]

    [Service(Label = "LocationUpdatesService", Enabled = true, Exported = true)]
    [IntentFilter(new string[] { "com.xamarin.LocUpdFgService.LocationUpdatesService" })]

    public class LocationUpdatesService : Service, GoogleApiClient.IConnectionCallbacks,
GoogleApiClient.IOnConnectionFailedListener
    {
        GoogleApiClient apiClient;

        const string LocationPackageName = "com.xamarin.LocUpdFgService";

        public string Tag = "LocationUpdatesService";
        string ChannelId = "channel_01";

        public const string ActionBroadcast = LocationPackageName + ".broadcast";
        public const string ExtraLocation = LocationPackageName + ".location";
        public const string LocationType = LocationPackageName + ".locationtype";
        public const string LocationError = LocationPackageName + ".locationerror";
        public const string IsLocationAvailable = LocationPackageName + ".locationavailable";
        public const string ConnectionSuspendedCause = LocationPackageName + ".locationcause";
        public const string RequestLocation = LocationPackageName + ".locationrequest";
        const string ExtraStartedFromNotification = LocationPackageName + ".started_from_notification";

        public IBinder Binder;

        /**
         * The desired interval for location updates. Inexact. Updates may be more or less frequent.
         */
        const long UpdateIntervalInMilliseconds = 4000;

        /**
         * The fastest rate for active location updates. Updates will never be more frequent
         * than this value.
         */
        private const int PriorityAccuracy = LocationRequest.PriorityHighAccuracy;// jlsPriorityBalancedPowerAccuracy;
        const long FastestUpdateIntervalInMilliseconds = 5000;

        /**
         * The identifier for the notification displayed for the foreground service.
         */
        const int NotificationId = 12345678;

        /**
         * Used to check whether the bound activity has really gone away and not unbound as part of an
         * orientation change. We create a foreground service notification only if the former takes
         * place.
         */
        bool ChangingConfiguration = false;

        NotificationManager NotificationManager;

        /**
         * Contains parameters used by {@link com.google.android.gms.location.FusedLocationProviderApi}.
         */
        LocationRequest LocationRequest;

        /**
         * Provides access to the Fused Location Provider API.
         */
        FusedLocationProviderClient FusedLocationClient;

        /**
         * Callback for changes in location.
         */
        LocationCallback LocationCallback;

        Handler ServiceHandler;

        /**
         * The current location.
         */
        public Location Location;
        protected bool canStartForeground = false;
        private bool isGooglePlayServicesInstalled;

        public LocationUpdatesService()
        {
            //jls
            Binder = new LocationUpdatesServiceBinder(this);
            canStartForeground = false;

            //    Binder = new H113LocationUpdatesServiceBinder(this);
        }

        class LocationCallbackImpl : LocationCallback
        {
            public LocationUpdatesService Service { get; set; }

            public override void OnLocationAvailability(LocationAvailability locationAvailability)
            {
                Log.Debug("FusedLocationProviderSample", "IsLocationAvailable: {0}", locationAvailability.IsLocationAvailable);
                base.OnLocationAvailability(locationAvailability);
                Service.OnLocationAvailability(locationAvailability);
            }

            public override void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);
                Service.OnNewLocation(result.LastLocation);
            }
        }

        public override void OnCreate()
        {
            HandlerThread handlerThread = new HandlerThread(Tag);
            handlerThread.Start();
            ServiceHandler = new Handler(handlerThread.Looper);
            NotificationManager = (NotificationManager)GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string name = GetString(Resource.String.app_name);
                NotificationChannel mChannel = new NotificationChannel(ChannelId, name, NotificationManager.ImportanceDefault);
                NotificationManager.CreateNotificationChannel(mChannel);
            }

            isGooglePlayServicesInstalled = IsGooglePlayServicesInstalled();

            if (isGooglePlayServicesInstalled)
                apiClient = new GoogleApiClient.Builder(this, this, this).AddApi(LocationServices.API).Build();
        }

        private void OnLocationServiceError(LocationErrors error)
        {
            BroadcastEvent(LocationEventType.ServiceError, LocationError, (int)error);
        }

        private bool IsGooglePlayServicesInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Debug("MainActivity", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
                OnLocationServiceError(LocationErrors.GooglePlayServicesFails);
            else
                OnLocationServiceError(LocationErrors.GooglePlayServicesNotInstalled);

            var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            Log.Error("MainActivity", "There is a problem with Google Play Services on this device: {0} - {1}",
                      queryResult, errorString);

            return false;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Info(Tag, "Service started");
            var startedFromNotification = intent.GetBooleanExtra(ExtraStartedFromNotification, false);

            // We got here because the user decided to remove location updates from the notification.
            if (startedFromNotification)
            {
                RemoveLocationUpdates();
                StopSelf();
            }
            // Tells the system to not try to recreate the service after it has been killed.
            return StartCommandResult.NotSticky;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            ChangingConfiguration = true;
        }

        public override IBinder OnBind(Intent intent)
        {
            // Called when a client (MainActivity in case of this sample) comes to the foreground
            // and binds with this service. The service should cease to be a foreground service
            // when that happens.
            Log.Debug(Tag, "in onBind()");
            StopForeground(true);
            ChangingConfiguration = false;
            return Binder;
        }

        public override void OnRebind(Intent intent)
        {
            // Called when a client (MainActivity in case of this sample) returns to the foreground
            // and binds once again with this service. The service should cease to be a foreground
            // service when that happens.
            Log.Debug(Tag, "in onRebind()");
            StopForeground(true);
            ChangingConfiguration = false;
            base.OnRebind(intent);
        }

        public override bool OnUnbind(Intent intent)
        {
            Log.Debug(Tag, "Last client unbound from service");

            // Called when the last client (MainActivity in case of this sample) unbinds from this
            // service. If this method is called due to a configuration change in MainActivity, we
            // do nothing. Otherwise, we make this service a foreground service.
            if (!ChangingConfiguration && Utils.RequestingLocationUpdates(this))
            {
                Log.Debug(Tag, "Starting foreground service");
                /*
                // TODO(developer). If targeting O, use the following code.
                if (Build.VERSION.SDK_INT == Build.VERSION_CODES.O) {
                    mNotificationManager.startServiceInForeground(new Intent(this,
                            LocationUpdatesService.class), NOTIFICATION_ID, getNotification());
                } else {
                    startForeground(NOTIFICATION_ID, getNotification());
                }
                 */

                if (canStartForeground)
                    StartForeground(NotificationId, GetNotification());
            }
            return true; // Ensures onRebind() is called when a client re-binds.
        }

        public override void OnDestroy()
        {
            ServiceHandler.RemoveCallbacksAndMessages(null);
        }

        /*
        RequestLocationUpdates();
        MakeCall
            >sett flagg
            >send inn pos
            >start timer
            >viser over lay og sender pos
            >bakgrunn
            >sjekker call state og avslutter selv
            RemoveLocationUpdates()
            */

        /**
         * Makes a request for location updates. Note that in this sample we merely log the
         * {@link SecurityException}.
         */
        private void NOTUSEDRequestLocationUpdates()
        {
            Log.Debug(Tag, "Requesting location updates");
            Utils.SetRequestingLocationUpdates(this, true);
            StartService(new Intent(ApplicationContext, typeof(LocationUpdatesService)));
            try
            {
                FusedLocationClient.RequestLocationUpdates(LocationRequest, LocationCallback, Looper.MyLooper());
            }
            catch (SecurityException unlikely)
            {
                Utils.SetRequestingLocationUpdates(this, false);
                Log.Error(Tag, "Lost location permission. Could not request updates. " + unlikely);
            }
        }

        /**
         * Removes location updates. Note that in this sample we merely log the
         * {@link SecurityException}.
         */
        public void RemoveLocationUpdates()
        {
            Log.Debug(Tag, "Removing location updates");
            try
            {
                FusedLocationClient.RemoveLocationUpdates(LocationCallback);
                Utils.SetRequestingLocationUpdates(this, false);
                StopSelf();
            }
            catch (SecurityException unlikely)
            {
                Utils.SetRequestingLocationUpdates(this, true);
                Log.Error(Tag, "Lost location permission. Could not remove updates. " + unlikely);
            }
        }

        const int SERVICE_RUNNING_NOTIFICATION_ID = 123;
        const string NOTIFICATION_CHANNEL_ID = "com.company.app.channel";
        const string NOTIFICATION_CHANNEL_ID2 = "com.company.app.channel2";

        public MainActivity Activity { get; internal set; }

        private void StartNotification()
        {

            var text = Utils.GetLocationText(Location);

            // Check if device is running Android 8.0 or higher and call StartForeground() if so
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                                   .SetContentTitle(Resources.GetString(Resource.String.app_name))
                                   .SetContentText(text)
                                    .SetSmallIcon(Resource.Mipmap.ic_launcher)
                                   .SetOngoing(true)
                                     .SetSound(null)
                                   .Build();

                var notificationManager = GetSystemService(NotificationService) as NotificationManager;
                var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On-going Notification", NotificationImportance.Low);
                chan.SetShowBadge(false);
                chan.SetSound(null, null);
                notificationManager.CreateNotificationChannel(chan);
                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }
        }

        /**
         * Returns the {@link NotificationCompat} used as part of the foreground service.
         */
        Notification GetNotification()
        {
            Intent intent = new Intent(this, typeof(LocationUpdatesService));
            // Extra to help us figure out if we arrived in onStartCommand via the notification or not.
            intent.PutExtra(ExtraStartedFromNotification, true);

            // The PendingIntent that leads to a call to onStartCommand() in this service.
            var servicePendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            // The PendingIntent to launch activity.
            var text = Utils.GetLocationText(Location);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                                   .SetContentTitle(Resources.GetString(Resource.String.app_name))
                                   .SetContentText(text)
                                    .SetSmallIcon(Resource.Mipmap.ic_launcher)
                                   .SetOngoing(true)
                                     .SetSound(null)
                                   .Build();

                var notificationManager = GetSystemService(NotificationService) as NotificationManager;
                var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On-going Notification", NotificationImportance.Low);
                chan.SetShowBadge(false);
                chan.SetSound(null, null);
                notificationManager.CreateNotificationChannel(chan);
                return notification;
            }
            return null;
        }

        public async System.Threading.Tasks.Task RequestLocationUpdatesAsync()
        {
            Log.Debug(Tag, "Requesting location updates");
            Utils.SetRequestingLocationUpdates(this, true);
            StartService(new Intent(ApplicationContext, typeof(LocationUpdatesService)));
            try
            {
                await FusedLocationClient.RequestLocationUpdatesAsync(LocationRequest, LocationCallback, Looper.MyLooper());
            }
            catch (SecurityException unlikely)
            {
                Utils.SetRequestingLocationUpdates(this, false);
                Log.Error(Tag, "Lost location permission. Could not request updates. " + unlikely);
            }
        }

        /*
        public async Task StartLocationUpdatesAsync()
        {
            requestLocationUpdates = true;
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(Application.Context);

            await GetLastLocationFromDevice();
            await fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, locationCallback);

            Log.Debug(logTag, "Now sending location updates");
        }
        */
        // Handle location updates from the location manager
        public void StartLocationUpdates()
        {
            if (apiClient != null)
            {
                if (apiClient.IsConnected)
                    StartLocationUpdatesInt();
                else
                    apiClient.Connect();
            }
        }

        private void GetLastLocation()
        {
            try
            {
                FusedLocationClient.LastLocation.AddOnCompleteListener(new GetLastLocationOnCompleteListener { Service = this });
            }
            catch (SecurityException unlikely)
            {
                Log.Error(Tag, "Lost location permission." + unlikely);
            }
        }

        private void OnNewLocation(Location location)
        {
            Log.Debug(Tag, "New location: " + location);
            NewLocation(location);
            Location = location;

            // Notify anyone listening for broadcasts about the new location.
            Intent intent = new Intent(ActionBroadcast);
            intent.PutExtra(ExtraLocation, location);
            intent.PutExtra(LocationType, (int)LocationEventType.Location);

            LocalBroadcastManager.GetInstance(ApplicationContext).SendBroadcast(intent);

            // Update notification content if running as a foreground service.
            if (ServiceIsRunningInForeground(this))
            {
                NotificationManager.Notify(NotificationId, GetNotification());
            }
        }

        protected virtual void NewLocation(Location location)
        {

        }

        protected async System.Threading.Tasks.Task CheckLocationSettings()
        {
            bool permissionLocationGranted = true;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                permissionLocationGranted = Application.Context.CheckSelfPermission(Manifest.Permission.AccessFineLocation) == (int)Permission.Granted;
                if (!permissionLocationGranted)
                    OnLocationServiceError(LocationErrors.MissingPermission);
            }

            if (permissionLocationGranted)
            {
                if (apiClient != null)
                {
                    //jls
                    LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
                    builder.AddLocationRequest(LocationRequest);
                    var mLocationSettingsRequest = builder.Build();

                    //   BuildLocationSettingsRequest();

                    var result = await LocationServices.SettingsApi.CheckLocationSettingsAsync(
                        apiClient, mLocationSettingsRequest);
                    await HandleResult(result);
                }
            }
        }

        private async System.Threading.Tasks.Task HandleResult(LocationSettingsResult locationSettingsResult)
        {
            var status = locationSettingsResult.Status;
            switch (status.StatusCode)
            {
                case CommonStatusCodes.Success:
                    Log.Debug(Tag, "All location settings are satisfied.");
                    await RequestLocationUpdatesAsync();
                    break;
                case CommonStatusCodes.ResolutionRequired:
                    Log.Debug(Tag, "Location settings are not satisfied. Show the user a dialog to" +
                    "upgrade location settings ");

                    try
                    {
                        OnRequestLocation(status);
                        //   status.StartResolutionForResult(this, REQUEST_CHECK_SETTINGS);
                    }
                    catch (IntentSender.SendIntentException)
                    {
                        Log.Debug(Tag, "PendingIntent unable to execute request.");
                    }
                    break;
                case LocationSettingsStatusCodes.SettingsChangeUnavailable:
                    Log.Debug(Tag, "Location settings are inadequate, and cannot be fixed here. Dialog " +
                    "not created.");

                    OnLocationServiceError(LocationErrors.OtherErrors);
                    break;
            }
        }

        /**
         * Sets the location request parameters.
         */
        private void CreateLocationRequest()
        {
            LocationRequest = new LocationRequest();
            LocationRequest.SetInterval(UpdateIntervalInMilliseconds);
            LocationRequest.SetFastestInterval(FastestUpdateIntervalInMilliseconds);
            LocationRequest.SetPriority(PriorityAccuracy);
        }

        /**
         * Returns true if this is a foreground service.
         *
         * @param context The {@link Context}.
         */
        public bool ServiceIsRunningInForeground(Context context)
        {
            var manager = (ActivityManager)context.GetSystemService(ActivityService);
            foreach (var service in manager.GetRunningServices(Integer.MaxValue))
            {
                if (Class.Name.Equals(service.Service.ClassName))
                {
                    if (service.Foreground)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            var value = LocationIsLocationAvailable.NotFound;
            if (locationAvailability != null)
                if (locationAvailability.IsLocationAvailable)
                    value = LocationIsLocationAvailable.OK;
                else
                    value = LocationIsLocationAvailable.NotFound;

            BroadcastEvent(LocationEventType.Availability, IsLocationAvailable, (int)value);
        }

        private void BroadcastEvent(LocationEventType locationEventType, string valuename, int value)
        {
            // Notify anyone listening for broadcasts about the new location.
            Intent intent = new Intent(ActionBroadcast);
            intent.PutExtra(LocationType, (int)locationEventType);

            intent.PutExtra(valuename, value);
            LocalBroadcastManager.GetInstance(ApplicationContext).SendBroadcast(intent);

            // Update notification content if running as a foreground service.
            if (ServiceIsRunningInForeground(this))
            {
                NotificationManager.Notify(NotificationId, GetNotification());
            }
        }

        private void StartLocationUpdatesInt()
        {
            FusedLocationClient = LocationServices.GetFusedLocationProviderClient(this);
            LocationCallback = new LocationCallbackImpl { Service = this };

            CreateLocationRequest();
            CheckLocationSettings();

            //   GetLastLocation();

            //   RequestLocationUpdates();
            /*
            locationCallback = new FusedLocationProviderCallback(this);

            locationRequest = new LocationRequest()
                                    .SetPriority(priorityAccuracy)
                                    .SetInterval(updateIntervalInMilliseconds)
                                    .SetFastestInterval(fastestUpdateIntervalInMilliseconds);
            CheckLocationSettings();
            */
        }

        public void OnConnected(Bundle connectionHint)
        {
            StartLocationUpdatesInt();
        }

        public virtual void OnRequestLocation(Statuses status)
        {

        }

        public void OnConnectionSuspended(int cause)
        {
            BroadcastEvent(LocationEventType.ConnectionSuspended, ConnectionSuspendedCause, cause);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            OnLocationAvailability(null);
        }
    }

    /**
     * Class used for the client Binder.  Since this service runs in the same process as its
     * clients, we don't need to deal with IPC.
     */
    public class LocationUpdatesServiceBinder : Binder
    {
        readonly LocationUpdatesService service;

        public LocationUpdatesServiceBinder(LocationUpdatesService service)
        {
            this.service = service;
        }

        public LocationUpdatesService GetLocationUpdatesService()
        {
            return service;
        }
    }

    public class GetLastLocationOnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        public LocationUpdatesService Service { get; set; }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful && task.Result != null)
            {
                Service.Location = (Location)task.Result;
            }
            else
            {
                Log.Warn(Service.Tag, "Failed to get location.");
            }
        }
    }
}

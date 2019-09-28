using Android.App;
using Android.Content;
using Android.Views;
using Android.OS;
using Java.Lang;
using Android.Util;
using Android;
using Android.Preferences;
using Android.Net;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Android.Graphics;
using Android.Widget;
using Xamarin.Essentials;
using Location = Android.Locations.Location;
using Android.Provider;
using Google.Android.Material.AppBar;
using AndroidX.LocalBroadcastManager.Content;
using AndroidX.Core.Content;
using AndroidX.Core.App;
using Fragment = AndroidX.Fragment.App.Fragment;
using Google.Android.Material.Snackbar;

namespace Henspe.Droid
{

    [Activity(Label = "Main")]
    public class MainActivity : SinglePaneActivity, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        static readonly string TAG = "MainActivity";
        const string Tag = "MainActivity";
        // Used in checking for runtime permissions.
        const int RequestPermissionsRequestCode = 34;

        // The BroadcastReceiver used to listen from broadcasts from the service.
        LocastionReceiver myReceiver;

        // A reference to the service used to get location updates.
        LocationUpdatesService Service;

        // Tracks the bound state of the service.
        bool Bound;
        private Toolbar toolbar;
        private AppBarLayout appBarLayout;
        private HenspeFragment henspeFragment { get { return (HenspeFragment)_mFragment; } }

        // Monitors the state of the connection to the service.
        CustomServiceConnection ServiceConnection;
        class CustomServiceConnection : Object, IServiceConnection
        {
            public MainActivity Activity { get; set; }
            public void OnServiceConnected(ComponentName name, IBinder service)
            {
                LocationUpdatesServiceBinder binder = (LocationUpdatesServiceBinder)service;
                Activity.Service = binder.GetLocationUpdatesService();
                Activity.Bound = true;
                Activity.Service.StartLocationUpdates(); //RequestLocationUpdates();
                Activity.Service.Activity = Activity;
            }

            public void OnServiceDisconnected(ComponentName name)
            {
                Activity.Service = null;
                Activity.Bound = false;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AppCenter.Start("ff65fe46-1505-497b-95b7-001ce1e74a6a", typeof(Analytics), typeof(Crashes));

            myReceiver = new LocastionReceiver { Context = this };

            if (CheckPermissions())
                ServiceConnection = new CustomServiceConnection { Activity = this };

            // Check that the user hasn't revoked permissions by going to Settings.
            //     if (Utils.RequestingLocationUpdates(this))
            {
                if (!CheckPermissions())
                {
                    RequestPermissions();
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            PreferenceManager.GetDefaultSharedPreferences(this).RegisterOnSharedPreferenceChangeListener(this);

            // Bind to the service. If the service is in foreground mode, this signals to the service
            // that since this activity is in the foreground, the service can exit foreground mode.
            if (ServiceConnection != null)
                BindService(new Intent(this, typeof(LocationUpdatesService)), ServiceConnection, Bind.AutoCreate);
        }

        protected override void OnResume()
        {
            base.OnResume();
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(myReceiver,
                new IntentFilter(LocationUpdatesService.ActionBroadcast));
        }

        protected override void OnPause()
        {
            LocalBroadcastManager.GetInstance(this).UnregisterReceiver(myReceiver);
            base.OnPause();
        }

        protected override void OnStop()
        {
            if (Bound)
            {
                // Unbind from the service. This signals to the service that this activity is no longer
                // in the foreground, and the service can respond by promoting itself to a foreground
                // service.
                UnbindService(ServiceConnection);
                Bound = false;
            }
            PreferenceManager.GetDefaultSharedPreferences(this)
                    .UnregisterOnSharedPreferenceChangeListener(this);
            base.OnStop();
        }

        /**
         * Returns the current state of the permissions needed.
         */
        bool CheckPermissions()
        {
            return PermissionChecker.PermissionGranted == ContextCompat.CheckSelfPermission(this,
                Manifest.Permission.AccessFineLocation);
        }

        void RequestPermissions()
        {
            var shouldProvideRationale = ActivityCompat.ShouldShowRequestPermissionRationale(this,
                Manifest.Permission.AccessFineLocation);

            // Provide an additional rationale to the user. This would happen if the user denied the
            // request previously, but didn't check the "Don't ask again" checkbox.
            if (shouldProvideRationale)
            {
                Log.Info(Tag, "Displaying permission rationale to provide additional context.");
                Snackbar.Make(
                        FindViewById(Resource.Id.activity_main),
                        Resource.String.permission_rationale,
                        Snackbar.LengthIndefinite)
                        .SetAction(Resource.String.ok, (obj) =>
                        {
                            // Request permission
                            ActivityCompat.RequestPermissions(this,
                                    new string[] { Manifest.Permission.AccessFineLocation },
                                    RequestPermissionsRequestCode);
                        })
                        .Show();
            }
            else
            {
                Log.Info(Tag, "Requesting permission");
                // Request permission. It's possible this can be auto answered if device policy
                // sets the permission in a given state or the user denied the permission
                // previously and checked "Never ask again".
                ActivityCompat.RequestPermissions(this,
                        new string[] { Manifest.Permission.AccessFineLocation },
                        RequestPermissionsRequestCode);
            }
        }

        protected const int REQUEST_CHECK_SETTINGS = 0x1;

        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
       

            switch (requestCode)
            {
                case REQUEST_CHECK_SETTINGS:
                    switch (resultCode)
                    {
                        case Result.Ok:
                            Log.Debug(TAG, "User agreed to make required location settings changes.");
                            //   await StartLocationUpdates();
                            Service.StartLocationUpdates();// App.Current.LocationService.StartLocationUpdatesAsync();
                                                           //        await Hjelp113.current.applicationService.locationUpdatesService.StartLocationUpdates();// App.Current.LocationService.StartLocationUpdatesAsync();
                                                           //   App.StartLocationService();
                            break;
                        case Result.Canceled:
                            Log.Debug(TAG, "User chose not to make required location settings changes.");
                            ShowNoConnection();
                            break;
                    }
                    break;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
                         Android.Content.PM.Permission[] grantResults)
        {
            Log.Info(Tag, "onRequestPermissionResult");
            if (requestCode == RequestPermissionsRequestCode)
            {
                if (grantResults.Length <= 0)
                {
                    // If user interaction was interrupted, the permission request is cancelled and you
                    // receive empty arrays.
                    Log.Info(Tag, "User interaction was cancelled.");
                }
                else if (grantResults[0] == PermissionChecker.PermissionGranted)
                {
                    // Permission was granted.

                    if (ServiceConnection == null)
                    {
                        ServiceConnection = new CustomServiceConnection { Activity = this };
                        BindService(new Intent(this, typeof(LocationUpdatesService)), ServiceConnection, Bind.AutoCreate);
                    }
                    //    Service.RequestLocationUpdates();
                }
                else
                {
                    // Permission denied.
                    //   SetButtonsState(false);
                    Snackbar.Make(
                            FindViewById(Resource.Id.activity_main),
                            Resource.String.permission_denied_explanation,
                            Snackbar.LengthIndefinite)
                            .SetActionTextColor(Color.White)
                            .SetAction(Resource.String.settings, (obj) =>
                            {
                                // Build intent that displays the App settings screen.
                                Intent intent = new Intent();
                                intent.SetAction(Android.Provider.Settings.ActionApplicationDetailsSettings);
                                var uri = Uri.FromParts("package", PackageName, null);
                                intent.SetData(uri);
                                intent.SetFlags(ActivityFlags.NewTask);
                                StartActivity(intent);
                            })
                            .Show();
                }
            }
        }

        protected override Fragment OnCreatePane()
        {
            return new HenspeFragment();
        }

        private void GoToInfoScreen()
        {
            var intentInitialActivity = new Intent(this, typeof(OnboardingActivity));
            StartActivity(intentInitialActivity);
            //this.Activity.Finish();
        }

        /*
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater menuInflater = MenuInflater;
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
        */

        public void OnConnectionSuspended(int cause)
        {
        }

        public void HandleLocationServiceError(LocationErrors error)
        {
            Log.Debug(TAG, "ERROR: " + error.ToString());
            bool showToast = false;

            if (error == LocationErrors.GooglePlayServicesNotInstalled)
            {
                Log.Debug(TAG, "GooglePlay Services not installed");
                if (showToast)
                    Toast.MakeText(this, "GooglePlay Services not installed", ToastLength.Short).Show();
            }
            if (error == LocationErrors.MissingPermission)
            {
                Log.Debug(TAG, "MissingPermission");
                if (showToast)
                    Toast.MakeText(this, "MissingPermission", ToastLength.Short).Show();
            }
            if (error == LocationErrors.GooglePlayServicesFails)
            {
                Log.Debug(TAG, "GooglePlay Services fails");
                if (showToast)
                    Toast.MakeText(this, "GooglePlay Services fails", ToastLength.Short).Show();
            }
            if (IsAirplaneModeOn(ApplicationContext))
            {
                Log.Debug(TAG, "IsAirplaneModeOn");
                if (showToast)
                    Toast.MakeText(this, "AirplaneModeOn", ToastLength.Short).Show();
            }
            else
            {
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    // Connection to internet is available
                    if (showToast)
                        Toast.MakeText(this, "NoInternet", ToastLength.Short).Show();
                    Log.Debug(TAG, "NoInternet");
                }
            }
            //   ShowLocationFragment(myLocation, LocationStatus.OK);
            //    infoPanelVisible = InfoPanelVisible.False;
            //   ShowinfoPanel(InfoPanelVisible.False);
        }

        bool IsAirplaneModeOn(Context context)
        {
            var isAirplaneModeOn = Settings.Global.GetInt(context.ContentResolver, Settings.Global.AirplaneModeOn);
            return isAirplaneModeOn != 0;
        }

        public void HandleLocationAvailability(LocationIsLocationAvailable value)
        {
            RunOnUiThread(() =>
            {
                if (value == LocationIsLocationAvailable.Waiting)
                {
                    Log.Debug(TAG, "Waiting on Connection");
                    //   ShowNoConnection();
                }
                else if (value == LocationIsLocationAvailable.OK)
                {
                    Log.Debug(TAG, "Connection available");
                    //   ShowNoConnection()
                }
                else
                {
                    Log.Debug(TAG, "No Connection available");
                    ShowNoConnection();
                }
            });
        }

        public void ShowNoConnection()
        {
        }

        public void OnLocationChangedInternal(Location location)
        {
            Henspe.Current.myLocation = location;

            if (henspeFragment != null)
                henspeFragment.UpdateLocation(location);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == 16908332)
            {
                GoToInfoScreen();
                return true;
            }
            if (item.ItemId == Resource.Id.menu_settings)
            {
                //     Service.RequestLocationUpdates();
                //   SettingsClicked();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }


        /**
                 * Receiver for broadcasts sent by {@link LocationUpdatesService}.
                 */
        class LocastionReceiver : BroadcastReceiver
        {
            public Context Context { get; set; }
            public override void OnReceive(Context context, Intent intent)
            {
                var locationEventType = (LocationEventType)intent.GetIntExtra(LocationUpdatesService.LocationType, (int)LocationEventType.Unnassigned);

                switch (locationEventType)
                {
                    case LocationEventType.Location:
                        {
                            var location = intent.GetParcelableExtra(LocationUpdatesService.ExtraLocation) as Location;
                            if (location != null)
                            {

                                if ((Context as MainActivity) != null)
                                    (Context as MainActivity).OnLocationChangedInternal(location);
                            }
                            break;
                        }
                    case LocationEventType.Availability:
                        {
                            var value = (LocationIsLocationAvailable)intent.GetIntExtra(LocationUpdatesService.IsLocationAvailable, (int)LocationIsLocationAvailable.NotFound);
                            (Context as MainActivity).HandleLocationAvailability(value);
                            break;
                        }
                    case LocationEventType.ConnectionSuspended:
                        {
                            var value = intent.GetIntExtra(LocationUpdatesService.ConnectionSuspendedCause, -1);
                            (Context as MainActivity).OnConnectionSuspended(value);
                            break;
                        }
                    /*
                case LocationEventType.RequestLocation:
                    {
                        var status = (Statuses)intent.GetIntExtra(LocationUpdatesService.RequestLocation, (int)Statuses.ResultCanceled);
                        (Context as MainActivity).OnRequestLocation(status);
                        break;
                    }
                    */
                    case LocationEventType.ServiceError:
                        {
                            var locationErrors = (LocationErrors)intent.GetIntExtra(LocationUpdatesService.LocationError, (int)LocationErrors.OK);
                            (Context as MainActivity).HandleLocationServiceError(locationErrors);
                            break;
                        }
                }
            }
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            // Update the buttons state depending on whether location updates are being requested.
            if (key.Equals(Utils.KeyRequestingLocationUpdates))
            {

            }
        }
    }
}
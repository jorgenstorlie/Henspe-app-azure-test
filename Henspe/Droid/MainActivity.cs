using Android.App;
using Android.Content;
using Android.Support.Design.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Fragment = Android.Support.V4.App.Fragment;
using Android.OS;
using Java.Lang;
using Android.Locations;
using Android.Util;
using Android.Support.V4.App;
using Android;
using Android.Preferences;
using Android.Support.V4.Content;
using Android.Net;

namespace Henspe.Droid
{
    [Activity(Label = "Main")]
    public class MainActivity : SinglePaneActivity, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        const string Tag = "MainActivity";
        // Used in checking for runtime permissions.
        const int RequestPermissionsRequestCode = 34;

        // The BroadcastReceiver used to listen from broadcasts from the service.
        MyReceiver myReceiver;

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

                Activity.Service.RequestLocationUpdates();
            }

            public void OnServiceDisconnected(ComponentName name)
            {
                Activity.Service = null;
                Activity.Bound = false;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            myReceiver = new MyReceiver { Context = this };

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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == 16908332)
            {
                GoToInfoScreen();
                return true;
            }
            if (item.ItemId == Resource.Id.menu_settings)
            {
                Service.RequestLocationUpdates();
                //   SettingsClicked();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }


        /**
                 * Receiver for broadcasts sent by {@link LocationUpdatesService}.
                 */
        class MyReceiver : BroadcastReceiver
        {
            public Context Context { get; set; }

            public override void OnReceive(Context context, Intent intent)
            {
                var location = intent.GetParcelableExtra(LocationUpdatesService.ExtraLocation) as Location;
                if (location != null)
                {
                    Henspe.Current.myLocation = location;

                    if ((Context as MainActivity).henspeFragment != null)
                        (Context as MainActivity).henspeFragment.UpdateLocation(location);

               //     Toast.MakeText(Context, Utils.GetLocationText(location), ToastLength.Short).Show();
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
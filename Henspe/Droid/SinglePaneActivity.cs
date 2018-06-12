using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.App;
using Android.Support.V4.App;
using Fragment = Android.App.Fragment;
using Android.Support.V4.Content;
using Henspe.Droid.Const;
using Android.Support.V7.App;
using System.Net;
using Android.Content.PM;
using Android;

namespace Henspe.Droid
{
	[Activity(Label = "")]
	public abstract class SinglePaneActivity : AppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
		private int REQUEST_WRITE_EXTERNAL_STORAGE_STATE = 143;

        public const string ArgUri = "_uri";
        private Fragment _mFragment;
		private NewVersionAvailableBroadcastReceiver _newVersionReceiver;

        protected SinglePaneActivity(IntPtr handle, JniHandleOwnership ownerShip): base(handle, ownerShip)
        {
        // nothing to do
        // (need this constructor to avoid exceptions)
        }

        protected SinglePaneActivity()
        {
        }

        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.ActivityBase);

			//ActionBar.SetLogo(Resource.Drawable.ic_snla);

			if (Intent.HasExtra(Intent.ExtraTitle))
			{
				Title = Intent.GetStringExtra(Intent.ExtraTitle);
			}

			if (bundle == null)
			{
				_mFragment = OnCreatePane();
				_mFragment.Arguments = (IntentToFragmentArguments(Intent));

				FragmentManager.BeginTransaction()
					.Add(Resource.Id.root_container, _mFragment, "single_pane")
					.Commit();
			}
			else
			{
				_mFragment = FragmentManager.FindFragmentByTag("single_pane");
			}
            
			_newVersionReceiver = new NewVersionAvailableBroadcastReceiver(this);
			LocalBroadcastManager.GetInstance(this).RegisterReceiver(_newVersionReceiver, new IntentFilter(EventsConst.newVersionAvailable));

			AskForUpdateIfExists();
		}

		private void AskForUpdateIfExists()
		{
			if (Henspe.Current.fetchNewVersion)
			{
				if (!Henspe.Current.askedIfUserWantNewVersion)
				{
					Henspe.Current.askedIfUserWantNewVersion = true;

					this.RunOnUiThread(() =>
                    {
    					Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
    					alert.SetTitle(Resources.GetString(Resource.String.alert_title));
    					alert.SetMessage(Resources.GetString(Resource.String.alert_content));
    					alert.SetPositiveButton(Resources.GetString(Resource.String.alert_yes), (sender, e) =>
    					{
							OpenUpdatePage();
							//DownloadNewVersionIfPermitted();
						});

    					alert.SetNegativeButton(Resources.GetString(Resource.String.alert_no), (sender, e) => { });

    					Dialog dialog = alert.Create();
    					dialog.Show();
					});
				}
			}
		}

		private void OpenUpdatePage()
		{
			var uri = Android.Net.Uri.Parse(EventsConst.newApkInstallUrl);
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
		}

		private void DownloadNewVersionIfPermitted()
		{
			Permission permissionCheck = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage);

			if (permissionCheck != Permission.Granted)
            {
				ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, REQUEST_WRITE_EXTERNAL_STORAGE_STATE);
            }
            else
            {
                Henspe.Current.fetchNewVersion = false;

                var uri = global::Android.Net.Uri.Parse(EventsConst.newApkUrl);

                DownloadManager manager = (DownloadManager)GetSystemService(Context.DownloadService);
                DownloadManager.Request request = new DownloadManager.Request(uri);

                request.AllowScanningByMediaScanner();
                request.SetDestinationInExternalPublicDir(global::Android.OS.Environment.DirectoryDownloads,
                                                          EventsConst.newApkName);
                request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                request.SetVisibleInDownloadsUi(true);

                long reference = manager.Enqueue(request);
            }
		}

		public void OnNewVersionDownloaded()
		{
			LocalBroadcastManager.GetInstance(this).UnregisterReceiver(_newVersionReceiver);
		}

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    //Intent intent = NavUtils.GetParentActivityIntent(this);
                    //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop); 
                    //NavUtils.NavigateUpTo(this, intent);
                    //NavUtils.NavigateUpFromSameTask(this);
					Finish ();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			if(requestCode == REQUEST_WRITE_EXTERNAL_STORAGE_STATE)
			{
				if(grantResults.Length > 0 && grantResults[0] == Permission.Granted)
				{
					DownloadNewVersionIfPermitted();
				}
			}
		}

		/**
         * Called in <code>onCreate</code> when the fragment constituting this activity is needed.
         * The returned fragment's arguments will be set to the intent used to invoke this activity.
         */
		protected abstract Fragment OnCreatePane();

        public Fragment GetFragment()
        {
            return _mFragment;
        }

        /**
     * Converts an intent into a {@link android.os.Bundle} suitable for use as fragment arguments.
     */
        public static Bundle IntentToFragmentArguments(Intent intent)
        {
            var arguments = new Bundle();
            if (intent == null)
            {
                return arguments;
            }

            var data = intent.Data;
            if (data != null)
            {
                arguments.PutParcelable(ArgUri, data);
            }

            var extras = intent.Extras;
            if (extras != null)
            {
                arguments.PutAll(intent.Extras);
            }

            return arguments;
        }

		protected override void OnResume()
		{
			base.OnResume();

			//if (LATSamband.Current.wasInBackground)
			//	LATSamband.Current.OnFromBackroundToForeground();

			/*
			if (!(this is OnDutyBaseSelectActivity) || !(this is OnDutyAsActivity))
			{
				if (LATSamband.Current.wasInBackground)
					LATSamband.Current.OnFromBackroundToForeground();

				LATSamband.Current.StopActivityTranstitionTimer();
			}
			*/
			//LATSamband.Current.StopActivityTranstitionTimer();

			Henspe.Current.CheckNewAppVersionAvailable();
			AskForUpdateIfExists();
		}

		protected override void OnPause()
		{
			base.OnPause();

			//LATSamband.Current.StartActivityTranstitionTimer ();
		}

        /*
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        */

        private class NewVersionAvailableBroadcastReceiver : BroadcastReceiver
		{
			private WeakReference<SinglePaneActivity> _activity;

			public NewVersionAvailableBroadcastReceiver(SinglePaneActivity fragment)
			{
				_activity = new WeakReference<SinglePaneActivity>(fragment);
			}

			public override void OnReceive(Context context, Intent intent)
			{

			}
		}
    }
}
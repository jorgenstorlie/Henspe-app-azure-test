using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.App;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace Henspe.Droid
{
    [Activity(Label = "")]
    public abstract class SinglePaneActivity : AppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        private Toolbar toolbar;
        private AppBarLayout appBarLayout;

        private Fragment _mFragment;

        protected SinglePaneActivity(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
            // nothing to do
            // (need this constructor to avoid exceptions)
        }

        protected SinglePaneActivity()
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            SetTheme(Resource.Style.AppThemeDarkMaterial);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MainNew);

            //   SetContentView(Resource.Layout.ActivityBase);

            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            FindViewById<AppBarLayout>(Resource.Id.appBar).BringToFront();

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_info);




            //ActionBar.SetLogo(Resource.Drawable.ic_snla);

            if (Intent.HasExtra(Intent.ExtraTitle))
            {
                Title = Intent.GetStringExtra(Intent.ExtraTitle);
            }

            if (bundle == null)
            {
                _mFragment = OnCreatePane();
                //		_mFragment.Arguments = (IntentToFragmentArguments(Intent));

                SupportFragmentManager.BeginTransaction()
                            .Add(Resource.Id.sample_content_fragment, _mFragment, "single_pane")
                            .Commit();
            }
            else
            {
                _mFragment = SupportFragmentManager.FindFragmentByTag("single_pane");
            }
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
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
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

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

    }
}
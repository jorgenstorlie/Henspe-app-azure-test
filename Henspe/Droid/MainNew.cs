using System;
using Android;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.Apis;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;

using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using ScreenOrientation = Android.Content.PM.ScreenOrientation;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;

namespace Henspe.Droid
{
    [Activity(Label = "MainNew")]
    public class MainNew : AppCompatActivity
    {
        NewHenspeFragment henspeFragment;
        private Toolbar toolbar;
        private AppBarLayout appBarLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            SetTheme(Resource.Style.AppTheme);

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainNew);
            toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            FindViewById<AppBarLayout>(Resource.Id.appBar).BringToFront();

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

          
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_info);
         


            if (savedInstanceState == null)
            {
                var transaction = SupportFragmentManager.BeginTransaction();
                henspeFragment = new NewHenspeFragment();
                transaction.Replace(Resource.Id.sample_content_fragment, henspeFragment);
                transaction.Commit();
            }


        }


    
        private void GoToInfoScreen()
        {
            var intentInitialActivity = new Intent(this, typeof(OnBoardingActivity));
            StartActivity(intentInitialActivity);
            //this.Activity.Finish();
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater menuInflater = MenuInflater;
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
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
                //   SettingsClicked();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

    }
}

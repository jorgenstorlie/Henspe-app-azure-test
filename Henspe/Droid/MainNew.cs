using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
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
            SetTheme(Resource.Style.AppThemeDarkMaterial);

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

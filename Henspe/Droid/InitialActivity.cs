using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Henspe.Droid.Adapter;
using Henspe.Droid.Util;
using Henspe.Droid.Utils;
using Henspe.Droid.View.Indicator;

namespace Henspe.Droid
{
    [Activity(Label = "InitialActivity", NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class InitialActivity : AppCompatActivity
    {
        private TextView mSkipButton;
        private static TextView mNextButton;
		private global::Android.Support.V4.View.ViewPager _mPager;
        private InitialPagerAdapter mPagerAdapter;
        private CirclePageIndicator _mCirclePageIndicator;

        static int totalPages = 3;
        static int currentPage = 0;

        private double scrollTime = 0.5;
        private float scrollViewBorder = 8;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //((PreferenceUtil.AndroidSettings) PreferenceUtil.Settings).SettingsContext = this;
            
			SetContentView(Resource.Layout.initial_act_indicator);
            mSkipButton = FindViewById<TextView>(Resource.Id.initial_skip_button);
            mNextButton = FindViewById<TextView>(Resource.Id.initial_next_button);
            mSkipButton.Click += SkipButtonOnClick;
            //mSkipButton.Visibility = ViewStates.Invisible;

            mNextButton.Click += NextButtonOnClick;

            _mPager = FindViewById<ViewPager>(Resource.Id.pager);
            mPagerAdapter = new InitialPagerAdapter(SupportFragmentManager);
            _mPager.Adapter = mPagerAdapter;
            _mCirclePageIndicator = FindViewById<CirclePageIndicator>(Resource.Id.indicator);
            _mCirclePageIndicator.SetViewPager(_mPager);
            _mCirclePageIndicator.SetSnap(true);
            _mCirclePageIndicator.SetOnPageChangeListener(new CirclePageChangeListener(_mPager, this));
        }

		public void OnPageSelected(int position)
		{
			if (position == (totalPages - 1))
			{
				mNextButton.Text = this.GetString(Resource.String.Initial_Finished);
            }
			else
			{
				mNextButton.Text = this.GetString(Resource.String.Initial_Next);
			}
		}

		private void SkipButtonOnClick(object sender, EventArgs eventArgs)
		{
			NavigateToMain();
		}

		private void NavigateToMain()
		{
			UserUtil.settings.instructionsFinished = true;
			StartActivity(new Intent(Application.Context, typeof(MainActivity)));
			Finish();
		}

		private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {
			//var instructionsFinished = UserUtil.settings.instructionsFinished;
            if (currentPage == (totalPages - 1))
            {
				// Last page
				NavigateToMain();
            }
			else 
			{
				// Not last page
				if (currentPage < (totalPages - 1))
                {
                    currentPage++;
                }

                _mPager.SetCurrentItem(currentPage, true);
			}
        }

        protected override async void OnResume()
        {
            base.OnResume();
        }

		private class CirclePageChangeListener : Java.Lang.Object,
		    global::Android.Support.V4.View.ViewPager.IOnPageChangeListener
        {
			private readonly InitialActivity _mContext;
            private readonly ViewPager _mPager;

			public CirclePageChangeListener(ViewPager pager, InitialActivity context)
            {
                _mPager = pager;
                _mContext = context;
            }

            public void OnPageScrollStateChanged(int state)
            {
            }

            public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
            {
                currentPage = position;
            }

            public void OnPageSelected(int position)
            {
				_mContext.OnPageSelected(position);
            }
        }
    }
}
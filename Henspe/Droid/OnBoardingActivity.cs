using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using Henspe.Droid.Adapter;
using SNLA.Core.Util;

using Java.Lang;


// https://github.com/airbnb/lottie-android/blob/master/LottieSample/src/main/kotlin/com/airbnb/lottie/samples/AppIntroActivity.kt

namespace Henspe.Droid
{
    [Activity(Label = "OnBoardingActivity", NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppThemeOnBoarding")]
    public class OnBoardingActivity : AppCompatActivity, ViewPager.IOnPageChangeListener
    {
        private LottieAnimationView animationView;

        private TextView titleText;
        private TextView mSkipButton;
        private static TextView mNextButton;
        private global::Android.Support.V4.View.ViewPager _mPager;
        private InitialPagerAdapter mPagerAdapter;
       

        static int totalPages = 3;
        static int currentPage = 0;

        //private double scrollTime = 0.5;
        //private float scrollViewBorder = 8;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //((PreferenceUtil.AndroidSettings) PreferenceUtil.Settings).SettingsContext = this;

            SetContentView(Resource.Layout.acivity_onboarding);
            mSkipButton = FindViewById<TextView>(Resource.Id.onboarding_skip_button);
            mNextButton = FindViewById<TextView>(Resource.Id.onboarding_next_button);
            mSkipButton.Click += SkipButtonOnClick;

            titleText = FindViewById<TextView>(Resource.Id.onboarding_title);
            animationView = FindViewById<LottieAnimationView>(Resource.Id.onboarding_animation_view);

            //mSkipButton.Visibility = ViewStates.Invisible;

            mNextButton.Click += NextButtonOnClick;

            _mPager = FindViewById<ViewPager>(Resource.Id.onboarding_pager);
            mPagerAdapter = new InitialPagerAdapter(SupportFragmentManager);
        //    _mPager.Adapter = mPagerAdapter;


        //    _mCirclePageIndicator = FindViewById<CirclePageIndicator>(Resource.Id.onboarding_indicator);
         //   _mCirclePageIndicator.Visibility = ViewStates.Gone;

   //   var      onboarding_button_bottom = FindViewById<RelativeLayout>(Resource.Id.onboarding_button_bottom);
     //       onboarding_button_bottom.Visibility = ViewStates.Gone;
	
            /*
            _mCirclePageIndicator.SetViewPager(_mPager);
            _mCirclePageIndicator.SetSnap(true);
            _mCirclePageIndicator.SetOnPageChangeListener(new CirclePageChangeListener(_mPager, this, animationView));

            _mPager.SetBackgroundColor(Color.Transparent);
            */

            //  int colorInt2 = ContextCompat.GetColor(this, Resource.Color.primaryOnboarding);
            //  Color color2 = new Color(colorInt2);

            //  animationView.SetBackgroundColor(color2);
            //   titleText.Text = "H E N S P E";

            OnBoardingItemFragment f = new OnBoardingItemFragment(0);
            OnBoardingItemFragment f2 = new OnBoardingItemFragment(1);
            OnBoardingItemFragment f3 = new OnBoardingItemFragment(2);

            ViewPagerAdapter adapter = new ViewPagerAdapter((this as AppCompatActivity).SupportFragmentManager);
            adapter.addFragment(f, "");
            adapter.addFragment(f2, "");
            adapter.addFragment(f3, "");

            _mPager.Adapter = adapter;


            _mPager.PageSelected += ViewPager_PageSelected;
            _mPager.AddOnPageChangeListener(this);


            var dots = FindViewById<TabLayout>(Resource.Id.dots);
         dots.SetupWithViewPager(_mPager, true); // <- magic here

            _mPager.SetOnPageChangeListener(new CirclePageChangeListener(_mPager, this, animationView));



        }

        void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {

        }

        public void OnPageSelected(int position)
        {
            /*
            animationView.CancelAnimation();

            _mPager.SetBackgroundColor(Color.Transparent);
            animationView.SetBackgroundColor(Color.Red);

            if (position == 0)
                animationView.SetAnimation("intro1.json");
            else
                   if (position == 1)
                animationView.SetAnimation("intro2.json");
            else
    if (position == 2)
                animationView.SetAnimation("intro3.json");


            animationView.Progress = 0f;

            animationView.PlayAnimation();// Loop = true;
            */

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
            UserUtil.Current.onboardingCompleted = true;
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            Finish();
        }

        private void NextButtonOnClick(object sender, EventArgs eventArgs)
        {

			//var onboardingCompleted = UserUtil.Current.OnboardingCompleted;
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


        protected override void OnPause()
        {
            base.OnPause();
        }


        protected override async void OnResume()
        {
            base.OnResume();
        }

        public void OnPageScrollStateChanged(int state)
        {
        //    throw new NotImplementedException();
        }

        private float[] ANIMATION_TIMES = { 0f, 0.3333f, 0.6666f, 1f, 1f };


        float Lerp(float a, float b, float t)
        {
            return (1f - t) * a + t * b;
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {

            currentPage = position;

            float startProgress = ANIMATION_TIMES[position];
            float endProgress = ANIMATION_TIMES[position + 1];


            if (animationView.Animation == null)
                animationView.SetAnimation("intro1.json");

            animationView.Progress = Lerp(startProgress, endProgress, positionOffset);


        }

        private class CirclePageChangeListener : Java.Lang.Object,
            global::Android.Support.V4.View.ViewPager.IOnPageChangeListener
        {
            private readonly OnBoardingActivity _mContext;
            private readonly ViewPager _mPager;


            private readonly LottieAnimationView animationView;

            public CirclePageChangeListener(ViewPager pager, OnBoardingActivity context, LottieAnimationView animationView)
            {
                _mPager = pager;
                _mContext = context;
                this.animationView = animationView;
            }

            public void OnPageScrollStateChanged(int state)
            {
            }

            private float[] ANIMATION_TIMES = { 0f, 0.3333f, 0.6666f, 1f, 1f };


            float Lerp(float a, float b, float t)
            {
                return (1f - t) * a + t * b;
            }

            public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
            {
                currentPage = position;

                float startProgress = ANIMATION_TIMES[position];
                float endProgress = ANIMATION_TIMES[position + 1];


                if (animationView.Animation == null)
                    animationView.SetAnimation("intro1.json");

                animationView.Progress = Lerp(startProgress, endProgress, positionOffset);

            }

            public void OnPageSelected(int position)
            {
                _mContext.OnPageSelected(position);
            }
        }
    }
}
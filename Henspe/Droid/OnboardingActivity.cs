using System;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using Henspe.Droid.Adapter;
using SNLA.Core.Util;

// https://github.com/airbnb/lottie-android/blob/master/LottieSample/src/main/kotlin/com/airbnb/lottie/samples/AppIntroActivity.kt

namespace Henspe.Droid
{
    [Activity(Label = "OnBoardingActivity", NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppThemeOnBoarding")]
    public class OnboardingActivity : AppCompatActivity, ViewPager.IOnPageChangeListener, Animator.IAnimatorListener, ValueAnimator.IAnimatorUpdateListener
    {
        private LottieAnimationView animationView;
        private TextView titleText;
        private TextView mSkipButton;
        private static TextView mNextButton;
        private ViewPager _mPager;
        private InitialPagerAdapter mPagerAdapter;
        static int totalPages = 3;
        static int currentPage = 0;
        private int pageIndex = -1;
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
            mNextButton.Click += NextButtonOnClick;

            _mPager = FindViewById<ViewPager>(Resource.Id.onboarding_pager);
            mPagerAdapter = new InitialPagerAdapter(SupportFragmentManager);

            OnboardingItemFragment f = new OnboardingItemFragment(0);
            OnboardingItemFragment f2 = new OnboardingItemFragment(1);
            OnboardingItemFragment f3 = new OnboardingItemFragment(2);

            ViewPagerAdapter adapter = new ViewPagerAdapter((this as AppCompatActivity).SupportFragmentManager);
            adapter.addFragment(f, "");
            adapter.addFragment(f2, "");
            adapter.addFragment(f3, "");

            _mPager.Adapter = adapter;
            _mPager.PageSelected += ViewPager_PageSelected;
            _mPager.AddOnPageChangeListener(this);
            _mPager.PageScrolled += OnViewPagerPageScrolled;
            var dots = FindViewById<TabLayout>(Resource.Id.dots);
            dots.SetupWithViewPager(_mPager, true); // <- magic here

            _mPager.SetOnPageChangeListener(new CirclePageChangeListener(_mPager, this, animationView));

            this.animationView.AddAnimatorListener(this);
            this.animationView.AddAnimatorUpdateListener(this);

            if (animationView.Animation == null)
            {
                animationView.EnableMergePathsForKitKatAndAbove(true);
                animationView.SetScaleType(ImageView.ScaleType.CenterInside);
                animationView.SetAnimation("intro.json");

                //   animationView.CancelAnimation();
                animationView.RepeatCount = 0;
                animationView.PlayAnimation();

                //    animationView.SetBackgroundColor(Color.Green);
            }

        }

        private void OnViewPagerPageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            PlayAnimation(e.Position);
        }

        void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            //     PlayAnimation(e.Position);
        }

        public void OnPageSelected(int position)
        {
            HideLabel(position);
            if (position == 2)
                mSkipButton.Visibility = ViewStates.Gone;
            else
                mSkipButton.Visibility = ViewStates.Visible;

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

        private void PlayAnimation(int position)
        {
            HideLabel(position);

            if (pageIndex != position)
            {
                pageIndex = position;


                //     animationView.CancelAnimation();

                float startFrame = 0;
                float stopFrame = 0;

                switch (position)
                {
                    case 0:
                        startFrame = 0;
                        stopFrame = 125;
                        break;
                    case 1:
                        startFrame = 125;
                        stopFrame = 220;
                        break;
                    case 2:
                        startFrame = 210;
                        stopFrame = 311;
                        break;

                }

                stopFrame = (stopFrame / 310);
                startFrame = (startFrame / 310);

                //    animationView.SetMinFrame(startFrame);
                //  animationView.SetMaxFrame(stopFrame);
                //  animationView.Frame = startFrame;

                animationView.SetMinProgress(startFrame);
                //  animationView.SetMaxProgress(stopFrame);

                switch (pageIndex)
                {
                    case 0:
                        startFrame = 0.0f;
                        break;
                    case 1:
                        startFrame = 0.31f;
                        break;
                    case 2:
                        startFrame = 0.576f;
                        break;

                }


                //    animationView.Progress = startFrame;

                animationView.PlayAnimation();// Loop = true;


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
            /*
            float startProgress = ANIMATION_TIMES[position];
            float endProgress = ANIMATION_TIMES[position + 1];

            if (animationView.Animation == null)
                animationView.SetAnimation("intro.json");

            animationView.Progress = Lerp(startProgress, endProgress, positionOffset);

*/
        }

        public void ShowLabel(int position)
        {
            if (position != -1)
            {
                var adapter2 = (_mPager.Adapter as ViewPagerAdapter);
                (adapter2.GetItem(position) as OnboardingItemFragment).ShowLabel();
            }
        }
        public void HideLabel(int position)
        {
            if (position != -1)
            {
                var adapter2 = (_mPager.Adapter as ViewPagerAdapter);
                (adapter2.GetItem(position) as OnboardingItemFragment).HideLabel();
            }
        }

        #region Animator.IAnimatorListener
        public void OnAnimationCancel(Animator animation)
        {

        }

        public void OnAnimationEnd(Animator animation)
        {
            ShowLabel(pageIndex);
        }

        public void OnAnimationRepeat(Animator animation)
        {

        }

        public void OnAnimationStart(Animator animation)
        {

        }
        #endregion

        #region ValueAnimator.IAnimatorUpdateListener
        public void OnAnimationUpdate(ValueAnimator animation)
        {
            float stopFrame = 0;
            switch (pageIndex)
            {
                case 0:
                    stopFrame = 125;
                    break;
                case 1:
                    stopFrame = 200;
                    break;
                case 2:
                    stopFrame = 311;
                    break;

            }

            stopFrame = (stopFrame / 311);

            switch (pageIndex)
            {
                case 0:
                    stopFrame = 0.31f;
                    break;
                case 1:
                    stopFrame = 0.576f;
                    break;
                case 2:
                    stopFrame = 0.777f;
                    break;
            }

            if (animationView.Progress >= stopFrame)
                animationView.PauseAnimation();

            //  var adapter2 = (_mPager.Adapter as ViewPagerAdapter);
            //  (adapter2.GetItem(pageIndex) as OnboardingItemFragment).ShowLabelText(animationView.Progress.ToString());

            if (animationView.Progress >= stopFrame)
                ShowLabel(pageIndex);
        }
        #endregion

        private class CirclePageChangeListener : Java.Lang.Object,
            ViewPager.IOnPageChangeListener
        {
            private readonly OnboardingActivity _mContext;
            private readonly ViewPager _mPager;

            private readonly LottieAnimationView animationView;

            public CirclePageChangeListener(ViewPager pager, OnboardingActivity context, LottieAnimationView animationView)
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

                /*
                float startProgress = ANIMATION_TIMES[position];
                float endProgress = ANIMATION_TIMES[position + 1];

                if (animationView.Animation == null)
                    animationView.SetAnimation("intro.json");

                animationView.Progress = Lerp(startProgress, endProgress, positionOffset);
                */
            }

            public void OnPageSelected(int position)
            {
                _mContext.OnPageSelected(position);
            }

        }
    }
}
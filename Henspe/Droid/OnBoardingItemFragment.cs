using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Henspe.Droid
{
    public class OnBoardingItemFragment : global::Android.Support.V4.App.Fragment
    {
        private TextView mTextDescriptionTextView;
        private int mCurrentPosition = -1;

        public OnBoardingItemFragment(int position)
        {
            mCurrentPosition = position;
        }

        public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var viewRoot = inflater.Inflate(Resource.Layout.onboarding_fragment, container, false);
            mTextDescriptionTextView = viewRoot.FindViewById<TextView>(Resource.Id.onboarding_description_text);
            return viewRoot;
        }

        public override void OnResume()
        {
            base.OnResume();
            SetupInitialPageInformation();
        }

        private void SetupInitialPageInformation()
        {
            var textDesc = "";

            switch (mCurrentPosition)
            {
                case 0:

                    textDesc = Resources.GetString(Resource.String.Initial_PageOne_Text);

                    break;
                case 1:

                    textDesc = Resources.GetString(Resource.String.Initial_PageTwo_Text);

                    break;
                case 2:

                    textDesc = Resources.GetString(Resource.String.Initial_PageThree_Text);

                    break;
            }

            mTextDescriptionTextView.Text = textDesc;

            View.SetBackgroundColor(Color.Transparent);
        }
    }
}
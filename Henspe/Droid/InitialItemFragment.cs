using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace Henspe.Droid
{
	public class InitialItemFragment : global::Android.Support.V4.App.Fragment
    {
        private TextView mInitialTitleTextView;
        private TextView mTextDescriptionTextView;
        private ImageView mInitialImageView;

        private int mCurrentPosition = -1;

        public InitialItemFragment(int position)
        {
            mCurrentPosition = position;
        }

		public override global::Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var viewRoot = inflater.Inflate(Resource.Layout.initial_fragment, container, false);
            mInitialTitleTextView = viewRoot.FindViewById<TextView>(Resource.Id.initial_display_title);
            mInitialImageView = viewRoot.FindViewById<ImageView>(Resource.Id.initial_image_display);
            mTextDescriptionTextView = viewRoot.FindViewById<TextView>(Resource.Id.initial_description_text);
            return viewRoot;
        }

        public override void OnResume()
        {
            base.OnResume();
            SetupInitialPageInformation();
        }

        private void SetupInitialPageInformation()
        {
            var displayTitleText = "";
            var titleText = "";
            var textDesc = "";
            int initialImageResource = -1;
            switch (mCurrentPosition)
            {
                case 0:
                    displayTitleText = Resources.GetString(Resource.String.Initial_PageOne_Header);
                    textDesc = Resources.GetString(Resource.String.Initial_PageOne_Text);
                    initialImageResource = Resource.Drawable.ic_intro1;
                    break;
                case 1:
                    displayTitleText = Resources.GetString(Resource.String.Initial_PageTwo_Header);
                    textDesc = Resources.GetString(Resource.String.Initial_PageTwo_Text);
                    initialImageResource = Resource.Drawable.ic_intro2;
                    break;
                case 2:
                    displayTitleText = Resources.GetString(Resource.String.Initial_PageThree_Header);
                    textDesc = Resources.GetString(Resource.String.Initial_PageThree_Text);
                    initialImageResource = Resource.Drawable.ic_intro3;
                    break;
            }

            mInitialTitleTextView.Text = displayTitleText;
            mTextDescriptionTextView.Text = textDesc;
            mInitialImageView.SetImageDrawable(Resources.GetDrawable(initialImageResource));
        }
    }
}
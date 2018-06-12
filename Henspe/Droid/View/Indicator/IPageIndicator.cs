using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Henspe.Droid.View.Indicator
{
	public interface IPageIndicator : global::Android.Support.V4.View.ViewPager.IOnPageChangeListener
    {
		void SetViewPager(global::Android.Support.V4.View.ViewPager view);
		void SetViewPager(global::Android.Support.V4.View.ViewPager view, int initialPosition);
        void SetCurrentItem(int item);
		void SetOnPageChangeListener(global::Android.Support.V4.View.ViewPager.IOnPageChangeListener listener);
        void NotifyDataSetChanged();
    }
}
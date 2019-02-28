using System;
using Android.Runtime;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace Henspe.Droid.Adapter
{
    public class InitialPagerAdapter : FragmentStatePagerAdapter
    {
        private const int NumPage = 3;
        public int CurrentPage { get; set; }
        public InitialPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public InitialPagerAdapter(FragmentManager fm) : base(fm)
        {
        }

        public override int Count { get {return NumPage;} }
        public override Fragment GetItem(int position)
        {
            return new OnBoardingItemFragment(position);
        }
    }
}
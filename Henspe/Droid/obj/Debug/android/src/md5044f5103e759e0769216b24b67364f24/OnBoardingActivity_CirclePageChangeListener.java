package md5044f5103e759e0769216b24b67364f24;


public class OnBoardingActivity_CirclePageChangeListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.v4.view.ViewPager.OnPageChangeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageScrollStateChanged:(I)V:GetOnPageScrollStateChanged_IHandler:Android.Support.V4.View.ViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Support.Core.UI\n" +
			"n_onPageScrolled:(IFI)V:GetOnPageScrolled_IFIHandler:Android.Support.V4.View.ViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Support.Core.UI\n" +
			"n_onPageSelected:(I)V:GetOnPageSelected_IHandler:Android.Support.V4.View.ViewPager/IOnPageChangeListenerInvoker, Xamarin.Android.Support.Core.UI\n" +
			"";
		mono.android.Runtime.register ("Henspe.Droid.OnBoardingActivity+CirclePageChangeListener, Henspe.Droid", OnBoardingActivity_CirclePageChangeListener.class, __md_methods);
	}


	public OnBoardingActivity_CirclePageChangeListener ()
	{
		super ();
		if (getClass () == OnBoardingActivity_CirclePageChangeListener.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.OnBoardingActivity+CirclePageChangeListener, Henspe.Droid", "", this, new java.lang.Object[] {  });
	}

	public OnBoardingActivity_CirclePageChangeListener (android.support.v4.view.ViewPager p0, md5044f5103e759e0769216b24b67364f24.OnBoardingActivity p1, com.airbnb.lottie.LottieAnimationView p2)
	{
		super ();
		if (getClass () == OnBoardingActivity_CirclePageChangeListener.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.OnBoardingActivity+CirclePageChangeListener, Henspe.Droid", "Android.Support.V4.View.ViewPager, Xamarin.Android.Support.Core.UI:Henspe.Droid.OnBoardingActivity, Henspe.Droid:Com.Airbnb.Lottie.LottieAnimationView, Lottie.Android", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onPageScrollStateChanged (int p0)
	{
		n_onPageScrollStateChanged (p0);
	}

	private native void n_onPageScrollStateChanged (int p0);


	public void onPageScrolled (int p0, float p1, int p2)
	{
		n_onPageScrolled (p0, p1, p2);
	}

	private native void n_onPageScrolled (int p0, float p1, int p2);


	public void onPageSelected (int p0)
	{
		n_onPageSelected (p0);
	}

	private native void n_onPageSelected (int p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}

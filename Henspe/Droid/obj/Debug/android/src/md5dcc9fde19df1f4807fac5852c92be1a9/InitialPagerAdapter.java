package md5dcc9fde19df1f4807fac5852c92be1a9;


public class InitialPagerAdapter
	extends android.support.v4.app.FragmentStatePagerAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getItem:(I)Landroid/support/v4/app/Fragment;:GetGetItem_IHandler\n" +
			"";
		mono.android.Runtime.register ("Henspe.Droid.Adapter.InitialPagerAdapter, Henspe.Droid", InitialPagerAdapter.class, __md_methods);
	}


	public InitialPagerAdapter (android.support.v4.app.FragmentManager p0)
	{
		super (p0);
		if (getClass () == InitialPagerAdapter.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.Adapter.InitialPagerAdapter, Henspe.Droid", "Android.Support.V4.App.FragmentManager, Xamarin.Android.Support.Fragment", this, new java.lang.Object[] { p0 });
	}


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public android.support.v4.app.Fragment getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native android.support.v4.app.Fragment n_getItem (int p0);

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
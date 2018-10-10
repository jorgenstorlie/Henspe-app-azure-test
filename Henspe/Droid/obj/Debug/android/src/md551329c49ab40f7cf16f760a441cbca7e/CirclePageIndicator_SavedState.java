package md551329c49ab40f7cf16f760a441cbca7e;


public class CirclePageIndicator_SavedState
	extends android.view.View.BaseSavedState
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_writeToParcel:(Landroid/os/Parcel;I)V:GetWriteToParcel_Landroid_os_Parcel_IHandler\n" +
			"n_InitializeCreator:()Lmd551329c49ab40f7cf16f760a441cbca7e/CirclePageIndicator_SavedState_SavedStateCreator;:__export__\n" +
			"";
		mono.android.Runtime.register ("Henspe.Droid.View.Indicator.CirclePageIndicator+SavedState, Henspe.Droid", CirclePageIndicator_SavedState.class, __md_methods);
	}


	public CirclePageIndicator_SavedState (android.os.Parcel p0)
	{
		super (p0);
		if (getClass () == CirclePageIndicator_SavedState.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.View.Indicator.CirclePageIndicator+SavedState, Henspe.Droid", "Android.OS.Parcel, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public CirclePageIndicator_SavedState (android.os.Parcel p0, java.lang.ClassLoader p1)
	{
		super (p0, p1);
		if (getClass () == CirclePageIndicator_SavedState.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.View.Indicator.CirclePageIndicator+SavedState, Henspe.Droid", "Android.OS.Parcel, Mono.Android:Java.Lang.ClassLoader, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CirclePageIndicator_SavedState (android.os.Parcelable p0)
	{
		super (p0);
		if (getClass () == CirclePageIndicator_SavedState.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.View.Indicator.CirclePageIndicator+SavedState, Henspe.Droid", "Android.OS.IParcelable, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	private static md551329c49ab40f7cf16f760a441cbca7e.CirclePageIndicator_SavedState_SavedStateCreator CREATOR = InitializeCreator ();


	public void writeToParcel (android.os.Parcel p0, int p1)
	{
		n_writeToParcel (p0, p1);
	}

	private native void n_writeToParcel (android.os.Parcel p0, int p1);

	private static md551329c49ab40f7cf16f760a441cbca7e.CirclePageIndicator_SavedState_SavedStateCreator InitializeCreator ()
	{
		return n_InitializeCreator ();
	}

	private static native md551329c49ab40f7cf16f760a441cbca7e.CirclePageIndicator_SavedState_SavedStateCreator n_InitializeCreator ();

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

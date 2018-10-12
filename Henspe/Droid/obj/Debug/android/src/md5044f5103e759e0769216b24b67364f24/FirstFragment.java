package md5044f5103e759e0769216b24b67364f24;


public class FirstFragment
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Henspe.Droid.FirstFragment, Henspe.Droid", FirstFragment.class, __md_methods);
	}


	public FirstFragment ()
	{
		super ();
		if (getClass () == FirstFragment.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.FirstFragment, Henspe.Droid", "", this, new java.lang.Object[] {  });
	}

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

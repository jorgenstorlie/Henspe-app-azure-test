package md5a9c2a7829e33c55c7e3430b3d34666a9;


public class HenspeRowAdapter_SectionViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Henspe.Droid.Adapters.HenspeRowAdapter+SectionViewHolder, Henspe.Droid", HenspeRowAdapter_SectionViewHolder.class, __md_methods);
	}


	public HenspeRowAdapter_SectionViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == HenspeRowAdapter_SectionViewHolder.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.Adapters.HenspeRowAdapter+SectionViewHolder, Henspe.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
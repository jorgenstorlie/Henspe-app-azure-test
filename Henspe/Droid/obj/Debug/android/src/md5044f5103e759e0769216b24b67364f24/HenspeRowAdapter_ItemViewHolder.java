package md5044f5103e759e0769216b24b67364f24;


public class HenspeRowAdapter_ItemViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Henspe.Droid.HenspeRowAdapter+ItemViewHolder, Henspe.Droid", HenspeRowAdapter_ItemViewHolder.class, __md_methods);
	}


	public HenspeRowAdapter_ItemViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == HenspeRowAdapter_ItemViewHolder.class)
			mono.android.TypeManager.Activate ("Henspe.Droid.HenspeRowAdapter+ItemViewHolder, Henspe.Droid", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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

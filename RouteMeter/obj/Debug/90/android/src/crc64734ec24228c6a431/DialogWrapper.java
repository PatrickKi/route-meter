package crc64734ec24228c6a431;


public class DialogWrapper
	extends crc64734ec24228c6a431.DialogBase
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("RouteMeter.Dialogs.DialogWrapper, RouteMeter", DialogWrapper.class, __md_methods);
	}


	public DialogWrapper (android.content.Context p0)
	{
		super (p0);
		if (getClass () == DialogWrapper.class)
			mono.android.TypeManager.Activate ("RouteMeter.Dialogs.DialogWrapper, RouteMeter", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public DialogWrapper (android.content.Context p0, boolean p1, android.content.DialogInterface.OnCancelListener p2)
	{
		super (p0, p1, p2);
		if (getClass () == DialogWrapper.class)
			mono.android.TypeManager.Activate ("RouteMeter.Dialogs.DialogWrapper, RouteMeter", "Android.Content.Context, Mono.Android:System.Boolean, mscorlib:Android.Content.IDialogInterfaceOnCancelListener, Mono.Android", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public DialogWrapper (android.content.Context p0, int p1)
	{
		super (p0, p1);
		if (getClass () == DialogWrapper.class)
			mono.android.TypeManager.Activate ("RouteMeter.Dialogs.DialogWrapper, RouteMeter", "Android.Content.Context, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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

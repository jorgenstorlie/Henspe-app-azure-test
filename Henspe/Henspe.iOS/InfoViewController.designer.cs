// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Henspe.iOS
{
	[Register ("InfoViewController")]
	partial class InfoViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView actIndicator { get; set; }

		[Outlet]
		UIKit.UIWebView webView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}

			if (actIndicator != null) {
				actIndicator.Dispose ();
				actIndicator = null;
			}
		}
	}
}

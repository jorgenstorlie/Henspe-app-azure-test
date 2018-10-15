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
	[Register ("RegisterEmailViewController")]
	partial class RegisterEmailViewController
	{
		[Outlet]
		UIKit.UIButton btnOK { get; set; }

		[Outlet]
		UIKit.UILabel lblNewsletter { get; set; }

		[Outlet]
		UIKit.UITextField txtEmail { get; set; }

		[Action ("OKClicked:")]
		partial void OKClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (lblNewsletter != null) {
				lblNewsletter.Dispose ();
				lblNewsletter = null;
			}

			if (txtEmail != null) {
				txtEmail.Dispose ();
				txtEmail = null;
			}

			if (btnOK != null) {
				btnOK.Dispose ();
				btnOK = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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
            if (btnOK != null) {
                btnOK.Dispose ();
                btnOK = null;
            }

            if (lblNewsletter != null) {
                lblNewsletter.Dispose ();
                lblNewsletter = null;
            }

            if (txtEmail != null) {
                txtEmail.Dispose ();
                txtEmail = null;
            }
        }
    }
}
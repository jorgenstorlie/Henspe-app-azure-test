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
    [Register ("AddressCell")]
    partial class AddressCell
    {
        [Outlet]
        UIKit.UIImageView imgImageView { get; set; }


        [Outlet]
        UIKit.UILabel labAddressLine1 { get; set; }


        [Outlet]
        UIKit.UILabel labAddressLine2 { get; set; }


        [Outlet]
        UIKit.UILabel labAddressLine3 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgImageView != null) {
                imgImageView.Dispose ();
                imgImageView = null;
            }

            if (labAddressLine1 != null) {
                labAddressLine1.Dispose ();
                labAddressLine1 = null;
            }

            if (labAddressLine2 != null) {
                labAddressLine2.Dispose ();
                labAddressLine2 = null;
            }

            if (labAddressLine3 != null) {
                labAddressLine3.Dispose ();
                labAddressLine3 = null;
            }
        }
    }
}
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
    [Register ("InitialCardViewController")]
    partial class InitialCardViewController
    {
        [Outlet]
        UIKit.UIImageView imgImage { get; set; }

        [Outlet]
        UIKit.UILabel labHeader { get; set; }

        [Outlet]
        UIKit.UILabel labText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgImage != null) {
                imgImage.Dispose ();
                imgImage = null;
            }

            if (labHeader != null) {
                labHeader.Dispose ();
                labHeader = null;
            }

            if (labText != null) {
                labText.Dispose ();
                labText = null;
            }
        }
    }
}
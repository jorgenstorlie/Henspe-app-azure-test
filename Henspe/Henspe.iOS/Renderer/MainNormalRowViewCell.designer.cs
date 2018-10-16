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
    [Register ("MainNormalRowViewCell")]
    partial class MainNormalRowViewCell
    {
        [Outlet]
        UIKit.NSLayoutConstraint constraintImageWidth { get; set; }


        [Outlet]
        UIKit.UIImageView imgImage { get; set; }


        [Outlet]
        UIKit.UILabel labLabel { get; set; }


        [Outlet]
        UIKit.UIView viewImageContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (constraintImageWidth != null) {
                constraintImageWidth.Dispose ();
                constraintImageWidth = null;
            }

            if (imgImage != null) {
                imgImage.Dispose ();
                imgImage = null;
            }

            if (labLabel != null) {
                labLabel.Dispose ();
                labLabel = null;
            }

            if (viewImageContainer != null) {
                viewImageContainer.Dispose ();
                viewImageContainer = null;
            }
        }
    }
}
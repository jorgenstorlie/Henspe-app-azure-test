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
    [Register ("MainLocationRowViewCell")]
    partial class MainLocationRowViewCell
    {
        [Outlet]
        UIKit.UIImageView imgImage { get; set; }


        [Outlet]
        UIKit.UILabel labLabelBottom { get; set; }


        [Outlet]
        UIKit.UILabel labLabelTop { get; set; }


        [Outlet]
        UIKit.UIView viewImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imgImage != null) {
                imgImage.Dispose ();
                imgImage = null;
            }

            if (labLabelBottom != null) {
                labLabelBottom.Dispose ();
                labLabelBottom = null;
            }

            if (labLabelTop != null) {
                labLabelTop.Dispose ();
                labLabelTop = null;
            }
        }
    }
}
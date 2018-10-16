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
    [Register ("HeaderTableCell")]
    partial class HeaderTableCell
    {
        [Outlet]
        UIKit.UILabel labDescription { get; set; }


        [Outlet]
        UIKit.UILabel labHeadline { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (labDescription != null) {
                labDescription.Dispose ();
                labDescription = null;
            }

            if (labHeadline != null) {
                labHeadline.Dispose ();
                labHeadline = null;
            }
        }
    }
}
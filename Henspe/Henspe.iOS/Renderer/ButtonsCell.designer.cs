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
    [Register ("ButtonsCell")]
    partial class ButtonsCell
    {
        [Outlet]
        UIKit.UIButton btnLeftButton { get; set; }

        [Outlet]
        UIKit.UIButton btnMiddleButton { get; set; }

        [Outlet]
        UIKit.UIButton btnRightButton { get; set; }

        [Outlet]
        UIKit.UILabel labLeft { get; set; }

        [Outlet]
        UIKit.UILabel labMiddle { get; set; }

        [Outlet]
        UIKit.UILabel labRight { get; set; }

        [Action ("RightButtonClicked:")]
        partial void RightButtonClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnLeftButton != null) {
                btnLeftButton.Dispose ();
                btnLeftButton = null;
            }

            if (btnMiddleButton != null) {
                btnMiddleButton.Dispose ();
                btnMiddleButton = null;
            }

            if (btnRightButton != null) {
                btnRightButton.Dispose ();
                btnRightButton = null;
            }

            if (labLeft != null) {
                labLeft.Dispose ();
                labLeft = null;
            }

            if (labMiddle != null) {
                labMiddle.Dispose ();
                labMiddle = null;
            }

            if (labRight != null) {
                labRight.Dispose ();
                labRight = null;
            }
        }
    }
}
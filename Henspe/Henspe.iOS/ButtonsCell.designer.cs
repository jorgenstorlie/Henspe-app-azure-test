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

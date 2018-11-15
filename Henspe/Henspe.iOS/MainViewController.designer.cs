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
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UIButton btnHelpUs { get; set; }

        [Outlet]
        UIKit.NSLayoutConstraint constraintHelpUsHeight { get; set; }

        [Outlet]
        UIKit.UITableView myTableView { get; set; }

        [Action ("HelpUsClicked:")]
        partial void HelpUsClicked (Foundation.NSObject sender);

        [Action ("SettingsClicked:")]
        partial void SettingsClicked (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (btnHelpUs != null) {
                btnHelpUs.Dispose ();
                btnHelpUs = null;
            }

            if (constraintHelpUsHeight != null) {
                constraintHelpUsHeight.Dispose ();
                constraintHelpUsHeight = null;
            }

            if (myTableView != null) {
                myTableView.Dispose ();
                myTableView = null;
            }
        }
    }
}

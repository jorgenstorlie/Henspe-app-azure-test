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
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UIButton btnHelpUs { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint constraintHelpUsHeight { get; set; }


        [Outlet]
        UIKit.UITableView myTableView { get; set; }


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
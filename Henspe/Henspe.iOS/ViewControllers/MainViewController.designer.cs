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
    [Register("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        UIKit.UIButton btnConsent { get; set; }

        [Outlet]
        UIKit.NSLayoutConstraint constraintAreaOverTable { get; set; }

        [Outlet]
        UIKit.UITableView myTableView { get; set; }

        [Action("OnConsentClicked:")]
        partial void OnConsentClicked(Foundation.NSObject sender);

        [Action("OnSettingsClicked:")]
        partial void OnSettingsClicked(Foundation.NSObject sender);

        void ReleaseDesignerOutlets()
        {
            if (btnConsent != null)
            {
                btnConsent.Dispose();
                btnConsent = null;
            }

            if (constraintAreaOverTable != null)
            {
                constraintAreaOverTable.Dispose();
                constraintAreaOverTable = null;
            }

            if (myTableView != null)
            {
                myTableView.Dispose();
                myTableView = null;
            }
        }
    }
}

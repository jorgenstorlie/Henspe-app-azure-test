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
		UIKit.UITableView myTableView { get; set; }

		[Action ("OnInfoClicked:")]
		partial void OnInfoClicked (Foundation.NSObject sender);

		[Action ("OnSettingsClicked:")]
		partial void OnSettingsClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (myTableView != null) {
				myTableView.Dispose ();
				myTableView = null;
			}
		}
	}
}

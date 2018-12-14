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
	[Register ("SettingsCoordinatesTableCell")]
	partial class SettingsCoordinatesTableCell
	{
		[Outlet]
		UIKit.UILabel labInfo { get; set; }

		[Outlet]
		UIKit.UILabel labValue { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (labInfo != null) {
				labInfo.Dispose ();
				labInfo = null;
			}

			if (labValue != null) {
				labValue.Dispose ();
				labValue = null;
			}
		}
	}
}

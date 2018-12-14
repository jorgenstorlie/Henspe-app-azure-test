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
	[Register ("SettingsLocationTableCell")]
	partial class SettingsLocationTableCell
	{
		[Outlet]
		UIKit.UILabel labInfo { get; set; }

		[Outlet]
		UIKit.UILabel labLeft { get; set; }

		[Outlet]
		UIKit.UILabel labRight { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (labInfo != null) {
				labInfo.Dispose ();
				labInfo = null;
			}

			if (labLeft != null) {
				labLeft.Dispose ();
				labLeft = null;
			}

			if (labRight != null) {
				labRight.Dispose ();
				labRight = null;
			}
		}
	}
}

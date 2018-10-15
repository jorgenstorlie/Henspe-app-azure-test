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
	[Register ("HeaderTableCell")]
	partial class HeaderTableCell
	{
		[Outlet]
		UIKit.UILabel labDescription { get; set; }

		[Outlet]
		UIKit.UILabel labHeadline { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (labHeadline != null) {
				labHeadline.Dispose ();
				labHeadline = null;
			}

			if (labDescription != null) {
				labDescription.Dispose ();
				labDescription = null;
			}
		}
	}
}

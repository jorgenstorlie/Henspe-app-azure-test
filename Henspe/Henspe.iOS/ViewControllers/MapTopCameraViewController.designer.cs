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
	[Register ("MapTopCameraViewController")]
	partial class MapTopCameraViewController
	{
		[Outlet]
		UIKit.UIImageView imgImage { get; set; }

		[Outlet]
		UIKit.UILabel labHeader { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (labHeader != null) {
				labHeader.Dispose ();
				labHeader = null;
			}

			if (imgImage != null) {
				imgImage.Dispose ();
				imgImage = null;
			}
		}
	}
}

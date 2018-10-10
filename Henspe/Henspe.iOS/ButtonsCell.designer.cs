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
		UIKit.UIImageView imgLeftImage { get; set; }

		[Outlet]
		UIKit.UIImageView imgMiddleImage { get; set; }

		[Outlet]
		UIKit.UIImageView imgRightImage { get; set; }

		[Outlet]
		UIKit.UILabel labLeft { get; set; }

		[Outlet]
		UIKit.UILabel labMiddle { get; set; }

		[Outlet]
		UIKit.UILabel labRight { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgLeftImage != null) {
				imgLeftImage.Dispose ();
				imgLeftImage = null;
			}

			if (imgMiddleImage != null) {
				imgMiddleImage.Dispose ();
				imgMiddleImage = null;
			}

			if (imgRightImage != null) {
				imgRightImage.Dispose ();
				imgRightImage = null;
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

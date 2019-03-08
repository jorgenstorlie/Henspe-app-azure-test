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
	[Register ("MainLocationRowViewCell")]
	partial class MainLocationRowViewCell
	{
		[Outlet]
		UIKit.UIImageView imgImage { get; set; }

		[Outlet]
		UIKit.UILabel labLabelBottom { get; set; }

		[Outlet]
		UIKit.UILabel labLabelTop { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (imgImage != null) {
				imgImage.Dispose ();
				imgImage = null;
			}

			if (labLabelBottom != null) {
				labLabelBottom.Dispose ();
				labLabelBottom = null;
			}

			if (labLabelTop != null) {
				labLabelTop.Dispose ();
				labLabelTop = null;
			}
		}
	}
}

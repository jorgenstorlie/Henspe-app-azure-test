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
	[Register ("MapViewController")]
	partial class MapViewController
	{
		[Outlet]
		UIKit.UIButton btnMapType { get; set; }

		[Outlet]
		UIKit.UIButton btnZoomHome { get; set; }

		[Outlet]
		UIKit.UIView map { get; set; }

		[Outlet]
		UIKit.UIView viewInfo { get; set; }

		[Action ("OnBtnMapTypeClicked:")]
		partial void OnBtnMapTypeClicked (Foundation.NSObject sender);

		[Action ("OnBtnZoomHomeClicked:")]
		partial void OnBtnZoomHomeClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnMapType != null) {
				btnMapType.Dispose ();
				btnMapType = null;
			}

			if (btnZoomHome != null) {
				btnZoomHome.Dispose ();
				btnZoomHome = null;
			}

			if (viewInfo != null) {
				viewInfo.Dispose ();
				viewInfo = null;
			}

			if (map != null) {
				map.Dispose ();
				map = null;
			}
		}
	}
}

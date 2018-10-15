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
	[Register ("SegmentedCell")]
	partial class SegmentedCell
	{
		[Outlet]
		UIKit.UISegmentedControl segmentedController { get; set; }

		[Action ("SegmentedValueChanged:")]
		partial void SegmentedValueChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (segmentedController != null) {
				segmentedController.Dispose ();
				segmentedController = null;
			}
		}
	}
}

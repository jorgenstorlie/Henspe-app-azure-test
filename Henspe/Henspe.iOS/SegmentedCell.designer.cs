// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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
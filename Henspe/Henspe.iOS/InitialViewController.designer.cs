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
    [Register ("InitialViewController")]
    partial class InitialViewController
    {
        [Outlet]
        UIKit.UIActivityIndicatorView actActivityIndicator { get; set; }


        [Outlet]
        UIKit.UIButton btnNext { get; set; }


        [Outlet]
        UIKit.UIButton btnSkip { get; set; }


        [Outlet]
        UIKit.UIPageControl pagPager { get; set; }


        [Outlet]
        UIKit.UIView viewAnimation { get; set; }


        [Action ("OnNextClicked:")]
        partial void OnNextClicked (Foundation.NSObject sender);


        [Action ("OnSkipClicked:")]
        partial void OnSkipClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (actActivityIndicator != null) {
                actActivityIndicator.Dispose ();
                actActivityIndicator = null;
            }

            if (btnNext != null) {
                btnNext.Dispose ();
                btnNext = null;
            }

            if (btnSkip != null) {
                btnSkip.Dispose ();
                btnSkip = null;
            }

            if (pagPager != null) {
                pagPager.Dispose ();
                pagPager = null;
            }

            if (viewAnimation != null) {
                viewAnimation.Dispose ();
                viewAnimation = null;
            }
        }
    }
}
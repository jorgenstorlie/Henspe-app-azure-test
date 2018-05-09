// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Henspe.iOS
{
    [Register("InitialViewController")]
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
        UIKit.UIScrollView scrScrollView { get; set; }

        [Action("OnNextClicked:")]
        partial void OnNextClicked(Foundation.NSObject sender);

        [Action("OnSkipClicked:")]
        partial void OnSkipClicked(Foundation.NSObject sender);

        void ReleaseDesignerOutlets()
        {
            if (btnNext != null)
            {
                btnNext.Dispose();
                btnNext = null;
            }

            if (btnSkip != null)
            {
                btnSkip.Dispose();
                btnSkip = null;
            }

            if (pagPager != null)
            {
                pagPager.Dispose();
                pagPager = null;
            }

            if (scrScrollView != null)
            {
                scrScrollView.Dispose();
                scrScrollView = null;
            }

            if (actActivityIndicator != null)
            {
                actActivityIndicator.Dispose();
                actActivityIndicator = null;
            }
        }
    }
}

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
	[Register ("OnboardingViewController")]
	partial class OnboardingViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView actActivityIndicator { get; set; }

		[Outlet]
		UIKit.UIButton btnNext { get; set; }

		[Outlet]
		UIKit.UIButton btnSkip { get; set; }

		[Outlet]
		UIKit.UILabel labDescription { get; set; }

		[Outlet]
		UIKit.UILabel labHeader { get; set; }

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

			if (labHeader != null) {
				labHeader.Dispose ();
				labHeader = null;
			}

			if (pagPager != null) {
				pagPager.Dispose ();
				pagPager = null;
			}

			if (viewAnimation != null) {
				viewAnimation.Dispose ();
				viewAnimation = null;
			}

			if (labDescription != null) {
				labDescription.Dispose ();
				labDescription = null;
			}
		}
	}
}

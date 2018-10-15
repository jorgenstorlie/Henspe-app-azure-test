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
	[Register ("HelpUsViewController")]
	partial class HelpUsViewController
	{
		[Outlet]
		UIKit.UIButton btnEmail { get; set; }

		[Outlet]
		UIKit.UIButton btnNoThankYou { get; set; }

		[Outlet]
		UIKit.UIButton btnSMS { get; set; }

		[Outlet]
		UIKit.UIImageView imgArrow { get; set; }

		[Outlet]
		UIKit.UILabel labShowMore { get; set; }

		[Outlet]
		UIKit.UILabel lblAboutHENSPE { get; set; }

		[Outlet]
		UIKit.UILabel lblAboutSNLA { get; set; }

		[Outlet]
		UIKit.UILabel lblYesPlease { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollViewAboutSNLA { get; set; }

		[Outlet]
		UIKit.UIView viewAboutContainer { get; set; }

		[Outlet]
		UIKit.UIView viewMovieContainer { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnNoThankYou != null) {
				btnNoThankYou.Dispose ();
				btnNoThankYou = null;
			}

			if (btnSMS != null) {
				btnSMS.Dispose ();
				btnSMS = null;
			}

			if (btnEmail != null) {
				btnEmail.Dispose ();
				btnEmail = null;
			}

			if (lblAboutSNLA != null) {
				lblAboutSNLA.Dispose ();
				lblAboutSNLA = null;
			}

			if (viewAboutContainer != null) {
				viewAboutContainer.Dispose ();
				viewAboutContainer = null;
			}

			if (scrollViewAboutSNLA != null) {
				scrollViewAboutSNLA.Dispose ();
				scrollViewAboutSNLA = null;
			}

			if (lblAboutHENSPE != null) {
				lblAboutHENSPE.Dispose ();
				lblAboutHENSPE = null;
			}

			if (viewMovieContainer != null) {
				viewMovieContainer.Dispose ();
				viewMovieContainer = null;
			}

			if (imgArrow != null) {
				imgArrow.Dispose ();
				imgArrow = null;
			}

			if (labShowMore != null) {
				labShowMore.Dispose ();
				labShowMore = null;
			}

			if (lblYesPlease != null) {
				lblYesPlease.Dispose ();
				lblYesPlease = null;
			}
		}
	}
}

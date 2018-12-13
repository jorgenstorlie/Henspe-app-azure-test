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
		UIKit.UIButton btnAccept { get; set; }

		[Outlet]
		UIKit.UIButton btnNoThankYou { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintEmailHeight { get; set; }

		[Outlet]
		UIKit.UIImageView imgArrow { get; set; }

		[Outlet]
		UIKit.UILabel labShowMore { get; set; }

		[Outlet]
		UIKit.UILabel lblAboutHENSPE { get; set; }

		[Outlet]
		UIKit.UILabel lblAboutSNLA { get; set; }

		[Outlet]
		UIKit.UILabel lblEmail { get; set; }

		[Outlet]
		UIKit.UILabel lblSMS { get; set; }

		[Outlet]
		UIKit.UILabel lblYesPlease { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollViewAboutSNLA { get; set; }

		[Outlet]
		UIKit.UISwitch swtEmail { get; set; }

		[Outlet]
		UIKit.UISwitch swtSms { get; set; }

		[Outlet]
		UIKit.UITextField txtEpost { get; set; }

		[Outlet]
		UIKit.UIView viewAboutContainer { get; set; }

		[Outlet]
		UIKit.UIView viewMovieContainer { get; set; }

		[Action ("EmailChanged:")]
		partial void EmailChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAccept != null) {
				btnAccept.Dispose ();
				btnAccept = null;
			}

			if (btnNoThankYou != null) {
				btnNoThankYou.Dispose ();
				btnNoThankYou = null;
			}

			if (constraintEmailHeight != null) {
				constraintEmailHeight.Dispose ();
				constraintEmailHeight = null;
			}

			if (imgArrow != null) {
				imgArrow.Dispose ();
				imgArrow = null;
			}

			if (labShowMore != null) {
				labShowMore.Dispose ();
				labShowMore = null;
			}

			if (lblAboutHENSPE != null) {
				lblAboutHENSPE.Dispose ();
				lblAboutHENSPE = null;
			}

			if (lblAboutSNLA != null) {
				lblAboutSNLA.Dispose ();
				lblAboutSNLA = null;
			}

			if (lblEmail != null) {
				lblEmail.Dispose ();
				lblEmail = null;
			}

			if (lblSMS != null) {
				lblSMS.Dispose ();
				lblSMS = null;
			}

			if (lblYesPlease != null) {
				lblYesPlease.Dispose ();
				lblYesPlease = null;
			}

			if (scrollViewAboutSNLA != null) {
				scrollViewAboutSNLA.Dispose ();
				scrollViewAboutSNLA = null;
			}

			if (swtEmail != null) {
				swtEmail.Dispose ();
				swtEmail = null;
			}

			if (swtSms != null) {
				swtSms.Dispose ();
				swtSms = null;
			}

			if (txtEpost != null) {
				txtEpost.Dispose ();
				txtEpost = null;
			}

			if (viewAboutContainer != null) {
				viewAboutContainer.Dispose ();
				viewAboutContainer = null;
			}

			if (viewMovieContainer != null) {
				viewMovieContainer.Dispose ();
				viewMovieContainer = null;
			}
		}
	}
}

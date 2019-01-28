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
	[Register ("ConsentViewController")]
	partial class ConsentViewController
	{
		[Outlet]
		UIKit.UIButton btnAccept { get; set; }

		[Outlet]
		UIKit.UIButton btnDeny { get; set; }

		[Outlet]
		UIKit.UIButton btnReadMore { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintBottom { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintContentHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintEmailHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintNoThankYouHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintVideoHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint constraintVideoWidth { get; set; }

		[Outlet]
		UIKit.UILabel labConsentHeader { get; set; }

		[Outlet]
		UIKit.UILabel labEmail { get; set; }

		[Outlet]
		UIKit.UILabel labHeader { get; set; }

		[Outlet]
		UIKit.UILabel labSMS { get; set; }

		[Outlet]
		UIKit.UIScrollView scrView { get; set; }

		[Outlet]
		UIKit.UISwitch swiEmail { get; set; }

		[Outlet]
		UIKit.UISwitch swiSMS { get; set; }

		[Outlet]
		UIKit.UITextField txtEmail { get; set; }

		[Outlet]
		UIKit.UIView viewContent { get; set; }

		[Outlet]
		UIKit.UIView viewMovieContainer { get; set; }

		[Action ("OnAcceptClicked:")]
		partial void OnAcceptClicked (Foundation.NSObject sender);

		[Action ("OnDenyClicked:")]
		partial void OnDenyClicked (Foundation.NSObject sender);

		[Action ("OnEmailEditingChanged:")]
		partial void OnEmailEditingChanged (Foundation.NSObject sender);

		[Action ("OnEmailShouldBeginEditing:")]
		partial void OnEmailShouldBeginEditing (Foundation.NSObject sender);

		[Action ("OnEmailShouldReturn:")]
		partial void OnEmailShouldReturn (Foundation.NSObject sender);

		[Action ("OnReadMoreClicked:")]
		partial void OnReadMoreClicked (Foundation.NSObject sender);

		[Action ("OnSwiEmailChanged:")]
		partial void OnSwiEmailChanged (Foundation.NSObject sender);

		[Action ("OnSwiSMSChanged:")]
		partial void OnSwiSMSChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (constraintBottom != null) {
				constraintBottom.Dispose ();
				constraintBottom = null;
			}

			if (btnAccept != null) {
				btnAccept.Dispose ();
				btnAccept = null;
			}

			if (btnDeny != null) {
				btnDeny.Dispose ();
				btnDeny = null;
			}

			if (btnReadMore != null) {
				btnReadMore.Dispose ();
				btnReadMore = null;
			}

			if (constraintContentHeight != null) {
				constraintContentHeight.Dispose ();
				constraintContentHeight = null;
			}

			if (constraintEmailHeight != null) {
				constraintEmailHeight.Dispose ();
				constraintEmailHeight = null;
			}

			if (constraintNoThankYouHeight != null) {
				constraintNoThankYouHeight.Dispose ();
				constraintNoThankYouHeight = null;
			}

			if (constraintVideoHeight != null) {
				constraintVideoHeight.Dispose ();
				constraintVideoHeight = null;
			}

			if (constraintVideoWidth != null) {
				constraintVideoWidth.Dispose ();
				constraintVideoWidth = null;
			}

			if (labConsentHeader != null) {
				labConsentHeader.Dispose ();
				labConsentHeader = null;
			}

			if (labEmail != null) {
				labEmail.Dispose ();
				labEmail = null;
			}

			if (labHeader != null) {
				labHeader.Dispose ();
				labHeader = null;
			}

			if (labSMS != null) {
				labSMS.Dispose ();
				labSMS = null;
			}

			if (scrView != null) {
				scrView.Dispose ();
				scrView = null;
			}

			if (swiEmail != null) {
				swiEmail.Dispose ();
				swiEmail = null;
			}

			if (swiSMS != null) {
				swiSMS.Dispose ();
				swiSMS = null;
			}

			if (txtEmail != null) {
				txtEmail.Dispose ();
				txtEmail = null;
			}

			if (viewContent != null) {
				viewContent.Dispose ();
				viewContent = null;
			}

			if (viewMovieContainer != null) {
				viewMovieContainer.Dispose ();
				viewMovieContainer = null;
			}
		}
	}
}

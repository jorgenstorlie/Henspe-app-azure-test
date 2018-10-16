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
	[Register ("SettingsViewController")]
	partial class SettingsViewController
	{
		[Outlet]
		UIKit.UITableViewCell cellContact { get; set; }

		[Outlet]
		UIKit.UITableViewCell cellCoord { get; set; }

		[Outlet]
		UIKit.UITableViewCell cellPlace { get; set; }

		[Outlet]
		UIKit.UITableViewCell cellTerms { get; set; }

		[Outlet]
		UIKit.UILabel lblContactDescription { get; set; }

		[Outlet]
		UIKit.UILabel lblContactSubDescription { get; set; }

		[Outlet]
		UIKit.UILabel lblContactValue { get; set; }

		[Outlet]
		UIKit.UILabel lblCoordDescription { get; set; }

		[Outlet]
		UIKit.UILabel lblCoordValue { get; set; }

		[Outlet]
		UIKit.UILabel lblPlaceDescription { get; set; }

		[Outlet]
		UIKit.UILabel lblPlaceSubDescription { get; set; }

		[Outlet]
		UIKit.UILabel lblPlaceValue { get; set; }

		[Outlet]
		UIKit.UILabel lblTerms { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cellCoord != null) {
				cellCoord.Dispose ();
				cellCoord = null;
			}

			if (cellPlace != null) {
				cellPlace.Dispose ();
				cellPlace = null;
			}

			if (cellContact != null) {
				cellContact.Dispose ();
				cellContact = null;
			}

			if (cellTerms != null) {
				cellTerms.Dispose ();
				cellTerms = null;
			}

			if (lblContactDescription != null) {
				lblContactDescription.Dispose ();
				lblContactDescription = null;
			}

			if (lblContactSubDescription != null) {
				lblContactSubDescription.Dispose ();
				lblContactSubDescription = null;
			}

			if (lblContactValue != null) {
				lblContactValue.Dispose ();
				lblContactValue = null;
			}

			if (lblCoordDescription != null) {
				lblCoordDescription.Dispose ();
				lblCoordDescription = null;
			}

			if (lblCoordValue != null) {
				lblCoordValue.Dispose ();
				lblCoordValue = null;
			}

			if (lblPlaceDescription != null) {
				lblPlaceDescription.Dispose ();
				lblPlaceDescription = null;
			}

			if (lblPlaceSubDescription != null) {
				lblPlaceSubDescription.Dispose ();
				lblPlaceSubDescription = null;
			}

			if (lblPlaceValue != null) {
				lblPlaceValue.Dispose ();
				lblPlaceValue = null;
			}

			if (lblTerms != null) {
				lblTerms.Dispose ();
				lblTerms = null;
			}
		}
	}
}

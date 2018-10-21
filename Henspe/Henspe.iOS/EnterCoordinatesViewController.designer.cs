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
	[Register ("EnterCoordinatesViewController")]
	partial class EnterCoordinatesViewController
	{
		[Outlet]
		UIKit.UIImageView imgMapCenter { get; set; }

		[Outlet]
		UIKit.UILabel lblAddress { get; set; }

		[Outlet]
		UIKit.UILabel lblDegreesEast { get; set; }

		[Outlet]
		UIKit.UILabel lblDegreesNorth { get; set; }

		[Outlet]
		UIKit.UILabel lblEnterPosition { get; set; }

		[Outlet]
		MapKit.MKMapView mapView { get; set; }

		[Outlet]
		UIKit.UISearchBar searchBar { get; set; }

		[Outlet]
		UIKit.UIView searchContainerView { get; set; }

		[Outlet]
		UIKit.UITextField txtAddress { get; set; }

		[Outlet]
		UIKit.UITextField txtDegreesEast { get; set; }

		[Outlet]
		UIKit.UITextField txtDegreesNorth { get; set; }

		[Outlet]
		UIKit.UIView viewContainer { get; set; }

		[Action ("latitudeChanged:")]
		partial void latitudeChanged (Foundation.NSObject sender);

		[Action ("longitudeChanged:")]
		partial void longitudeChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (viewContainer != null) {
				viewContainer.Dispose ();
				viewContainer = null;
			}

			if (imgMapCenter != null) {
				imgMapCenter.Dispose ();
				imgMapCenter = null;
			}

			if (lblAddress != null) {
				lblAddress.Dispose ();
				lblAddress = null;
			}

			if (lblDegreesEast != null) {
				lblDegreesEast.Dispose ();
				lblDegreesEast = null;
			}

			if (lblDegreesNorth != null) {
				lblDegreesNorth.Dispose ();
				lblDegreesNorth = null;
			}

			if (lblEnterPosition != null) {
				lblEnterPosition.Dispose ();
				lblEnterPosition = null;
			}

			if (mapView != null) {
				mapView.Dispose ();
				mapView = null;
			}

			if (searchBar != null) {
				searchBar.Dispose ();
				searchBar = null;
			}

			if (searchContainerView != null) {
				searchContainerView.Dispose ();
				searchContainerView = null;
			}

			if (txtAddress != null) {
				txtAddress.Dispose ();
				txtAddress = null;
			}

			if (txtDegreesEast != null) {
				txtDegreesEast.Dispose ();
				txtDegreesEast = null;
			}

			if (txtDegreesNorth != null) {
				txtDegreesNorth.Dispose ();
				txtDegreesNorth = null;
			}
		}
	}
}

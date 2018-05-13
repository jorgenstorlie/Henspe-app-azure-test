// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Henspe.Core.Const;
using Henspe.Core.Model.Dto;
using Henspe.iOS.AppModel;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using UIKit;

namespace Henspe.iOS
{
	public partial class MainViewController : UIViewController
	{
		private MainListTableViewSource mainListTableViewSource = null;

		// Events
		NSObject observerActivatedOccured;

		private UIStringAttributes redText = new UIStringAttributes
		{
			ForegroundColor = ColorConst.textRed,
			Font = FontConst.fontMedium
		};

		private UIStringAttributes blackText = new UIStringAttributes
		{
			ForegroundColor = ColorConst.textGrayColor,
			Font = FontConst.fontMedium
		};

		public MainViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//this.Title = Foundation.NSBundle.MainBundle.LocalizedString("Kystvarsel.Title", null);

			SetupNavigationBar();
			SetupView();

			// Events
			//observerActivatedOccured = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventsConst.activatedOccured), HandleActivatedOccured);
		}

		private void SetupNavigationBar()
		{
			// Transparent background
			UIImage emptyImage = new UIImage();
			this.NavigationController.NavigationBar.Translucent = true;
			this.NavigationController.NavigationBar.SetBackgroundImage(emptyImage, UIBarMetrics.Default);
			this.NavigationController.NavigationBar.ShadowImage = emptyImage;

            // Logo
			UIImage imgLogo = UIImage.FromFile("ic_snla.png");
			UIImageView imgViewLogo = new UIImageView(imgLogo);
			this.NavigationItem.TitleView = imgViewLogo;
		}

		partial void OnSettingsClicked(NSObject sender)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
		}

		/*
        public void HandleActivatedOccured(NSNotification notification)
        {
            DoCallKystvarsel();
        }
        */

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();
		}

		public override void WillMoveToParentViewController(UIViewController parent)
		{
			base.WillMoveToParentViewController(parent);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}

		public override void ViewDidAppear(bool animated)
		{
			//base.ViewDidAppear(animated);

			StartGPSIfNotStartedAlreadyAfterActivated();
			//SetupGlowTimers();
		}

		private void StartGPSIfNotStartedAlreadyAfterActivated()
		{
			AppDelegate.current.gpsStatus = CLLocationManager.Status;

			if (AppDelegate.current.gpsStarted == false)
			{
				// GPG motor was not started. Restart it.
				StartGPSTracking();
			}
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

		private void SetupView()
		{
			ResetGPSVariables();

			// Table setup
			mainListTableViewSource = new MainListTableViewSource(this);
			myTableView.Source = mainListTableViewSource;
			myTableView.BackgroundColor = UIColor.Clear;
			myTableView.SeparatorColor = UIColor.Clear;
			this.AutomaticallyAdjustsScrollViewInsets = false;

			mainListTableViewSource.sectionsWithRows = AppDelegate.current.structure;

			myTableView.ReloadData();
		}

		private void ResetGPSVariables()
		{
			AppDelegate.current.gpsPosFound = false;
			AppDelegate.current.currentLocation = null;
			AppDelegate.current.lastAddressLocation = null;
		}

		#region GPS
		private void StartGPSTracking()
		{
			SetupPosition(PositionTypeConst.finding);

			if (AppDelegate.current.iPhoneLocationManager == null)
			{
				AppDelegate.current.iPhoneLocationManager = new CLLocationManager
				{
					ActivityType = CLActivityType.OtherNavigation,
				};
			}
			else
			{
				AppDelegate.current.iPhoneLocationManager.StopUpdatingLocation();
			}

			AppDelegate.current.iPhoneLocationManager.DesiredAccuracy = AppDelegate.current.desiredAccuracy;
			AppDelegate.current.iPhoneLocationManager.DistanceFilter = AppDelegate.current.distanceFilter;

			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				AppDelegate.current.iPhoneLocationManager.LocationsUpdated += HandleLocationsUpdated;
			}
			else
			{
				// This will not be called on iOS 6. Depricated
			}

			// iOS 8 requires you to manually request authorization
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				AppDelegate.current.iPhoneLocationManager.RequestWhenInUseAuthorization();
			}

			AppDelegate.current.gpsStatus = CLLocationManager.Status;

			// start updating our location, et. al.
			if (CLLocationManager.Status != CLAuthorizationStatus.NotDetermined &&
				CLLocationManager.Status != CLAuthorizationStatus.Authorized &&
				CLLocationManager.Status != CLAuthorizationStatus.AuthorizedAlways &&
				CLLocationManager.Status != CLAuthorizationStatus.AuthorizedWhenInUse)
			{
				// Something is wrong with the GPS grant
				SetupPosition(PositionTypeConst.error);

				AppDelegate.current.gpsStarted = false;

				InvokeOnMainThread(delegate
				{
					UIAlertView alert = new UIAlertView(Foundation.NSBundle.MainBundle.LocalizedString("Alert.Title.Warning", null),
					Foundation.NSBundle.MainBundle.LocalizedString("GPS.NoAccessToGPS.Message", null),
					null,
					Foundation.NSBundle.MainBundle.LocalizedString("Alert.OK", null),
					null);

					alert.Clicked += (s, b) =>
					{
						if (b.ButtonIndex == 0)
						{
							// Yes chosen. Stop 
							AppDelegate.current.iPhoneLocationManager.StopUpdatingLocation();
						}
					};

					alert.Show();
				});
			}
			else if (CLLocationManager.LocationServicesEnabled)
			{
				AppDelegate.current.iPhoneLocationManager.StartUpdatingLocation();
				AppDelegate.current.gpsStarted = true;

				//SetupGlowTimers();
			}
		}

		private void SetupPosition(int positionType)
		{
			/*
            if (positionType != AppDelegate.current.lastPositionType)
            {
                if (positionType == PositionTypeConst.error)
                    imgLocationOff.Image = UIImage.FromFile("ic_pin_yellow_error.png");
                else
                    imgLocationOff.Image = UIImage.FromFile("ic_pin_yellow.png");
                    
                lastPositionType = positionType;

				if (positionType == PositionTypeConst.off || positionType == PositionTypeConst.error)
                {
                    UIView.Animate(0.8f, 0, UIViewAnimationOptions.CurveEaseInOut,
                        () =>
                        {
                            imgLocationOn.Alpha = 0.0f;
                        },
                        () =>
                        {
                            imgLocationOn.Alpha = 0.0f;
                        });

                    PositionTextUnknown();
                }
                else if (positionType == PositionTypeConst.finding)
                {
                    imgLocationOn.Alpha = 0.1f;
                    UIView.Animate(0.8f, 0, UIViewAnimationOptions.Repeat | UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.Autoreverse,
                        () =>
                        {
                            imgLocationOn.Alpha = 0.5f;
                        },
                        null);

                    imgLocationOn.AccessibilityLabel = NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.MainViewController.Accessibility.Location.Finding", null);

                    PositionTextUnknown();
                }
                else if (positionType == PositionTypeConst.found)
                {
                    UIView.Animate(0.8f, 0, UIViewAnimationOptions.CurveEaseInOut,
                        () =>
                        {
                            imgLocationOn.Alpha = 1.0f;
                        },
                        () =>
                        {
                            imgLocationOn.Alpha = 1.0f;
                        });

                    imgLocationOn.AccessibilityLabel = NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.MainViewController.Accessibility.Location.Found", null);
                }
                else
                {
                    Console.WriteLine("Error: Illegal PositionType entered in MainViewController");
                    imgLocationOn.Alpha = 0.0f;
                }
            }
			*/
		}

		private void HandleLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
		{
			UpdateLocation(e);
		}

		private void UpdateLocation(CLLocationsUpdatedEventArgs e)
		{
			if (AppDelegate.current.iPhoneLocationManager != null)
			{
				AppDelegate.current.gpsPosFound = true;

				AppDelegate.current.currentLocation = e.Locations[e.Locations.Length - 1];

				double roundedLatitude = Math.Floor(AppDelegate.current.currentLocation.Coordinate.Latitude);
				double roundedLongitude = Math.Floor(AppDelegate.current.currentLocation.Coordinate.Longitude);

				// If location is cupertino, we are in simulator. Lets set it to Norway to speed things up
				if (AppDelegate.current.roundedLatitude == POIConst.cupertinoLatitude && AppDelegate.current.roundedLongitude == POIConst.cupertinoLongitude)
				{
					AppDelegate.current.currentLocation = new CLLocation(POIConst.stabekkLatitude, POIConst.stabekkLongitude);
				}

				int previousGpsCoverage = AppDelegate.current.gpsCoverage;
				int newGpsCoverage = GpsCoverageConst.none;

				newGpsCoverage = iOSMapUtil.GetAccuracyType(AppDelegate.current.currentLocation.HorizontalAccuracy, AppDelegate.current.highAndLowAccuracyDivider, GpsCoverageConst.low, GpsCoverageConst.high);

				AppDelegate.current.gpsCoverage = newGpsCoverage;

				GPSObject gpsObject = new GPSObject();
				gpsObject.accuracy = AppDelegate.current.currentLocation.HorizontalAccuracy;
				gpsObject.gpsCoordinates = AppDelegate.current.currentLocation.Coordinate;
				gpsObject.storedDateTime = DateTime.Now;
				AppDelegate.current.gpsCurrentPositionObject = gpsObject;

				AppDelegate.current.gpsEventOccured = true;

				NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.gpsEvent, this);

				if (AppDelegate.current.currentLocation.HorizontalAccuracy <= AppDelegate.current.gpsAccuracyRequirement)
				{
					SetupPosition(PositionTypeConst.found);
				}
				else
				{
					SetupPosition(PositionTypeConst.finding);
				}

				//Console.WriteLine ("Location updated");
				bool positionSignificantlyChanged = UpdateGPSPositionAndAlsoAddressIfSignificantChange();
			}
			else
			{
				StartGPSTracking();
			}
		}

		private bool UpdateGPSPositionAndAlsoAddressIfSignificantChange()
		{
			double lat1 = AppDelegate.current.currentLocation.Coordinate.Latitude;
			double lon1 = AppDelegate.current.currentLocation.Coordinate.Longitude;
			double lat2 = 0;
			double lon2 = 0;

			if (AppDelegate.current.lastAddressLocation != null)
			{
				lat2 = AppDelegate.current.lastAddressLocation.Coordinate.Latitude;
				lon2 = AppDelegate.current.lastAddressLocation.Coordinate.Longitude;
			}

			if (iOSMapUtil.Distance(lat1, lon1, lat2, lon2) > AppDelegate.current.distanceToUpdateAddress || (lat2 == 0 && lon2 == 0))
			{
				AppDelegate.current.lastAddressLocation = AppDelegate.current.currentLocation;

				// Refresh position
                RefreshTableRow(1, 0);

                // Refresh address
                RefreshTableRow(1, 1);

				return true;
			}

			return false;
		}

		private void RefreshTableRow(int section, int row)
        {
			NSIndexPath[] indexPathList = new NSIndexPath[] { NSIndexPath.FromRowSection(row, section) };
			myTableView.ReloadRows(indexPathList, UITableViewRowAnimation.Fade);
        }
        #endregion
	}

    // Table view source
    public partial class MainListTableViewSource : UITableViewSource
    {
		private int headerHeight = 70;
		private WeakReference<MainViewController> _parent;

		private string lastPositionText = "";
		private string lastAddressText = "";

        public StructureDto sectionsWithRows;

		public MainListTableViewSource(MainViewController controller)
        {
			_parent = new WeakReference<MainViewController>(controller);
        }

        // UITablViewSource methods
        public override nint NumberOfSections(UITableView tableView)
        {
            if (sectionsWithRows != null)
            {
				return sectionsWithRows.structureSectionList.Count;
            }
            else
            {
                return 0;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
			if(sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
			{
				StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

				if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
				{
					return structureSection.structureElementList.Count;
				}
				else
				{
					return 0;
				}               
			}
			else
			{
				return 0;
			}
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
			if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
            {
                StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

                if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
                {
					return headerHeight;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
			StructureSectionDto structureSection = null;

			if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
			{
				structureSection = sectionsWithRows.structureSectionList[(int)section];
			}
			else
			{
				return null;
			}

            CGRect headerframe = new CGRect(0, 0, tableView.Bounds.Size.Width, headerHeight);
            UIView headerView = new UIView(headerframe);

			headerView.BackgroundColor = UIColor.White;

			// Image
			CGRect imageFrame = new CGRect(15, headerHeight - 40 - 8, 40, 40);
			UIImageView imageView = new UIImageView(imageFrame);
			imageView.Image = UIImage.FromFile(structureSection.image);

			headerView.AddSubview(imageView);

            // Label
			CGRect labelFrame = new CGRect(63, 12, tableView.Bounds.Size.Width - 10, headerHeight - 9);
            UILabel label = new UILabel(labelFrame);
			label.Font = FontConst.fontHeading;
			label.TextColor = ColorConst.textColor;
			label.Text = structureSection.description;

            headerView.AddSubview(label);

			// Separator line
			CGRect viewFrame = new CGRect(0, headerHeight - 1, tableView.Bounds.Size.Width, 1);
			UIView separatorLine = new UIView(viewFrame);
			separatorLine.BackgroundColor = ColorConst.separatorColor;

			headerView.AddSubview(separatorLine);

            return headerView;
        }
        
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            const string cellIdentifier1 = "cellId1";
			const string cellIdentifier2 = "cellId2";

			int section = indexPath.Section;
			int row = indexPath.Row;

			StructureElementDto structureElement = null;

			if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
			{
				StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

				if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
				{
					structureElement = structureSection.structureElementList[row];
				}
			}

			if (structureElement == null)
				return 0;

			if(structureElement.elementType == StructureElementDto.ElementType.Normal)
			{
				MainNormalRowViewCell mainNormalRowViewCell = tableView.DequeueReusableCell(cellIdentifier1) as MainNormalRowViewCell;
				return 45;
				//return mainNormalRowViewCell.Bounds.Height;
			}
			else
			{
				MainLocationRowViewCell mainLocationRowViewCell = tableView.DequeueReusableCell(cellIdentifier2) as MainLocationRowViewCell;

				if (mainLocationRowViewCell.LabLabelBottom.Text != null)
                {
					NSString cellText = new NSString(mainLocationRowViewCell.LabLabelBottom.Text);
					UIFont font = mainLocationRowViewCell.LabLabelBottom.Font;
                    float width = (float)tableView.Frame.Width - 63.0f - 15.0f;

                    CGSize constraintSize = new CGSize(width, float.MaxValue);
                    CGSize labelSize = cellText.StringSize(font, constraintSize, UILineBreakMode.WordWrap);
					nfloat height = labelSize.Height + (85 - 21);
					return height;
                }
				else
				{
					return 0;
				}
			}
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
			const string cellIdentifier1 = "cellId1";
            const string cellIdentifier2 = "cellId2";

            int section = indexPath.Section;
            int row = indexPath.Row;

            StructureElementDto structureElement = null;

            if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
            {
                StructureSectionDto structureSection = sectionsWithRows.structureSectionList[(int)section];

                if (structureSection.structureElementList != null && structureSection.structureElementList.Count > 0)
                {
                    structureElement = structureSection.structureElementList[row];
                }
            }

            if (structureElement == null)
                return null;

            if (structureElement.elementType == StructureElementDto.ElementType.Normal)
            {
				// Normal row
                MainNormalRowViewCell mainNormalRowViewCell = tableView.DequeueReusableCell(cellIdentifier1) as MainNormalRowViewCell;

				mainNormalRowViewCell.LabLabel.TextColor = ColorConst.textColor;
				mainNormalRowViewCell.LabLabel.Text = structureElement.description;

				mainNormalRowViewCell.ImgImage.Image = UIImage.FromFile(structureElement.image);

				nfloat width = mainNormalRowViewCell.ViewImage.Frame.Width * structureElement.percent;
				nfloat height = mainNormalRowViewCell.ViewImage.Frame.Height * structureElement.percent;
				nfloat x = (mainNormalRowViewCell.ViewImage.Frame.Width / 2) - (width / 2);
				nfloat y = (mainNormalRowViewCell.ViewImage.Frame.Height / 2) - (height / 2);

				mainNormalRowViewCell.ImgImage.Frame = new CGRect(x, y, width, height);

				return mainNormalRowViewCell;
            }
            else
            {
				// Location row
                MainLocationRowViewCell mainLocationRowViewCell = tableView.DequeueReusableCell(cellIdentifier2) as MainLocationRowViewCell;

				mainLocationRowViewCell.LabLabelTop.TextColor = ColorConst.textColor;
				mainLocationRowViewCell.LabLabelTop.Text = structureElement.description;

				mainLocationRowViewCell.LabLabelBottom.TextColor = ColorConst.textGrayColor;
			
                if(section == 1 && row == 0)
				{
					// Position
					UpdatePosition(mainLocationRowViewCell.LabLabelBottom);
				}
				else if(section == 1 && row == 1)
				{
					// Address
					UpdateAddress(mainLocationRowViewCell.LabLabelBottom);
                }

				mainLocationRowViewCell.ImgImage.Image = UIImage.FromFile(structureElement.image);

				nfloat width = mainLocationRowViewCell.ViewImage.Frame.Width * structureElement.percent;
				nfloat height = mainLocationRowViewCell.ViewImage.Frame.Height * structureElement.percent;
				nfloat x = (mainLocationRowViewCell.ViewImage.Frame.Width / 2) - (width / 2);
				nfloat y = (mainLocationRowViewCell.ViewImage.Frame.Height / 2) - (height / 2);
                                                
				mainLocationRowViewCell.ImgImage.Frame = new CGRect(x, y, width, height);

				return mainLocationRowViewCell;
            }
        }

		private void UpdatePosition(UILabel labLabelBottom)
        {
			labLabelBottom.Text = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownPosition", null);

            if (AppDelegate.current.gpsCurrentPositionObject != null)
            {
                string latitudeText = AppDelegate.current.gpsCurrentPositionObject.latitudeDescription;
                string longitudeText = AppDelegate.current.gpsCurrentPositionObject.longitudeDescription;
                //string accuracySmall = Foundation.NSBundle.MainBundle.LocalizedString("", null) + ": " + AppDelegate.current.gpsCurrentPositionObject.accuracy + " " + Foundation.NSBundle.MainBundle.LocalizedString("Location.Element.Meters.Text", null);

                string newPositionText = latitudeText + "\n" + longitudeText;

				lastPositionText = FlashTextUtil.FlashChangedText(lastPositionText, newPositionText, labLabelBottom, FlashTextUtil.Type.Position);
            }
            else
            {
				lastPositionText = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownPosition", null);
				labLabelBottom.Text = lastPositionText;
            }
        }

		private async void UpdateAddress(UILabel labLabelBottom)
        {
			labLabelBottom.Text = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownAddress", null);

            if (AppDelegate.current.gpsCurrentPositionObject == null || AppDelegate.current.gpsCurrentPositionObject.latitudeDescription == null)
            {
                // No position
                string text = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownAddress", null);
                if (text != lastAddressText)
                {
                    lastAddressText = text;
					labLabelBottom.Text = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownAddress", null);
                }
            }
            else
            {
                // Position known. Try to find address
                if (AppDelegate.current.geocoder == null)
                    AppDelegate.current.geocoder = new CLGeocoder();

                CLLocation location = new CLLocation(AppDelegate.current.gpsCurrentPositionObject.gpsCoordinates.Latitude, AppDelegate.current.gpsCurrentPositionObject.gpsCoordinates.Longitude);
				CLPlacemark[] placemarks = await AppDelegate.current.geocoder.ReverseGeocodeLocationAsync(location);

				if (placemarks != null && placemarks.Length > 0)
                {
                    string newAddressText = "";

                    /*
                    Console.WriteLine ("GEO AdministrativeArea: " + placemarks [0].AdministrativeArea);
                    Console.WriteLine ("GEO Country: " + placemarks [0].Country);
                    Console.WriteLine ("GEO Locality: " + placemarks [0].Locality);
                    Console.WriteLine ("GEO Name: " + placemarks [0].Name);
                    Console.WriteLine ("GEO PostalCode: " + placemarks [0].PostalCode);
                    Console.WriteLine ("GEO SubAdministrativeArea: " + placemarks [0].SubAdministrativeArea);
                    Console.WriteLine ("GEO SubLocality: " + placemarks [0].SubLocality);
                    Console.WriteLine ("GEO SubThoroughfare: " + placemarks [0].SubThoroughfare);
                    Console.WriteLine ("GEO Thoroughfare: " + placemarks [0].Thoroughfare);
                    Console.WriteLine ("GEO Zone: " + placemarks [0].Zone);
                    //Console.WriteLine ("Test: " + placemarks [0].ToString ());
                    */

                    NSObject formattedAddressLines = placemarks[0].AddressDictionary["FormattedAddressLines"];
                    if (formattedAddressLines != null)
                    {
                        NSArray addressArrayLines = formattedAddressLines as NSArray;

                        if (addressArrayLines.Count == 0)
                        {
							newAddressText = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownPosition", null);
                        }
                        else
                        {
							if (placemarks[0].Name != null)
							{
								newAddressText = newAddressText + placemarks[0].Name;
                            }

							if (placemarks[0].PostalCode != null)
							{
								if(newAddressText.Length > 0)
									newAddressText = newAddressText + "\n" + placemarks[0].PostalCode;
								else
									newAddressText = newAddressText + placemarks[0].PostalCode;
							}    

							if (placemarks[0].Locality != null)
                            {
                                if(newAddressText.Length > 0)
									newAddressText = newAddressText + " " + placemarks[0].Locality;
                                else
									newAddressText = newAddressText + placemarks[0].Locality;
                            }
                        }
                    }
                    else
                    {
						newAddressText = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownPosition", null);
                    }

					if (newAddressText != lastAddressText)
                    {
						lastAddressText = FlashTextUtil.FlashChangedText(lastAddressText, newAddressText, labLabelBottom, FlashTextUtil.Type.Address);
                    }
					else
					{
						labLabelBottom.Text = lastAddressText;
					}
                }
                else
                {
					labLabelBottom.Text = Foundation.NSBundle.MainBundle.LocalizedString("GPS.UnknownAddress", null);
                }
            }
        }
    }
}
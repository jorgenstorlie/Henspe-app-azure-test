using System;
using CoreGraphics;
using Foundation;
using Henspe.Core.Model.Dto;
using Henspe.iOS.Const;
using SNLA.iOS.Util;
using SNLA.Core.Util;
using UIKit;
using Xamarin.Essentials;

namespace Henspe.iOS
{
    public partial class MainViewController : UIViewController
    {
        private bool showHelpUs = false;
        private MainListTableViewSource mainListTableViewSource = null;

        // Events
        NSObject observerActivatedOccured;

        private UIStringAttributes normalText = new UIStringAttributes
        {
            ForegroundColor = ColorHelper.FromType(ColorType.Label),
            Font = FontConst.fontMedium
        };
        private UIImageView logoImageView;

        public MainViewController(IntPtr handle) : base(handle)
        {
            AppDelegate.current.mainViewController = this;
            AppDelegate.current.locationManager = new LocationManager();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupView();

            // Events
            observerActivatedOccured = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.appActivated), HandleActivatedOccured);

            AppDelegate.current.locationManager.StartGPSTracking();
        }

        public void HandleActivatedOccured(NSNotification notification)
        {
            if (AppDelegate.current.locationManager.gpsCurrentPositionObject != null && AppDelegate.current.locationManager.locationManager.Location != null)
                AppDelegate.current.locationManager.gpsCurrentPositionObject.gpsCoordinates = AppDelegate.current.locationManager.locationManager.Location.Coordinate;

            RefreshPositionAndAddressRows();
        }

        private void SetupNavigationBar()
        {
            // Transparent background
            UIImage emptyImage = new UIImage();
            NavigationController.NavigationBar.Translucent = true;
            NavigationController.NavigationBar.SetBackgroundImage(emptyImage, UIBarMetrics.Default);
            NavigationController.NavigationBar.ShadowImage = emptyImage;

            UINavigationBar navigationBar = NavigationController.NavigationBar;
            logoImageView = new UIImageView(UIImage.FromBundle("logo"));

       //     logoImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            double imageHeight = navigationBar.Bounds.Height * 0.8;
            double computedImageWidth = (imageHeight * logoImageView.Image.CGImage.Width) / logoImageView.Image.CGImage.Height;
            logoImageView.Frame = new CGRect((navigationBar.Bounds.Width / 2) - (computedImageWidth / 2), (navigationBar.Bounds.Height / 2) - (imageHeight / 2), computedImageWidth, imageHeight);
            navigationBar.AddSubview(logoImageView);

            NavigationItem.RightBarButtonItem.TintColor =  ColorHelper.FromType(ColorType.NavigationbarTint);
        }

        private void OnSettingsClicked()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

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
            SetupNavigationBar();
     //       btnConsent.SetTitleColor(ColorConst.snlaBlue, UIControlState.Normal);
            btnConsent.Layer.BorderColor = btnConsent.TitleColor(UIControlState.Normal).CGColor;
            btnConsent.Layer.BorderWidth = 1;
            btnConsent.Layer.CornerRadius = 5.0f;

            View.BackgroundColor = ColorHelper.FromType(ColorType.SystemBackground);

            if (!showHelpUs)
            {
                btnConsent.Hidden = true;
                constraintAreaOverTable.Constant = 0;
            }

            myTableView.ReloadData();

            RefreshPositionRow();

        }

        public override void ViewDidAppear(bool animated)
        {
            //base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            RemoveNavigationBarLogo();
            base.ViewWillDisappear(animated);
        }

        private void RemoveNavigationBarLogo()         {             logoImageView.RemoveFromSuperview();         }

        private void SetupView()
        {
            // Table setup
            mainListTableViewSource = new MainListTableViewSource(this);
            mainListTableViewSource.sectionsWithRows = AppDelegate.current.structure;

            myTableView.Source = mainListTableViewSource;
            myTableView.BackgroundColor = UIColor.Clear;
            myTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            myTableView.RowHeight = UITableView.AutomaticDimension;
            myTableView.EstimatedRowHeight = 50;

            AutomaticallyAdjustsScrollViewInsets = false;

            UIEdgeInsets insets = new UIEdgeInsets(0, 0, 100, 0);
            myTableView.ContentInset = insets;
        }

        private void RefreshPositionRow()
        {
            RefreshTableRow(1, MainListTableViewSource.locationRow);
        }

        private void RefreshAddressRow()
        {
            RefreshTableRow(1, MainListTableViewSource.addressRow);
        }

        public void RefreshPositionAndAddressRows()
        {
            NSIndexPath[] indexPathList =
            {
                NSIndexPath.FromRowSection(MainListTableViewSource.locationRow, 1),
                NSIndexPath.FromRowSection(MainListTableViewSource.addressRow, 1),
            };

            myTableView.ReloadRows(indexPathList, UITableViewRowAnimation.Fade);
        }

        private void RefreshTableRow(int section, int row)
        {
            NSIndexPath[] indexPathList = new NSIndexPath[] { NSIndexPath.FromRowSection(row, section) };
            myTableView.ReloadRows(indexPathList, UITableViewRowAnimation.Fade);
        }

        partial void OnConsentClicked(NSObject sender)
        {
            InvokeOnMainThread(delegate
            {
                this.PerformSegue("segueConsent", this);
            });
        }

        partial void OnSettingsClicked(NSObject sender)
        {
            InvokeOnMainThread(delegate
            {
                this.PerformSegue("segueSettings", this);
            });
        }

        public void RowSelected(NSIndexPath indexPath, bool selected)
        {
            if (indexPath.Section == MainListTableViewSource.exactPostitionSection && indexPath.Row == 0)
            {

                if (AppDelegate.current.locationManager.locationManager.Location != null)
                {
                    var loc = AppDelegate.current.locationManager.gpsCurrentPositionObject;

                    var location = new Location(AppDelegate.current.locationManager.lastKnownLocation.Coordinate.Latitude, AppDelegate.current.locationManager.lastKnownLocation.Coordinate.Longitude);

                    NavigationMode mode = NavigationMode.None;
                    string name = "HENSPE";
                    var options = new MapLaunchOptions
                    {
                        Name = name,
                        NavigationMode = mode
                    };

                    Map.OpenAsync(location);
                }

                /*
                InvokeOnMainThread(delegate
                {
                    this.PerformSegue("segueMap", this);
                });
                */

            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueConsent")
            {
            }
            else if (segue.Identifier == "segueSettings")
            {
            }
            else if (segue.Identifier == "segueMap")
            {
            }
        }

        [Action("UnwindToMain:")]
        public void UnwindToMainViewController(UIStoryboardSegue segue)
        {
        }
    }

    // Table view source
    public partial class MainListTableViewSource : UITableViewSource
    {
        public const int locationRow = 1;
        public const int addressRow = 2;
        public const int exactPostitionSection = 1;

        private int headerHeight = 80;
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
            if (sectionsWithRows.structureSectionList != null && sectionsWithRows.structureSectionList.Count > 0)
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

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            const string normalCellIdentifier = "MainNormalCell";
            const string positionIdentifier = "MainLocationCell";
            const string addressIdentifier = "AddressCell";

            int section = indexPath.Section;
            int row = indexPath.Row;

            string cellIdenifier;

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
                cellIdenifier = null;

            if (structureElement.elementType == StructureElementDto.ElementType.Normal)
            {
                cellIdenifier = normalCellIdentifier;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Position)
            {
                cellIdenifier = positionIdentifier;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Address)
            {
                cellIdenifier = addressIdentifier;
            }
            else
            {
                cellIdenifier = null;
            }

            var cell = tableView.DequeueReusableCell(cellIdenifier);
            return cell.Bounds.Height;
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

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
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

            var cell = (HeaderViewCell)tableView.DequeueReusableCell("HeaderCell");
            cell.SetContent(structureSection.description);

            if (IsOdd((int)section + 1))
                cell.ContentView.BackgroundColor = ColorHelper.FromType(ColorType.OddRow);
            else
                cell.ContentView.BackgroundColor = ColorHelper.FromType(ColorType.EvenRow);

            return cell.ContentView;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = null;
            const string normalCellIdentifier = "MainNormalCell";
            const string positionIdentifier = "MainLocationCell";
            const string addressIdentifier = "AddressCell";

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
                MainNormalRowViewCell mainNormalRowViewCell = tableView.DequeueReusableCell(normalCellIdentifier, indexPath) as MainNormalRowViewCell;

                mainNormalRowViewCell.SetContent(structureElement);

                cell = mainNormalRowViewCell;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Position)
            {
                // Location row
                MainLocationRowViewCell locationCell = tableView.DequeueReusableCell(positionIdentifier, indexPath) as MainLocationRowViewCell;
                locationCell.SetContent();
                cell = locationCell;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Address)
            {
                var addressCell = tableView.DequeueReusableCell(addressIdentifier, indexPath) as AddressViewCell;
                addressCell.SetContent();
                cell = addressCell;
            }
            else
            {
                return null;
            }

            if (cell != null)
            {
                if (IsOdd((int)section + 1))
                    cell.BackgroundColor = ColorHelper.FromType(ColorType.OddRow);
                else
                    cell.BackgroundColor = ColorHelper.FromType(ColorType.EvenRow);
            }
            return cell;
        }

        private void UpdatePosition(UILabel labLabelBottom)
        {
            labLabelBottom.Text = LangUtil.Get("GPS.UnknownPosition");

            if (AppDelegate.current.locationManager.gpsCurrentPositionObject != null)
            {
                string latitudeText = AppDelegate.current.locationManager.gpsCurrentPositionObject.latitudeDescription;
                string longitudeText = AppDelegate.current.locationManager.gpsCurrentPositionObject.longitudeDescription;
                //string accuracySmall = LangUtil.Get("") + ": " + AppDelegate.current.gpsCurrentPositionObject.accuracy + " " + LangUtil.Get("Location.Element.Meters.Text");

                string newPositionText = latitudeText + "\n" + longitudeText;

                lastPositionText = FlashTextUtil.FlashChangedText(lastPositionText, newPositionText, labLabelBottom, FlashTextUtil.Type.Position);
            }
            else
            {
                lastPositionText = LangUtil.Get("GPS.UnknownPosition");
                labLabelBottom.Text = lastPositionText;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            MainViewController parent;
            if (_parent.TryGetTarget(out parent))
            {
                parent.RowSelected(indexPath, true);
                tableView.DeselectRow(indexPath, false);
            }
        }
    }
}
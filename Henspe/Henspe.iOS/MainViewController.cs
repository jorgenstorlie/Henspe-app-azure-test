// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Threading.Tasks;
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
        private bool showHelpUs = false;
        private MainListTableViewSource mainListTableViewSource = null;

        // Events
        NSObject observerActivatedOccured;

        private UIStringAttributes normalText = new UIStringAttributes
        {
            ForegroundColor = ColorConst.snlaText,
            Font = FontConst.fontMedium
        };

        public MainViewController(IntPtr handle) : base(handle)
        {
            AppDelegate.current.mainViewController = this;
            AppDelegate.current.locationManager = new LocationManager();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupNavigationBar();
            SetupView();

            // Events
            observerActivatedOccured = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.appActivated), HandleActivatedOccured);
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
            this.NavigationController.NavigationBar.Translucent = true;
            this.NavigationController.NavigationBar.SetBackgroundImage(emptyImage, UIBarMetrics.Default);
            this.NavigationController.NavigationBar.ShadowImage = emptyImage;

            // Logo
            UIImage imgLogo = UIImage.FromFile("ic_snla.png");
            UIImageView imgViewLogo = new UIImageView(imgLogo);
            this.NavigationItem.TitleView = imgViewLogo;

            NavigationItem.RightBarButtonItem.TintColor = ColorConst.snlaBlue;
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

            btnConsent.SetTitleColor(ColorConst.snlaBlue, UIControlState.Normal);
            btnConsent.Layer.BorderColor = btnConsent.TitleColor(UIControlState.Normal).CGColor;
            btnConsent.Layer.BorderWidth = 1;
            btnConsent.Layer.CornerRadius = 5.0f;

            View.BackgroundColor = ColorConst.snlaBackground;

            if (!showHelpUs)
            {
                btnConsent.Hidden = true;
                constraintAreaOverTable.Constant = 0;
            }

            myTableView.ReloadData();
        }

        public override void ViewDidAppear(bool animated)
        {
            //base.ViewDidAppear(animated);

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

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
            RefreshTableRow(1, MainListTableViewSource.LocationRow);
        }

        private void RefreshAddressRow()
        {
            RefreshTableRow(1, MainListTableViewSource.AddressRow);
        }

        public void RefreshPositionAndAddressRows()
        {
            NSIndexPath[] indexPathList = 
            {
                NSIndexPath.FromRowSection(MainListTableViewSource.LocationRow, 1),
                NSIndexPath.FromRowSection(MainListTableViewSource.AddressRow, 1),
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

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueConsent")
            {
            }
            else if (segue.Identifier == "segueSettings")
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
        public const int LocationRow = 1;
        public const int AddressRow = 2;
        public int selectedSegment = 0;

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

            var cell = (HeaderViewCell)tableView.DequeueReusableCell("HeaderCell");
            cell.SetContent(structureSection.description);
            cell.ContentView.BackgroundColor = ColorConst.headerBackgroundColor;
            return cell.ContentView;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
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

                return mainNormalRowViewCell;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Position)
            {
                // Location row
                MainLocationRowViewCell locationCell = tableView.DequeueReusableCell(positionIdentifier, indexPath) as MainLocationRowViewCell;
                locationCell.SetContent();
                return locationCell;
            }
            else if (structureElement.elementType == StructureElementDto.ElementType.Address)
            {
                var addressCell = tableView.DequeueReusableCell(addressIdentifier, indexPath) as AddressViewCell;
                addressCell.SetContent();
                return addressCell;
            }
            else
            {
                return null;
            }
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

        void SegmentedCell_SegmentSelected(object sender, int e)
        {
            selectedSegment = e;
            if (!_parent.TryGetTarget(out MainViewController parent))
                return;
            parent.RefreshPositionAndAddressRows();
        }
    }
}
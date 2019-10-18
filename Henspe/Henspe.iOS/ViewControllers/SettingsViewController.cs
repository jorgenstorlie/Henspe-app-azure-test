using System;
using Foundation;
using UIKit;
using Henspe.iOS.Const;
using CoreGraphics;
using Henspe.Core.Services;
using SNLA.Core.Util;
using SNLA.iOS.Util;

namespace Henspe.iOS
{
    public enum LocationServiceAccess
    {
        notSet,
        notAllowed,
        onlyWhenInUse,
        always
    }

    public partial class SettingsViewController : UIViewController
    {
        private UITextField activeField;
        private Core.Util.InfoUrl infoUrl = Core.Util.InfoUrl.Undefined;

        private bool keyboardHideDone = true;
        private UITapGestureRecognizer tapGesture;
        private UIEdgeInsets originalContentInsets;
        private CGRect originalFrame;

        // Events
        private NSObject observerActivated = null;
        private NSObject observerExtraInfoLinkClicked = null;
        private NSObject observerCoordinateFormatLinkClicked = null;

        //NSObject activatedOccuredObserver = null;
        private SettingsViewSource settingsViewSource = null;
        private int indexExtraInfo = 0;

        public SettingsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Setup();
        }

        private void Setup()
        {
            SetupNavigationBar();
            SetupTable();
            observerActivated = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.activatedEvent), HandleOnActivatedOccured);
        }

        private void HandleOnActivatedOccured(NSNotification obj)
        {
            if (myTableView != null)
                myTableView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(false, false);

            SetupEvents();
            SetupTouchGesture();
        }

        private void SetupEvents()
        {
            observerExtraInfoLinkClicked = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.settingsChooseExtraInfo), OnExtraInfoLinkClicked);
            observerCoordinateFormatLinkClicked = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.settingsChooseCoordinateFormat), OnCoordinateFormatLinkClicked);
        }

        public override void ViewWillDisappear(bool animated)
        {
            if (observerExtraInfoLinkClicked != null)
                observerExtraInfoLinkClicked.Dispose();

            if (observerCoordinateFormatLinkClicked != null)
                observerCoordinateFormatLinkClicked.Dispose();

            if (observerActivated != null)
                observerActivated.Dispose();

            if (tapGesture != null)
                View.RemoveGestureRecognizer(tapGesture);

            base.ViewWillDisappear(animated);
        }

        private void SetupTouchGesture()
        {
            tapGesture = new UITapGestureRecognizer(OnViewContentTouched);
            tapGesture.NumberOfTapsRequired = 1;
            tapGesture.CancelsTouchesInView = false;
            View.AddGestureRecognizer(tapGesture);
        }

        private void OnViewContentTouched()
        {
            if (activeField == null)
            {
                return;
            }
            else
            {
                activeField.ResignFirstResponder();
                activeField = null;
            }
        }

        #region events
        private void OnExtraInfoLinkClicked(NSNotification obj)
        {
            //OpenModalPickerExtraInfo();
        }

        private void OnCoordinateFormatLinkClicked(NSNotification obj)
        {
            //OpenModalPickerCoordainateFormat(User.credentials.format);
        }
        #endregion

        #region navigation bar
        private void SetupNavigationBar()
        {
            Title = LangUtil.Get("Settings.heading");

        NavigationController.NavigationBar.Translucent = true;
            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = ColorHelper.FromType(ColorType.NavigationbarLabel),
            };

            NavigationController.NavigationBar.TintColor = ColorHelper.FromType(ColorType.NavigationbarTint);
            View.BackgroundColor = ColorHelper.FromType(ColorType.SystemBackground);

            //    NavigationItem.BackBarButtonItem.TintColor = ColorHelper.FromType(ColorType.Image);
            //    NavigationItem. TintColor = ColorHelper.FromType(ColorType.Image);

            // Back button
            /*
            string backText = LangUtil.Get("InfoViewController.ButtonBack.Text");
            string backAccessibilityLabel = backText;
            string backAccessibilityHint = LangUtil.Get("InfoViewController.ButtonBack.Accessibility.Hint");
            ButtonUtil.SetBackButton(btnBack, backText, backAccessibilityLabel, backAccessibilityHint);
            */
        }

        /*partial void OnBtnBackClicked(NSObject sender)
        {
            this.NavigationController.PopViewController(true);
        }*/
        #endregion

        #region table
        private void SetupTable()
        {
            // Table setup
            settingsViewSource = new SettingsViewSource(this);
            myTableView.Source = settingsViewSource;
            myTableView.BackgroundColor = UIColor.Clear;
            myTableView.SeparatorColor = UIColor.Clear;

            myTableView.ContentInset  = new UIEdgeInsets(0f, 0f, 0f, 0f);

        }

        public void RowSelected(NSIndexPath indexPath, bool selected)
        {
            if (indexPath.Section == settingsViewSource.concentSection && indexPath.Row == 0)
            {
                this.PerformSegue("segueConsent", this);
            }
            else if (indexPath.Section == settingsViewSource.coordinatsSection && indexPath.Row == 0)
            {
                this.PerformSegue("segueFormat", this);
            }
            else if ((indexPath.Section == settingsViewSource.locationSection))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
            }
            else if (indexPath.Section == settingsViewSource.informationSection && indexPath.Row == 0)
            {
                infoUrl = Core.Util.InfoUrl.PrivacyPolicy;
                this.PerformSegue("segueInfo", this);
            }
            else if (indexPath.Section == settingsViewSource.informationSection && indexPath.Row == 1)
            {
                infoUrl = Core.Util.InfoUrl.Terms;
                this.PerformSegue("segueInfo", this);
            }
        }
        #endregion

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
        }

        public override void ViewDidAppear(bool animated)
        {
            //to handle editing of coordinats
            TableUtil.RefreshRowInTable(myTableView, settingsViewSource.coordinatsSection, 0);
            base.ViewDidAppear(animated);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueInfo")
            {
                InfoViewController infoViewController = (InfoViewController)segue.DestinationViewController;
                infoViewController.infoUrl = infoUrl;
            }
        }
    }

    // Table view source
    public class SettingsViewSource : UITableViewSource
    {
        private const int numberOfSections = 3;
        public int concentSection = 0;
        public int coordinatsSection = 1;
        public int locationSection = 2;
        public int informationSection = 3;

        private WeakReference<SettingsViewController> _parent;
        private int headerHeight = 40;

        public SettingsViewSource(SettingsViewController controller)
        {
            _parent = new WeakReference<SettingsViewController>(controller);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return numberOfSections;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (section == informationSection)
                return 2;
            else
                return 1;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (!GetSectionVisible(section))
                return 0;
            else
                return headerHeight;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            CGRect headerframe = new CGRect(0, 0, tableView.Bounds.Size.Width, headerHeight);
            UIView headerView = new UIView(headerframe);
            headerView.BackgroundColor = ColorHelper.FromType(ColorType.SystemGroupedBackground);

            int labelHeight = 20;

            CGRect labelframe = new CGRect(15, headerHeight - labelHeight - 2, tableView.Bounds.Size.Width - 10, labelHeight);
            UILabel label = new UILabel(labelframe);
            label.Font = FontConst.fontHeadingTable;
            label.TextColor = ColorHelper.FromType(ColorType.Label);

            if (section == coordinatsSection)
                label.Text = LangUtil.Get("SettingsViewController.Coordinates.Header").ToUpper();
            else if (section == locationSection)
                label.Text = LangUtil.Get("SettingsViewController.Location.Header").ToUpper();
            else if (section == informationSection)
                label.Text = LangUtil.Get("SettingsViewController.Information.Header").ToUpper();
            else if (section == concentSection)
                label.Text = LangUtil.Get("SettingsViewController.Concent.Header").ToUpper();
            else
                label.Text = "";

            headerView.AddSubview(label);
            return headerView;
        }

        private bool GetSectionVisible(nint section)
        {
            if (section == 0)
                return false;
            else
                return true;
        }

        private bool GetCellVisible(NSIndexPath indexPath)
        {
            if (indexPath.Section == 0)
                return false;
            else
                return true;
        }

        private string GetCellName(NSIndexPath indexPath)
        {
            string cellIdentifier = "";

            if (indexPath.Section == coordinatsSection)
                cellIdentifier = "cellIdCoordinates";
            else if (indexPath.Section == locationSection)
                cellIdentifier = "cellIdLocation";
            else if (indexPath.Section == informationSection)
                cellIdentifier = "cellIdInfo";
            else if (indexPath.Section == concentSection)
                cellIdentifier = "cellIdTopInfo";
            return cellIdentifier;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            string cellIdentifier;
            cellIdentifier = GetCellName(indexPath);
            if (!GetCellVisible(indexPath))
                return 0;

            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
            if (cell != null)
                return cell.Bounds.Height;
            else
                return 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = null;

            string cellIdentifier;
            cellIdentifier = GetCellName(indexPath);

            if (indexPath.Section == coordinatsSection)
            {
                SettingsCoordinatesTableCell settingsCoordinatesTableCell = tableView.DequeueReusableCell(cellIdentifier) as SettingsCoordinatesTableCell;

                settingsCoordinatesTableCell.BackgroundColor = ColorHelper.FromType(ColorType.SecondarySystemGroupedBackground);
                settingsCoordinatesTableCell.LabInfo.TextColor = ColorHelper.FromType(ColorType.Label);
                settingsCoordinatesTableCell.LabInfo.Text = LangUtil.Get("SettingsViewController.Coordinates.Info");
                settingsCoordinatesTableCell.LabInfo.Font = FontConst.fontSmall;

                string coordinateButtonText = "";
                CoordinateFormat coordinateFormat = UserUtil.Current.format;

                if (coordinateFormat == CoordinateFormat.DD)
                    coordinateButtonText = LangUtil.Get("SettingsViewController.Coordinates.DD");
                else if (coordinateFormat == CoordinateFormat.DDM)
                    coordinateButtonText = LangUtil.Get("SettingsViewController.Coordinates.DDM");
                else if (coordinateFormat == CoordinateFormat.DMS)
                    coordinateButtonText = LangUtil.Get("SettingsViewController.Coordinates.DMS");
                else if (coordinateFormat == CoordinateFormat.UTM)
                    coordinateButtonText = LangUtil.Get("SettingsViewController.Coordinates.UTM");
                else
                    coordinateButtonText = LangUtil.Get("SettingsViewController.Coordinates.DDM");

                settingsCoordinatesTableCell.LabValue.TextColor = ColorHelper.FromType(ColorType.Label);
                settingsCoordinatesTableCell.LabValue.Text = coordinateButtonText;
                settingsCoordinatesTableCell.LabValue.Font = FontConst.fontMediumRegular;


                settingsCoordinatesTableCell.BackgroundColor = ColorHelper.FromType(ColorType.SecondarySystemGroupedBackground);

                cell = settingsCoordinatesTableCell;
            }
            else if (indexPath.Section == locationSection)
            {
                // Location
                LocationServiceAccess locationServiceAccess = AppDelegate.current.locationManager.GetLocationServiceAccess();
                SettingsLocationTableCell settingsLocationTableCell = tableView.DequeueReusableCell(cellIdentifier) as SettingsLocationTableCell;

                settingsLocationTableCell.LabInfo.Text = LangUtil.Get("SettingsViewController.Location.Info");

                settingsLocationTableCell.LabLeft.Text = LangUtil.Get("SettingsViewController.Location.Text");

                if (locationServiceAccess == LocationServiceAccess.always)
                    settingsLocationTableCell.LabRight.Text = LangUtil.Get("SettingsViewController.Location.Always");
                else if (locationServiceAccess == LocationServiceAccess.onlyWhenInUse)
                    settingsLocationTableCell.LabRight.Text = LangUtil.Get("SettingsViewController.Location.OnlyWhenInUse");
                else if (locationServiceAccess == LocationServiceAccess.notAllowed)
                    settingsLocationTableCell.LabRight.Text = LangUtil.Get("SettingsViewController.Location.NotAllowed");
                else
                    settingsLocationTableCell.LabRight.Text = LangUtil.Get("SettingsViewController.Location.Unset");

                //  if (locationServiceAccess == LocationServiceAccess.always)
                if (locationServiceAccess == LocationServiceAccess.onlyWhenInUse)
                {
                    settingsLocationTableCell.LabInfo.TextColor = ColorHelper.FromType(ColorType.Label);
                    settingsLocationTableCell.LabInfo.Font = FontConst.fontSmall;

                    settingsLocationTableCell.LabLeft.TextColor = ColorHelper.FromType(ColorType.Label);
                    settingsLocationTableCell.LabLeft.Font = FontConst.fontMediumRegular;

                    settingsLocationTableCell.LabRight.TextColor = ColorHelper.FromType(ColorType.Label);
                    settingsLocationTableCell.LabRight.Font = FontConst.fontMediumRegular;

                    settingsLocationTableCell.BackgroundColor = ColorHelper.FromType(ColorType.SecondarySystemGroupedBackground);
                }
                else
                {
                    settingsLocationTableCell.LabInfo.TextColor = ColorHelper.FromType(ColorType.LightText);
                    settingsLocationTableCell.LabInfo.Font = FontConst.fontSmall;

                    settingsLocationTableCell.LabLeft.TextColor = ColorHelper.FromType(ColorType.LightText);
                    settingsLocationTableCell.LabLeft.Font = FontConst.fontMediumRegular;

                    settingsLocationTableCell.LabRight.TextColor = ColorHelper.FromType(ColorType.LightText);
                    settingsLocationTableCell.LabRight.Font = FontConst.fontMediumRegular;

                    settingsLocationTableCell.BackgroundColor = ColorHelper.FromType(ColorType.RedBackground);
                }
                cell = settingsLocationTableCell;
            }
            else if (indexPath.Section == concentSection)
            {
                SettingsTopInfoTableCell settingsTopInfoTableCell = tableView.DequeueReusableCell(cellIdentifier) as SettingsTopInfoTableCell;
                settingsTopInfoTableCell.BackgroundColor = UIColor.Clear;
                settingsTopInfoTableCell.LabInfo.TextColor = ColorHelper.FromType(ColorType.Label);
                settingsTopInfoTableCell.LabInfo.Font = FontConst.fontLarge;

                if (indexPath.Section == concentSection)
                {
                    //    settingsInfoTableCell.AccessibilityLabel = LangUtil.Get("SettingsViewController.Concent.Accessibility.Label");
                    //    settingsInfoTableCell.AccessibilityHint = LangUtil.Get("SettingsViewController.Concent.Accessibility.Hint");

                    if (UserUtil.Current.consentAgreed == ConsentAgreed.True)
                    {
                        settingsTopInfoTableCell.BackgroundColor = ColorHelper.FromType(ColorType.SecondarySystemGroupedBackground);
                        settingsTopInfoTableCell.LabInfo.TextColor = ColorHelper.FromType(ColorType.Label);
                    }
                    else
                    {
                        settingsTopInfoTableCell.BackgroundColor = ColorHelper.FromType(ColorType.RedBackground);
                        settingsTopInfoTableCell.LabInfo.TextColor = ColorHelper.FromType(ColorType.LightText);
                    }
                }

                string text = "";

                if (indexPath.Section == informationSection)
                {
                    if (indexPath.Row == 0)
                        text = LangUtil.Get("SettingsViewController.Information.Text");
                    else if (indexPath.Row == 1)
                        text = LangUtil.Get("SettingsViewController.Information.Text2");
                }
                else if (indexPath.Section == concentSection)
                {
                    text = LangUtil.Get("SettingsViewController.Concent.Text");
                }

                settingsTopInfoTableCell.LabInfo.Text = text;
                cell = settingsTopInfoTableCell;
            }
            else if (indexPath.Section == informationSection)
            {
                SettingsInfoTableCell settingsInfoTableCell = tableView.DequeueReusableCell(cellIdentifier) as SettingsInfoTableCell;
                settingsInfoTableCell.BackgroundColor = ColorHelper.FromType(ColorType.SecondarySystemGroupedBackground);
                settingsInfoTableCell.LabLabel.TextColor = ColorHelper.FromType(ColorType.Label);
                settingsInfoTableCell.LabLabel.Font = FontConst.fontLarge;

                if (indexPath.Section == concentSection)
                {
                    //    settingsInfoTableCell.AccessibilityLabel = LangUtil.Get("SettingsViewController.Concent.Accessibility.Label");
                    //    settingsInfoTableCell.AccessibilityHint = LangUtil.Get("SettingsViewController.Concent.Accessibility.Hint");

                    if (UserUtil.Current.consentAgreed == ConsentAgreed.True)
                    {
                        settingsInfoTableCell.BackgroundColor = UIColor.Clear;
                        settingsInfoTableCell.LabLabel.TextColor = ColorHelper.FromType(ColorType.Label);
                    }
                    else
                    {
                        settingsInfoTableCell.BackgroundColor = ColorHelper.FromType(ColorType.RedBackground);
                        settingsInfoTableCell.LabLabel.TextColor = ColorHelper.FromType(ColorType.LightText);
                    }
                }

                string text = "";

                if (indexPath.Section == informationSection)
                {
                    if (indexPath.Row == 0)
                        text = LangUtil.Get("SettingsViewController.Information.Text");
                    else if (indexPath.Row == 1)
                        text = LangUtil.Get("SettingsViewController.Information.Text2");
                }
                else if (indexPath.Section == concentSection)
                {
                    text = LangUtil.Get("SettingsViewController.Concent.Text");
                }

                settingsInfoTableCell.LabLabel.Text = text;
                cell = settingsInfoTableCell;
            }

            int lineThickness = 1;
            CGRect bottomLineframe = new CGRect(0, cell.Frame.Height - lineThickness, tableView.Bounds.Size.Width, lineThickness);
            UIView bottomLine = new UIView(bottomLineframe);
            bottomLine.BackgroundColor = ColorHelper.FromType(ColorType.Separator);

            CGRect topLineframe = new CGRect(0, 0, tableView.Bounds.Size.Width, lineThickness);
            UIView topLine = new UIView(topLineframe);
            topLine.BackgroundColor = ColorHelper.FromType(ColorType.Separator);

            cell.AddSubview(bottomLine);
            cell.AddSubview(topLine);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            SettingsViewController parent;
            if (_parent.TryGetTarget(out parent))
            {
                parent.RowSelected(indexPath, true);
                tableView.DeselectRow(indexPath, false);
            }
        }
    }
}
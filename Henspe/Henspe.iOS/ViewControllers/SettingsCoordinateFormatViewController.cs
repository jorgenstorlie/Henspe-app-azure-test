﻿using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using CoreGraphics;
using Henspe.Core.Services;
using System.Linq;
using Henspe.iOS.Const;
using SNLA.Core.Util;
using SNLA.iOS.Util;

namespace Henspe.iOS
{
    public partial class SettingsCoordinateFormatViewController : UIViewController
    {
        private SettingsCoordinateFormatTableViewSource settingsCoordinateFormatTableViewSource = null;

        public SettingsCoordinateFormatViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = LangUtil.Get("SettingsViewController.Coordinates.Format.Title");

            View.BackgroundColor = ColorHelper.FromType(ColorType.SystemGroupedBackground);

            table.BackgroundColor = UIColor.Clear;

            table.RowHeight = UITableView.AutomaticDimension;
            table.EstimatedRowHeight = 70;
            var footer = new UIView(CGRect.FromLTRB(0, 0, View.Frame.Width, 1));
            footer.BackgroundColor = ColorHelper.FromType(ColorType.Separator);
            table.TableFooterView = footer;

            labHeader.TextColor = ColorHelper.FromType(ColorType.Label);
            labHeader.Font = FontConst.fontMediumRegular;
            labHeader.Text = LangUtil.Get("SettingsViewController.Coordinates.Format.Header");

            settingsCoordinateFormatTableViewSource = new SettingsCoordinateFormatTableViewSource(this);
            table.Source = settingsCoordinateFormatTableViewSource;

            SetupData();
        }

        private void SetupData()
        {
            double lat = 53.2314d;
            double lon = 10.9283d;

            if (AppDelegate.current.locationManager.gpsCurrentPositionObject != null)
            {
                if (AppDelegate.current.locationManager.gpsCurrentPositionObject.gpsCoordinates.Latitude != 0)
                {
                    lat = AppDelegate.current.locationManager.gpsCurrentPositionObject.gpsCoordinates.Latitude;
                    lon = AppDelegate.current.locationManager.gpsCurrentPositionObject.gpsCoordinates.Longitude;
                }
            }

            string latitudeText = AppDelegate.current.locationManager.gpsCurrentPositionObject.latitudeDescription;
            string longitudeText = AppDelegate.current.locationManager.gpsCurrentPositionObject.longitudeDescription;

            var dd = AppDelegate.current.coordinateService.FormatDD(lat, lon);
            var ddm = AppDelegate.current.coordinateService.FormatDDM(lat, lon);
            var dms = AppDelegate.current.coordinateService.FormatDMS(lat, lon);
            var utm = AppDelegate.current.coordinateService.FormatUTM(lat, lon);

            settingsCoordinateFormatTableViewSource.coordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.DD,
                Title = LangUtil.Get("SettingsViewController.Coordinates.DD"),
                Sub1 = dd.latitudeDescription,
                Sub2 = dd.longitudeDescription
            });

            settingsCoordinateFormatTableViewSource.coordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.DDM,
                Title = LangUtil.Get("SettingsViewController.Coordinates.DDM"),
                Sub1 = ddm.latitudeDescription,
                Sub2 = ddm.longitudeDescription
            });

            settingsCoordinateFormatTableViewSource.coordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.DMS,
                Title = LangUtil.Get("SettingsViewController.Coordinates.DMS"),
                Sub1 = dms.latitudeDescription,
                Sub2 = dms.longitudeDescription
            });

            settingsCoordinateFormatTableViewSource.coordinateRows.Add(new CoordianteRow
            {
                CoordinateFormat = CoordinateFormat.UTM,
                Title = LangUtil.Get("SettingsViewController.Coordinates.UTM"),
                Sub1 = utm.latitudeDescription,
                Sub2 = utm.longitudeDescription
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.NavigationController.SetNavigationBarHidden(false, false);

            int selectedCoordinatesRow = SelectedCoordinatesRow();

            table.SelectRow(NSIndexPath.FromRowSection(selectedCoordinatesRow, 0), false, UITableViewScrollPosition.None);
        }

        public int SelectedCoordinatesRow()
        {
            return settingsCoordinateFormatTableViewSource.coordinateRows.IndexOf(settingsCoordinateFormatTableViewSource.coordinateRows.First(x => x.CoordinateFormat == UserUtil.Current.format));
        }

        private void RowSelected(NSIndexPath indexPath)
        {
            CoordinateFormat format = settingsCoordinateFormatTableViewSource.coordinateRows[indexPath.Row].CoordinateFormat;
            UserUtil.Current.format = format;
        }

        // Table view source
        public partial class SettingsCoordinateFormatTableViewSource : UITableViewSource
        {
            WeakReference<SettingsCoordinateFormatViewController> _parent;
            public List<CoordianteRow> coordinateRows = new List<CoordianteRow>();

            public SettingsCoordinateFormatTableViewSource(SettingsCoordinateFormatViewController controller)
            {
                _parent = new WeakReference<SettingsCoordinateFormatViewController>(controller);
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return coordinateRows.Count;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                const string cellIdentifier = "cellPosition";

                SettingsCoordinateFormatTableCell settingsCoordinateFormatTableCell = tableView.DequeueReusableCell(cellIdentifier) as SettingsCoordinateFormatTableCell;
                return settingsCoordinateFormatTableCell.Bounds.Height;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                const string cellIdentifier = "cellPosition";

                SettingsCoordinateFormatTableCell settingsCoordinateFormatTableCell = tableView.DequeueReusableCell(cellIdentifier) as SettingsCoordinateFormatTableCell;
                var row = coordinateRows[indexPath.Row];
                settingsCoordinateFormatTableCell.SetContent(row.Title, row.Sub1, row.Sub2);

                settingsCoordinateFormatTableCell.BackgroundColor = ColorHelper.FromType(ColorType.SecondarySystemGroupedBackground);

                return settingsCoordinateFormatTableCell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                SettingsCoordinateFormatViewController parent;
                if (_parent.TryGetTarget(out parent))
                {
                    parent.RowSelected(indexPath);
                }
            }
        }
    }

    public class CoordianteRow
    {
        public CoordinateFormat CoordinateFormat { get; set; }
        public string Title { get; set; }
        public string Sub1 { get; set; }
        public string Sub2 { get; set; }
    }
}
// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Henspe.Core.ViewModel;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using MapKit;
using UIKit;

namespace Henspe.iOS
{
    public partial class EnterCoordinatesViewController : UIViewController, IMKLocalSearchCompleterDelegate//, IUISearchResultsUpdating, IUISearchBarDelegate, IUISearchControllerDelegate
	{
        private bool _isFocusing;
        private EnterCoordinatesViewModel _viewmodel;
        private UITableView _searchResultView;
        private MKLocalSearchCompleter _completer;

        public EnterCoordinatesViewController (IntPtr handle) : base (handle)
		{
            _viewmodel = new EnterCoordinatesViewModel();
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _completer = new MKLocalSearchCompleter();

            _completer.FilterType = MKSearchCompletionFilterType.AndQueries;
            _completer.Delegate = this;

            _viewmodel.Init();

            mapView.RegionChanged += MapView_RegionChanged;
            SVGUtil.LoadSVGToImageView(imgMapCenter, "ic_e_posisjon_given.svg", new System.Collections.Generic.Dictionary<string, string>());

            View.BackgroundColor = ColorConst.backgroundColor;

            lblEnterPosition.TextColor = ColorConst.textColor;
            lblDegreesNorth.TextColor = ColorConst.textColor;
            lblDegreesEast.TextColor = ColorConst.textColor;
            lblAddress.TextColor = ColorConst.textColor;
            txtDegreesNorth.TextColor = ColorConst.textColor;
            txtDegreesEast.TextColor = ColorConst.textColor;

            txtAddress.EditingChanged += TxtAddress_EditingChanged;

            UpdateCoordFields();
        }

        partial void latitudeChanged(NSObject sender)
        {
            var text = txtDegreesNorth.Text;

            if (text.Length > 2 && !text.Contains(","))
            {
                txtDegreesNorth.Text = text.Insert(2, ",");
            }

            if (text.Length >= 7)
                txtDegreesEast.BecomeFirstResponder();
        }

        partial void longitudeChanged(NSObject sender)
        {
            var text = txtDegreesEast.Text;

            if (text.Length > 2 && !text.Contains(","))
            {
                txtDegreesEast.Text = text.Insert(2, ",");
            }

            if (text.Length >= 6)
                FocusMap();

            if (text.Length >= 7)
                txtDegreesEast.ResignFirstResponder();
        }

        private void FocusMap()
        {
            _isFocusing = true;
            if (!double.TryParse(txtDegreesNorth.Text, out double latitude))
                return;

            if (!double.TryParse(txtDegreesEast.Text, out double longitude))
                return;

            var center = new CLLocationCoordinate2D(latitude, longitude);
            var span = new MKCoordinateSpan(0.01, 0.02);
            var region = new MKCoordinateRegion(center, span);
            mapView.SetRegion(region, true);

        }

        void MapView_RegionChanged(object sender, MKMapViewChangeEventArgs e)
        {
            imgMapCenter.Hidden = false;
            var location = mapView.CenterCoordinate;
            _viewmodel.Latitude = location.Latitude;
            _viewmodel.Longitude = location.Longitude;
            _viewmodel.Save();

            if (_isFocusing)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(2000);
                    _isFocusing = false;
                });
            }
            else
            {
                UpdateCoordFields();
            }
        }

        void TxtAddress_EditingChanged(object sender, EventArgs e)
        {
            if(txtAddress.Text.Length > 0)
            {
                if (_searchResultView == null)
                {
                    _searchResultView = new UITableView(new CGRect(txtAddress.Frame.Left, txtAddress.Frame.Bottom, txtAddress.Frame.Width, 220));
                    _searchResultView.RegisterClassForCellReuse(typeof(UITableViewCell), "AddressSearchCell");
                    var source = new SearchSource();
                    source.SearchSelected += Source_SearchSelected;
                    _searchResultView.Source = source;
                    _searchResultView.TableFooterView = new UIView(CGRect.Empty);
                }
                if(_searchResultView.Superview == null)
                {
                    viewContainer.AddSubview(_searchResultView);
                    var views = new NSMutableDictionary();
                    views.Add(new NSString("containerView"), txtAddress);
                    views.Add(new NSString("resultView"), _searchResultView);
                    var constraints = NSLayoutConstraint.FromVisualFormat("V:[containerView]-[resultView]", NSLayoutFormatOptions.AlignAllLeft, null, views);
                    NSLayoutConstraint.ActivateConstraints(constraints);
                }
                Search(txtAddress.Text);
                _searchResultView.ReloadData();
            }
        }

        public void Search(string forSearchString)
        {
            var region = MKCoordinateRegion.FromDistance(AppDelegate.current.currentLocation.Coordinate, 10000, 10000);

            var searchRequest = new MKLocalSearchRequest();
            searchRequest.NaturalLanguageQuery = forSearchString;

            //searchRequest.Region = region;

            //var localSearch = new MKLocalSearch(searchRequest);

            //localSearch.Start(delegate (MKLocalSearchResponse response, NSError error)
            //{
            //    if (response != null && error == null)
            //    {
            //        var items = response.MapItems.ToList();
            //        var source = (SearchSource) _searchResultView.Source;
            //        source.SetData(items);
            //        _searchResultView.ReloadData();
            //    }
            //    else
            //    {
            //        Console.WriteLine("local search error: {0}", error);
            //    }
            //});


            _completer.Region = region;
            _completer.QueryFragment = forSearchString;
        }

        [Export("completerDidUpdateResults:")]
        public void DidUpdateResults(MKLocalSearchCompleter completer)
        {
            var results = completer.Results;

            //var source = (SearchSource) _searchResultView.Source;
            //source.SetData(results);
            //_searchResultView.ReloadData();
        }

        void Source_SearchSelected(object sender, MKMapItem e)
        {
            txtAddress.Text = e.Placemark.Title;
            var coord = e?.Placemark?.Coordinate;
            if (coord.HasValue)
            {
                _viewmodel.Latitude = coord.Value.Latitude;
                _viewmodel.Longitude = coord.Value.Longitude;
                _viewmodel.Save();
                UpdateCoordFields();
            }

            _searchResultView.RemoveFromSuperview();
            txtAddress.ResignFirstResponder();
            FocusMap();
        }

        private void UpdateCoordFields()
        {
            if (_viewmodel.Latitude.HasValue)
                txtDegreesNorth.Text = _viewmodel.Latitude.Value.ToString("N4");
            if (_viewmodel.Longitude.HasValue)
                txtDegreesEast.Text = _viewmodel.Longitude.Value.ToString("N4");
        }

        private class SearchSource : UITableViewSource
        {
            private List<MKMapItem> _items;
            public event EventHandler<MKMapItem> SearchSelected;

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var item = _items[indexPath.Row];
                var cell = tableView.DequeueReusableCell("AddressSearchCell", indexPath);
                cell.TextLabel.Text = item.Placemark.Name;
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                if (_items == null)
                    return 0;
                return Math.Min(5, _items.Count);
            }

            public void SetData(List<MKMapItem> items)
            {
                _items = items;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                OnSearchSelected(_items[indexPath.Row]);
            }

            protected virtual void OnSearchSelected(MKMapItem e)
            {
                SearchSelected?.Invoke(this, e);
            }
        }
    }
}
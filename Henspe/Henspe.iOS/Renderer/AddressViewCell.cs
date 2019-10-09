using System;
using System.Linq;
using System.Threading.Tasks;
using Henspe.iOS.Const;
using SNLA.Core.Util;
using UIKit;
using Xamarin.Essentials;

namespace Henspe.iOS
{
    public partial class AddressViewCell : UITableViewCell
    {
        public AddressViewCell(IntPtr handle) : base(handle)
        {
        }

        public void SetContent()
        {
            btnMap.SetTitle(LangUtil.Get("Structure.EksaktPosisjon.ShareButton"), UIControlState.Normal);
            btnMap.AddTarget(ButtonEventHandler, UIControlEvent.TouchUpInside);
            btnMap.Font = FontConst.fontLarge;
            btnMap.SetTitleColor(ColorConst.snlaBlue, UIControlState.Normal);
            BackgroundColor = UIColor.Clear;

            imgImageView.Image = UIImage.FromBundle("ic_adresse").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            imgImageView.TintColor = UIColor.FromName("ImageColor");

            labAddressLine1.TextColor = UIColor.FromName("FontColor");
            labAddressLine2.TextColor = UIColor.FromName("FontColor");

            labAddressLine1.Font = FontConst.fontLarge;
            labAddressLine2.Font = FontConst.fontLarge;

            UpdateAddress();
        }

        public void ButtonEventHandler(object sender, EventArgs e)
        {
            Logger.Info("", "");

            if (sender == btnMap)
            {
                if (AppDelegate.current.locationManager.locationManager.Location != null)
                {
                    var location = new Location(AppDelegate.current.locationManager.lastKnownLocation.Coordinate.Latitude, AppDelegate.current.locationManager.lastKnownLocation.Coordinate.Longitude);
                    var loc = AppDelegate.current.locationManager.gpsCurrentPositionObject;

                    string latString = loc.latitudeDescription;
                    string lonString = loc.longitudeDescription;
                    string posString = latString + "\n" + lonString;
                    string lat = location.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    string lon = location.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    string linkString = "https://www.google.com/maps/search/?api=1&query=" + lat + "," + lon;
                    //       string common_share_my_position = Application.Context.Resources.GetString(Resource.String.common_share_my_position);

                    posString = posString + "\n" + linkString;

                    //      string posString = loc.latitudeDescription + "\n" + loc.longitudeDescription;

                    var shareTextRequest = new ShareTextRequest(posString, "HENSPE");
                    Share.RequestAsync(shareTextRequest);
                }

            }
        }

        public void UpdateAddress()
        {
            if (AppDelegate.current.locationManager.gpsCurrentPositionObject == null || AppDelegate.current.locationManager.gpsCurrentPositionObject.latitudeDescription == null)
            {
                labAddressLine1.Text = LangUtil.Get("GPS.UnknownAddress");
                labAddressLine2.Text = string.Empty;
            }
            else
            {
                Task.Run(async () =>
                {
                    var coords = AppDelegate.current.locationManager.gpsCurrentPositionObject.gpsCoordinates;
                    await SetAddress(coords.Latitude, coords.Longitude);
                });
            }
        }

        private async Task SetAddress(double latitude, double longitude)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                var street = placemark.FeatureName;
                var city = placemark.PostalCode + " " + placemark.Locality;
                BeginInvokeOnMainThread(delegate
                {
                    labAddressLine1.Text = street;
                    labAddressLine2.Text = city;
                });
            }
        }
    }
}

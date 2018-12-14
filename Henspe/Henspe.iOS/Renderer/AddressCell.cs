﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using System.Threading.Tasks;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using UIKit;
using Xamarin.Essentials;

namespace Henspe.iOS
{
    public partial class AddressCell : UITableViewCell
    {
        public AddressCell(IntPtr handle) : base(handle)
        {
        }

        public void SetContent()
        {
            BackgroundColor = UIColor.Clear;

            UIColor textColor;
            textColor = UIColor.FromRGB(0, 45, 115);
            SVGUtil.LoadSVGToImageView(imgImageView, "ic_e_adresse_my.svg", new System.Collections.Generic.Dictionary<string, string>());

            labAddressLine1.TextColor = textColor;
            labAddressLine2.TextColor = textColor;
            labAddressLine3.TextColor = ColorConst.snlaText;

            labAddressLine1.Font = FontConst.fontLarge;
            labAddressLine2.Font = FontConst.fontLarge;

            UpdateAddress();
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

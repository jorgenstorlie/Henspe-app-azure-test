using System;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using UIKit;
using System.Collections.Generic;
using SNLA.Core.Util;

namespace Henspe.iOS
{
    public partial class MainLocationRowViewCell : UITableViewCell
    {
        public MainLocationRowViewCell(IntPtr handle) : base(handle)
        {

        }

        public static string GetRGBFill(UIColor color)
        {
            nfloat r;
            nfloat g;
            nfloat b;
            nfloat a;

            color.GetRGBA(out r, out g, out b, out a);

            var red = (int)(r * 255);
            var green = (int)(g * 255);
            var blue = (int)(b * 255);
            var rgbFill = $"fill: rgb({red},{green},{blue});";

            return "fill=\"#ff0000\"";

            return rgbFill;
        }

        internal void SetContent()
        {
            this.BackgroundColor = UIColor.Clear;

            var dict = new Dictionary<string, string>();
            //   dict.Add("fill=\"#4A4A4A\"", GetRGBFill(UIColor.Red));
            //  dict.Add("stroke=\"#4A4A4A\"", "stroke=\"#00ff00\"");
            SVGUtil.LoadSVGToImageView(imgImage, "ic_posisjon.svg", dict);

            if (AppDelegate.current.locationManager.locationManager.Location != null)
            {
                var formattedCoordinatesDto = AppDelegate.current.coordinateService.GetFormattedCoordinateDescription(UserUtil.Current.format, AppDelegate.current.locationManager.locationManager.Location.Coordinate.Latitude , AppDelegate.current.locationManager.locationManager.Location.Coordinate.Longitude);

                if (formattedCoordinatesDto.success)
                {
                    labLabelTop.Text = formattedCoordinatesDto.latitudeDescription;
                    labLabelBottom.Text = formattedCoordinatesDto.longitudeDescription;
                }
            }
            else
            {
                labLabelTop.Text = string.Empty;
                labLabelBottom.Text = string.Empty;
            }

            labLabelTop.TextColor = ColorConst.snlaBlue;
            labLabelBottom.TextColor = ColorConst.snlaBlue;

            labLabelTop.Font = FontConst.fontLarge;
            labLabelBottom.Font = FontConst.fontLarge;
        }
    }
}
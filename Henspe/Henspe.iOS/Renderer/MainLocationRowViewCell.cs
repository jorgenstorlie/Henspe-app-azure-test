using System;
using Henspe.iOS.Const;
using UIKit;
using SNLA.Core.Util;

namespace Henspe.iOS
{
    public partial class MainLocationRowViewCell : UITableViewCell
    {
        public MainLocationRowViewCell(IntPtr handle) : base(handle)
        {

        }

        internal void SetContent()
        {
            this.BackgroundColor = UIColor.Clear;
       
            imgImage.Image = UIImage.FromBundle("ic_posisjon").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            imgImage.TintColor = UIColor.FromName("ImageColor");


            if (AppDelegate.current.locationManager.locationManager.Location != null)
            {
                var formattedCoordinatesDto = AppDelegate.current.coordinateService.GetFormattedCoordinateDescription(UserUtil.Current.format, AppDelegate.current.locationManager.locationManager.Location.Coordinate.Latitude, AppDelegate.current.locationManager.locationManager.Location.Coordinate.Longitude);

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
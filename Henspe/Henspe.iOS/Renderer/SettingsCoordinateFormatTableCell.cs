using System;
using Foundation;
using Henspe.iOS.Const;
using SNLA.iOS.Util;
using UIKit;

namespace Henspe.iOS
{
	public partial class SettingsCoordinateFormatTableCell : UITableViewCell
	{
		public SettingsCoordinateFormatTableCell (IntPtr handle) : base (handle)
		{
		}

        public void SetContent(string header, string latitude, string longitude)
        {
            BackgroundColor = UIColor.Red;

            labHeader.TextColor = ColorHelper.FromType(ColorType.Label);
            labHeader.Font = FontConst.fontHeading;
            labHeader.Text = header;

            labLatitude.TextColor = ColorHelper.FromType(ColorType.Label);
            labLatitude.Font = FontConst.fontMedium;
            labLatitude.Text = latitude;

            labLongitude.TextColor = ColorHelper.FromType(ColorType.Label);
            labLongitude.Font = FontConst.fontMedium;
            labLongitude.Text = longitude;
        }
    }
}
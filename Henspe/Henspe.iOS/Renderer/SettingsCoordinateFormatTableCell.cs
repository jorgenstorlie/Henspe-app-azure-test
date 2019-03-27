using System;
using Foundation;
using Henspe.iOS.Const;
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
            BackgroundColor = UIColor.Clear;

            labHeader.TextColor = ColorConst.snlaText;
            labHeader.Font = FontConst.fontHeading;
            labHeader.Text = header;

            labLatitude.TextColor = ColorConst.snlaText;
            labLatitude.Font = FontConst.fontMedium;
            labLatitude.Text = latitude;

            labLongitude.TextColor = ColorConst.snlaText;
            labLongitude.Font = FontConst.fontMedium;
            labLongitude.Text = longitude;
        }
    }
}
using System;
using UIKit;

namespace Henspe.iOS
{
	public partial class SettingsCoordinatesTableCell : UITableViewCell
	{
        public UIKit.UILabel LabInfo { get { return labInfo; } set { labInfo = value; } }
        public UIKit.UILabel LabValue { get { return labValue; } set { labValue = value; } }

        public SettingsCoordinatesTableCell (IntPtr handle) : base (handle)
		{
		}
	}
}

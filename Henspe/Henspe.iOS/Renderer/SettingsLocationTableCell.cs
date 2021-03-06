using System;
using Foundation;
using UIKit;

namespace Henspe.iOS
{
	public partial class SettingsLocationTableCell : UITableViewCell
	{
        public UIKit.UILabel LabInfo { get { return labInfo; } set { labInfo = value; } }
        public UIKit.UILabel LabLeft { get { return labLeft; } set { labLeft = value; } }
        public UIKit.UILabel LabRight { get { return labRight; } set { labRight = value; } }

        public SettingsLocationTableCell (IntPtr handle) : base (handle)
		{
		}
	}
}

using System;
using UIKit;

namespace Henspe.iOS
{
	public partial class SettingsTopInfoTableCell : UITableViewCell
	{
        public UIKit.UILabel LabInfo { get { return labInfo; } set { labInfo = value; } }

        public SettingsTopInfoTableCell (IntPtr handle) : base (handle)
		{
		}
	}
}

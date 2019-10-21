using System;
using UIKit;

namespace Henspe.iOS
{
	public partial class SettingsInfoTableCell : UITableViewCell
	{
        public UIKit.UILabel LabLabel { get { return labLabel; } set { labLabel = value; } }

        public SettingsInfoTableCell (IntPtr handle) : base (handle)
		{
		}
	}
}

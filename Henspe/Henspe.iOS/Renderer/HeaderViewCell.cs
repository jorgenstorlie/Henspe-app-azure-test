using System;
using System.Linq;
using Henspe.iOS.Const;
using UIKit;

namespace Henspe.iOS
{
    public partial class HeaderViewCell : UITableViewCell
    {
        public HeaderViewCell(IntPtr handle) : base(handle)
        {
        }

        public void SetContent(string title)
        {
            var header = title.First().ToString().ToUpper();
            labHeadline.Text = header;
            labDescription.Text = title;
            labHeadline.TextColor = ColorConst.largeTextColor;
            labDescription.TextColor = ColorConst.headerDescriptionTextColor;

            labHeadline.Font = FontConst.fontHeadingTitle;
            labDescription.Font = FontConst.fontHeadingDescription;
        }
    }
}
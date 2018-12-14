using System;
using System.Collections.Generic;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Henspe.iOS.Util
{
	public class TableUtil
	{
        static public void RefreshRowInTable(UITableView tableView, int section, int row)
        {
            NSIndexPath[] rowsToReload = new NSIndexPath[] 
            {
                NSIndexPath.FromRowSection(row, section)
            };

            tableView.ReloadRows(rowsToReload, UITableViewRowAnimation.None);
        }

        static public void ResizeHeightWithText(UILabel label, float maxHeight = 960f)
        {
            if (label != null && label.Text != null)
            {
                label.Lines = 0;
                label.LineBreakMode = UILineBreakMode.WordWrap;

                nfloat width = label.Frame.Width;
                CGSize size = ((NSString)label.Text).StringSize(label.Font, constrainedToSize: new SizeF((float)width, maxHeight), lineBreakMode: UILineBreakMode.WordWrap);
                var labelFrame = label.Frame;

                labelFrame.Size = new SizeF((float)width, (float)size.Height);
                label.Frame = labelFrame;
            }
        }

        static public nfloat ComputeHeightForLabelWith0LinesAndWordWrap(UILabel label, nfloat width)
        {
            NSString nsText = new NSString(label.Text);
            UIFont font = label.Font;
            CGSize constraintSize = new CGSize(width, float.MaxValue);
            CGSize labelSize = nsText.StringSize(font, constraintSize, UILineBreakMode.WordWrap);
            nfloat height = labelSize.Height;

            return height;
        }
    }
}
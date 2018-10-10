// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using Foundation;
using Henspe.iOS.Const;
using UIKit;

namespace Henspe.iOS
{
	public partial class SegmentedCell : UITableViewCell
	{
        public event EventHandler<int> SegmentSelected;

		public SegmentedCell (IntPtr handle) : base (handle)
		{
		}

        internal void SetContent(List<string> segments, int selectedSegment)
        {
            BackgroundColor = UIColor.Clear;
            segmentedController.TintColor = ColorConst.largeTextColor;

            segmentedController.RemoveAllSegments();
            for (int i = 0; i < segments.Count; i++)
            {
                segmentedController.InsertSegment(segments[i], i, false);
            }
            segmentedController.SelectedSegment = selectedSegment;
        }

        partial void SegmentedValueChanged(NSObject sender)
        {
            OnSegmentSelected((int)segmentedController.SelectedSegment);
        }

        protected virtual void OnSegmentSelected(int e)
        {
            SegmentSelected?.Invoke(this, e);
        }
    }
}
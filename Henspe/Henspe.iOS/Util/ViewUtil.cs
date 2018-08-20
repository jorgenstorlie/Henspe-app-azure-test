using System;
using UIKit;

namespace Henspe.iOS.Util
{
	public class ViewUtil
	{
        public ViewUtil () 
		{
		}

        public static void EnableSubviewsOfScrollview(UIScrollView scrollView, bool enabled) 
		{
            foreach (UIView subView in scrollView.Subviews)
            {
                subView.UserInteractionEnabled = enabled;
            }
		}
	}
}
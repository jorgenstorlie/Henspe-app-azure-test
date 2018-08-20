using System;
using UIKit;

namespace Henspe.iOS.Util
{
	public class CloneUtil
	{
		public CloneUtil () {
		}

		static public UILabel CloneLabel(UILabel uiLabel) 
		{
			UILabel uiClonedLabel = new UILabel();
			uiClonedLabel.Frame = uiLabel.Frame;
			uiClonedLabel.Font = uiLabel.Font;
			uiClonedLabel.AdjustsFontSizeToFitWidth = uiLabel.AdjustsFontSizeToFitWidth;
			uiClonedLabel.MinimumFontSize = uiLabel.MinimumFontSize;
			uiClonedLabel.TextAlignment = uiLabel.TextAlignment;
			uiClonedLabel.TextColor = uiLabel.TextColor;
			uiClonedLabel.MinimumFontSize = uiLabel.MinimumFontSize;
			uiClonedLabel.MinimumScaleFactor = uiLabel.MinimumScaleFactor;
			uiClonedLabel.Opaque = uiLabel.Opaque;

			return uiClonedLabel;
		}
	}
}
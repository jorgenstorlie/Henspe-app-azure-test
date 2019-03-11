using System;
using Foundation;
using UIKit;

namespace Henspe.iOS.Const
{
	public class ColorConst
	{
        public static UIColor snlaRed = UIColor.FromRGB(192, 2, 28); // Red c0021c
        public static UIColor snlaBlue = UIColor.FromRGB(24, 14, 66); // 180e42
        public static UIColor snlaText = UIColor.FromRGB(27, 27, 27);
        public static UIColor snlaTextLight = UIColor.FromRGB(255, 255, 255); // White
        public static UIColor snlaTextLightDisabled = UIColor.FromRGBA(255, 255, 255, 120); // White transparent
        public static UIColor largeTextColor = snlaRed;
        public static UIColor grayIndicator = UIColor.FromRGB(206, 206, 206);
        public static UIColor headerDescriptionTextColor = UIColor.FromRGB(27, 27, 27);

        public static UIColor snlaBackground = UIColor.FromRGB(248, 243, 241);
        public static UIColor headerBackgroundColor = UIColor.FromRGB(241, 236, 234);

        public static UIColor evenBackground = UIColor.FromRGB(241, 236, 234);
        public static UIColor oddBackgroundColor = UIColor.FromRGB(248, 243, 241)  ;


        public static UIColor headerTextColor = UIColor.FromRGB(107, 100, 100);
        public static UIColor tableSeparatorColor = UIColor.FromRGB(236, 236, 236);
        public static UIColor headerLineBackground = UIColor.FromRGB(220, 220, 220);

        public ColorConst ()
		{
		}
	}
}
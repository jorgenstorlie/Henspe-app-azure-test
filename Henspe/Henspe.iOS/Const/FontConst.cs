using System;
using Foundation;
using UIKit;

namespace Henspe.iOS.Const
{
	public class FontConst
	{
	
		static public UIFont fontSmall = UIFont.FromName("Montserrat-Light", 13f);
		static public UIFont fontMedium = UIFont.FromName("Montserrat-Light", 15f);
        static public UIFont fontMediumRegular = UIFont.FromName("Montserrat-Regular", 15f);
        static public UIFont fontMediumLight = UIFont.FromName("Montserrat-Light", 13f);
        static public UIFont fontLarge = UIFont.FromName("Montserrat-Light", 17f);
        static public UIFont fontHeading = UIFont.FromName("Montserrat-Light", 17f);

        static public UIFont fontHeadingDescription = UIFont.FromName("Montserrat-Regular", 17f);
        static public UIFont fontHeadingTitle = UIFont.FromName("Montserrat-Medium", 42f);

        static public UIFont fontHeadingTable = UIFont.FromName("Montserrat-Regular", 16f);

		public FontConst ()
		{
		}
	}
}
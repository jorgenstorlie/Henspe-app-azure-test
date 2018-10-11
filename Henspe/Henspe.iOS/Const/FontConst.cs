using System;
using Foundation;
using UIKit;

namespace Henspe.iOS.Const
{
	public class FontConst
	{
		/*
		 * PingFangTC-Medium
		2017-02-02 13:50:29.532 Hjelp113.iOS[6565:2482179] PingFangTC-Regular
		2017-02-02 13:50:29.532 Hjelp113.iOS[6565:2482179] PingFangTC-Light
		2017-02-02 13:50:29.532 Hjelp113.iOS[6565:2482179] PingFangTC-Ultralight
		2017-02-02 13:50:29.532 Hjelp113.iOS[6565:2482179] PingFangTC-Semibold
		2017-02-02 13:50:29.533 Hjelp113.iOS[6565:2482179] PingFangTC-Thin
		*/

		static public UIFont fontSmall = UIFont.FromName("Montserrat-Light", 13f);
		static public UIFont fontMedium = UIFont.FromName("Montserrat-Light", 15f);
        static public UIFont fontMediumRegular = UIFont.FromName("Montserrat-Regular", 15f);
        static public UIFont fontMediumLight = UIFont.FromName("Montserrat-Light", 13f);
        static public UIFont fontLarge = UIFont.FromName("Montserrat-Light", 17f);
		static public UIFont fontHeading = UIFont.FromName("Montserrat-Light", 17f);

		public FontConst ()
		{
		}
	}
}
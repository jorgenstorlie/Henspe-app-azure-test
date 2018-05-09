using System;

namespace Henspe.iOS.Util
{
	public class LocalDateUtil
	{
		public LocalDateUtil ()
		{
		}

		static public string GetLocalizedWeekdayForDate(DateTime date) {
			int dayOfWeek = (int)date.DayOfWeek;

			switch (dayOfWeek) {
			case 0:
				// Sunday is day 0 and not day 7
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.7", null);
			case 1:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.1", null);
			case 2:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.2", null);
			case 3:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.3", null);
			case 4:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.4", null);
			case 5:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.5", null);
			case 6:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Weekday.6", null);
			default:
				return "";
			}
		}

		static public string GetLocalizedMonthForDate(DateTime date) {
			int month = date.Month;

			switch (month) {
			case 1:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.1", null);
			case 2:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.2", null);
			case 3:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.3", null);
			case 4:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.4", null);
			case 5:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.5", null);
			case 6:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.6", null);
			case 7:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.7", null);
			case 8:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.8", null);
			case 9:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.9", null);
			case 10:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.10", null);
			case 11:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.11", null);
			case 12:
				return Foundation.NSBundle.MainBundle.LocalizedString ("Month.12", null);
			default:
				return "";
			}
		}
	}
}
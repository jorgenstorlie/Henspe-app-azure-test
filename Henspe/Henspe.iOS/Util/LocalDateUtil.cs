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
				return LangUtil.Get("Weekday.7");
			case 1:
				return LangUtil.Get("Weekday.1");
			case 2:
				return LangUtil.Get("Weekday.2");
			case 3:
				return LangUtil.Get("Weekday.3");
			case 4:
				return LangUtil.Get("Weekday.4");
			case 5:
				return LangUtil.Get("Weekday.5");
			case 6:
				return LangUtil.Get("Weekday.6");
			default:
				return "";
			}
		}

		static public string GetLocalizedMonthForDate(DateTime date) {
			int month = date.Month;

			switch (month) {
			case 1:
				return LangUtil.Get("Month.1");
			case 2:
				return LangUtil.Get("Month.2");
			case 3:
				return LangUtil.Get("Month.3");
			case 4:
				return LangUtil.Get("Month.4");
			case 5:
				return LangUtil.Get("Month.5");
			case 6:
				return LangUtil.Get("Month.6");
			case 7:
				return LangUtil.Get("Month.7");
			case 8:
				return LangUtil.Get("Month.8");
			case 9:
				return LangUtil.Get("Month.9");
			case 10:
				return LangUtil.Get("Month.10");
			case 11:
				return LangUtil.Get("Month.11");
			case 12:
				return LangUtil.Get("Month.12");
			default:
				return "";
			}
		}
	}
}
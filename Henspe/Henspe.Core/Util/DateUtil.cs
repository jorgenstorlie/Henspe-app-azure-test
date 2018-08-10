using System;
using System.Text.RegularExpressions;

namespace Henspe.Core.Util
{
	public class DateUtil
	{
		public DateUtil ()
		{
		}

		static public DateTime ConvertTimeStringToDate (string timeString)
		{
			DateTime dateTime = DateTime.ParseExact(timeString, "HH:mm", System.Globalization.CultureInfo.CurrentCulture);
			return dateTime;
		}

		static public DateTime AddMinutesToDate (DateTime dateTime, double minutes)
		{
			DateTime result = dateTime.AddMinutes (minutes);
			return result;
		}

		static public DateTime AddHoursToDate (DateTime dateTime, double hours)
		{
			DateTime result = dateTime.AddHours (hours);
			return result;
		}

		static public DateTime AddDaysToDate (DateTime dateTime, double days)
		{
			DateTime result = dateTime.AddDays (days);
			return result;
		}

		static public string ConvertDateTimeToTimeString (DateTime dateTime)
		{
			string hourPart = dateTime.TimeOfDay.Hours.ToString ();
			if (hourPart.Length < 2) {
				hourPart = "0" + hourPart;
			}

			string minutesPart = dateTime.TimeOfDay.Minutes.ToString ();
			if (minutesPart.Length < 2) {
				minutesPart = "0" + minutesPart;
			}

			string result = hourPart + ":" + minutesPart;

			return result;
		}

		static public string ConvertDateTimeToDateString (DateTime dateTime)
		{
			string datePart = dateTime.Day.ToString ();
			if (datePart.Length < 2) {
				datePart = "0" + datePart;
			}

			string monthPart = dateTime.Month.ToString ();
			if (monthPart.Length < 2) {
				monthPart = "0" + monthPart;
			}

			string yearPart = dateTime.Year.ToString ();

			string result = datePart + "." + monthPart + "." + yearPart;

			return result;
		}
			
		static public DateTime GetMidnightForDate(DateTime inputDate) 
		{
			DateTime resultDate = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, 0, 0, 0);
			return resultDate;
		}

		static public DateTime ConvertDateStringToDateTime(string dateTimeString, string format) 
		{
			if (dateTimeString == null || dateTimeString.Trim().Length == 0 || format == null || ConvertUtil.ConvertStringToDouble(dateTimeString) == 0) 
			{
				return DateTime.MinValue; // Null date
			}

			DateTime dateTime = DateTime.ParseExact(dateTimeString, format, System.Globalization.CultureInfo.CurrentCulture);
			return dateTime;
		}

		static public string ConvertDateTimeToDateString(DateTime date, string format) 
		{
			return date.ToString(format);
		}
	}
}
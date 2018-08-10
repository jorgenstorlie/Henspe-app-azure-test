using System;

namespace Henspe.Core.Util
{
	public class SessionUtil
	{
		public SessionUtil ()
		{
		}

		static public string MakeSectionText(string weekDayString, string dateNumberString, string monthString, string fromTimeString, string durationString, string timeNotDecidedString, string minString) {
			string resultString = timeNotDecidedString;;

			string timeFromString = fromTimeString;
			if(timeFromString != null && durationString != null && timeFromString.Length > 0) {
				double duration = Convert.ToDouble (durationString);
				DateTime timeFrom = DateUtil.ConvertTimeStringToDate(fromTimeString);
				DateTime timeTo = DateUtil.AddMinutesToDate(timeFrom, duration);
				string timeToString = DateUtil.ConvertDateTimeToTimeString(timeTo);

				resultString = weekDayString + " " + dateNumberString + "." + monthString + "    " + timeFromString + " - " + timeToString + " (" + durationString + " " + minString + ")";
			}

			return resultString;
		}

		static public string MakeSortText(DateTime sessionDate, string sessionTime, string sessionDurationMinutes, string fromTimeString, string durationString) {
			string resultString = "";

			string timeFromString = fromTimeString;
			if(timeFromString != null && durationString != null && timeFromString.Length > 0) {
				double duration = Convert.ToDouble (durationString);
				DateTime timeFrom = DateUtil.ConvertTimeStringToDate(fromTimeString);
				DateTime timeTo = DateUtil.AddMinutesToDate(timeFrom, duration);
				string timeToString = DateUtil.ConvertDateTimeToTimeString(timeTo);
				string dateFormattedString = DateUtil.ConvertDateTimeToDateString (sessionDate, "yyyy.MM.dd");

				resultString = dateFormattedString + " " + timeFromString + " - " + timeToString + " (" + durationString + ")";
			}

			return resultString;
		}
	}
}

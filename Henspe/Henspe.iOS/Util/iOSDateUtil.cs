using System;

namespace Henspe.iOS.Util
{
    public class iOSDateUtil
    {
        public iOSDateUtil()
        {
        }

        static public DateTime ConvertNsDateToDateTime(Foundation.NSDate date)
        {
            DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var utcDateTime = reference.AddSeconds(date.SecondsSinceReferenceDate);
            return utcDateTime.ToLocalTime();
        }

        static public Foundation.NSDate ConvertDateTimeToNSDate(DateTime date)
        {
            DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var utcDateTime = date.ToUniversalTime();
            return Foundation.NSDate.FromTimeIntervalSinceReferenceDate((utcDateTime - reference).TotalSeconds);
        }
    }
}

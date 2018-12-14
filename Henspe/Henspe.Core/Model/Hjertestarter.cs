using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Henspe.Core.Model;
using Henspe.Core.Util;
using SQLite;
using Nager.Date;
using System.Linq;
using Henspe.Core.Service;
using Henspe.Core.Communication;
using System.Diagnostics;

namespace Henspe.Core.Model
{
    public class Hjertestarter : BusinessEntityBase
    {
        private const int ClosingInLimit = 30;
        private bool isOpen = false;
        private HjertestarterService hjertestarterService;

        public override string Key
        {
            get => assetId.ToString();
            set => assetId = ConvertUtil.ConvertStringToDouble(value);
        }

        [JsonProperty("assId")]
        public double assetId { get; set; }
        [JsonProperty("addGuid")]
        public string assetGuid { get; set; }
        [JsonProperty("lat"), Indexed]
        public double siteLatitude { get; set; }
        [JsonProperty("lon"), Indexed]
        public double siteLongitude { get; set; }
        [JsonProperty("name")]
        public string siteName { get; set; }
        [JsonProperty("floor")]
        public int siteFloorNumber { get; set; }
        [JsonProperty("desc")]
        public string siteDescription { get; set; }
        [JsonProperty("actDateLim")]
        public string activeDateLimited { get; set; }
        [JsonProperty("actFromDate")]
        public string activeFromDate { get; set; }
        [JsonProperty("actToDate")]
        public string activeToDate { get; set; }
        [JsonProperty("openClosedHol")]
        public string openingHoursClosedHolidays { get; set; }
        [JsonProperty("openLimited")]
        public string openingHoursLimited { get; set; }
        [JsonProperty("monFrom")]
        public int openingHoursMonFrom { get; set; }
        [JsonProperty("monTo")]
        public int openingHoursMonTo { get; set; }
        [JsonProperty("tueFrom")]
        public int openingHoursTueFrom { get; set; }
        [JsonProperty("tueTo")]
        public int openingHoursTueTo { get; set; }
        [JsonProperty("wedFrom")]
        public int openingHoursWedFrom { get; set; }
        [JsonProperty("wedTo")]
        public int openingHoursWedTo { get; set; }
        [JsonProperty("thuFrom")]
        public int openingHoursThuFrom { get; set; }
        [JsonProperty("thuTo")]
        public int openingHoursThuTo { get; set; }
        [JsonProperty("friFrom")]
        public int openingHoursFriFrom { get; set; }
        [JsonProperty("friTo")]
        public int openingHoursFriTo { get; set; }
        [JsonProperty("satFrom")]
        public int openingHoursSatFrom { get; set; }
        [JsonProperty("satTo")]
        public int openingHoursSatTo { get; set; }
        [JsonProperty("sunFrom")]
        public int openingHoursSunFrom { get; set; }
        [JsonProperty("sunTo")]
        public int openingHoursSunTo { get; set; }

        public Dictionary<string, string> translations;

        public Hjertestarter()
        {
        }

        private string GetLanguageString(HjertestarterServiceLanguageKey key)
        {
            if (translations != null)
            {
                string languageString = "";

                bool result = translations.TryGetValue(key.ToString(), out languageString);
                if (result == true)
                {
                    return languageString;
                }
                else
                {
                    Debug.WriteLine("ERROR: Unknown language string for key " + key);
                    return "";
                }
            }
            else
                return key.ToString();
        }

        public bool IsOpenChanged()
        {
            return true;
            //return isOpen != IsOpen(ServerUtil.GetServerDate());
        }

        public bool IsOpen()
        {
            return true;
            //isOpen = IsOpen(ServerUtil.GetServerDate());
            //return isOpen;
        }

        public bool IsOpen(DateTime date)
        {
            bool isActive = IsActive(date);
            if (!isActive)
                return false;

            if (ClosedBecauseOfHoliday(date))
                return false;

            if (OpenAllDay())
                return true;

            int openFromCombined = GetOpenFromHour(date.DayOfWeek);
            int openToCombined = GetOpenToHour(date.DayOfWeek);

            if (openFromCombined == 0 && openToCombined == 0)
                return false;

            var openFrom = ConvertToDateTime(date, openFromCombined);
            var openTo = ConvertToDateTime(date, openToCombined);

            if (openFrom > date || openTo < date)
                return false;

            return true;
        }

        public string GetOpeningHourText()
        {
            return GetOpeningHourText(DateTime.Now);
        }

        public string GetOpeningHourText(DateTime date)
        {
            if (!IsActive(date))
                return GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_NotActive);

            var openFromHour = GetOpenFromHour(DateTime.Now.DayOfWeek);
            var openFrom = ConvertToDateTime(date, openFromHour);

            var openToHour = GetOpenToHour(DateTime.Now.DayOfWeek);
            var openTo = ConvertToDateTime(date, openToHour);

            if (IsOpen())
            {
                if (OpenAllDay())
                {
                    return GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_AlwaysOpen);
                }
                TimeSpan closingIn = openTo - date;
                // If closing in less than 30 minutes, show it's closing soon
                if (closingIn < new TimeSpan(0, ClosingInLimit, 0))
                    return string.Format(GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_ClosingSoon), closingIn.Minutes);
                else
                {
                    string openFromString = openFrom.ToString("HH");
                    string openToString = openTo.ToString("HH");
                    if (openFrom.Minute != 0)
                        openFromString = openFrom.ToString("HH:mm");
                    if (openTo.Minute != 0)
                        openToString = openTo.ToString("HH:mm");
                    return string.Format(GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_OpenAtTime), openFromString, openToString);
                }
            }
            else
            {
                // Closed today already, closed because of holiday or not open today
                if ((openTo < date && openFrom < date) || ClosedBecauseOfHoliday(date) || (openFromHour == 0 && openToHour == 0))
                {
                    return GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_Closed);
                }
                else
                {
                    // Opening Later today
                    return OpenLaterToday(date);
                }
            }
            //  throw new Exception("Wow, I didn't think of this...");
        }

        //private string FindNextOpeningHour(DateTime date, string closedOpensLaterDay, string notActive, string tomorrow)
        //{
        //    int dayAddition = 1;
        //    while (true)
        //    {
        //        var nextDate = date.AddDays(dayAddition);

        //        dayAddition = dayAddition + 1;

        //        if (!IsActive(nextDate))
        //            return notActive;

        //        if (ClosedBecauseOfHoliday(nextDate))
        //            continue;

        //        var nextFromHour = GetOpenFromHour(nextDate.DayOfWeek);
        //        var nextFrom = ConvertToDateTime(nextDate, nextFromHour);

        //        var nextToHour = GetOpenToHour(nextDate.DayOfWeek);
        //        var nextTo = ConvertToDateTime(nextDate, nextToHour);

        //        if (nextFromHour == 0 && nextToHour == 0)
        //            continue;

        //        var later = OpenLaterToday(nextDate);
        //        if (!later.HasValue)
        //            continue;

        //        string datepart = string.Empty;
        //        if (dayAddition == 2)
        //            datepart = tomorrow;
        //        else if (dayAddition < 8)
        //            datepart = later.Value.ToString("dddd");
        //        else
        //            datepart = later.Value.ToShortDateString();

        //        return string.Format(closedOpensLaterDay, datepart, later.Value.ToShortTimeString());
        //    }
        //}

        //private DateTime? OpenLaterToday(DateTime nextDate)
        //{
        //    var openHour = GetOpenFromHour(nextDate.DayOfWeek);
        //    if (openHour == 0)
        //        return null;
        //    var date = ConvertToDateTime(nextDate, openHour);
        //    return date;
        //}

        private string OpenLaterToday(DateTime nextDate)
        {
            var openHour = GetOpenFromHour(nextDate.DayOfWeek);
            var date = ConvertToDateTime(nextDate, openHour);
            var diff = date - nextDate;
            if (diff < new TimeSpan(0, 30, 0))
            {
                return string.Format(GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_ClosedOpensSoon), diff.Minutes);
            }
            return GetLanguageString(HjertestarterServiceLanguageKey.Hjertestarter_Closed);
        }

        private DateTime ConvertToDateTime(DateTime date, int time)
        {
            var temp = date;
            int hour = time / 100;
            int minute = time % 100;
            if (hour == 24)
            {
                hour = 0;
                temp = temp.AddDays(1);
            }
            var dateTime = new DateTime(temp.Year, temp.Month, temp.Day, hour, minute, 0);
            return dateTime;
        }

        private bool ClosedBecauseOfHoliday(DateTime date)
        {
            if (openingHoursClosedHolidays == "Y")
            {
                return DateSystem.IsPublicHoliday(date, CountryCode.NO);
            }
            return false;
        }

        private bool IsActive(DateTime date)
        {
            if (activeDateLimited == "Y")
            {
                var hasFromDate = DateTime.TryParse(activeFromDate, out var activeFrom);
                var hasToDate = DateTime.TryParse(activeToDate, out var activeTo);
                if ((hasFromDate && activeFrom > date) || (hasToDate && activeTo < date))
                {
                    return false;
                }
            }
            return true;
        }

        private int GetOpenFromHour(DayOfWeek day)
        {
            if (day == DayOfWeek.Monday)
                return openingHoursMonFrom;
            if (day == DayOfWeek.Tuesday)
                return openingHoursTueFrom;
            if (day == DayOfWeek.Wednesday)
                return openingHoursWedFrom;
            if (day == DayOfWeek.Thursday)
                return openingHoursThuFrom;
            if (day == DayOfWeek.Friday)
                return openingHoursFriFrom;
            if (day == DayOfWeek.Saturday)
                return openingHoursSatFrom;
            if (day == DayOfWeek.Sunday)
                return openingHoursSunFrom;
            throw new Exception("That's not a day!");
        }

        private int GetOpenToHour(DayOfWeek day)
        {
            if (day == DayOfWeek.Monday)
                return openingHoursMonTo;
            if (day == DayOfWeek.Tuesday)
                return openingHoursTueTo;
            if (day == DayOfWeek.Wednesday)
                return openingHoursWedTo;
            if (day == DayOfWeek.Thursday)
                return openingHoursThuTo;
            if (day == DayOfWeek.Friday)
                return openingHoursFriTo;
            if (day == DayOfWeek.Saturday)
                return openingHoursSatTo;
            if (day == DayOfWeek.Sunday)
                return openingHoursSunTo;
            throw new Exception("That's not a day!");
        }

        private bool OpenAllDay()
        {
            return openingHoursLimited == "N";
        }
    }
}
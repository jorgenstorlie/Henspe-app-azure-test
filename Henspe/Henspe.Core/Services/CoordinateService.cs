﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Henspe.Core.Communication;

namespace Henspe.Core.Services
{
    public enum CoordinateFormat
    {
        Undefined = 0,
        DD = 1,
        DDM = 2,
        DMS = 3,
        UTM = 4
    }

    public enum CoordinateServiceLanguageKey
    {
        Coordinate_North,
        Coordinate_South,
        Coordinate_East,
        Coordinate_West,
        Coordinate_Degrees,
        Coordinate_Minutes,
        Coordinate_Seconds
    }

	public class FormattedCoordinatesDto
	{
		public bool success { get; set; }
		public string error { get; set; }
		public string latitudeDescription { get; set; }
		public string longitudeDescription { get; set; }
	}

	public class CoordinateService
    {
        public Dictionary<string, string> translations = new Dictionary<string, string>();

        public CoordinateService()
        {
        }

        public void AddLanguageValue(CoordinateServiceLanguageKey key, string value)
        {
            translations.Add(key.ToString(), value);
        }

        private string GetLanguageString(CoordinateServiceLanguageKey key)
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

        public FormattedCoordinatesDto GetFormattedCoordinateDescription(CoordinateFormat format, double latitude, double longitude)
        {
            FormattedCoordinatesDto formattedCoordinatesDto;

            if (format == CoordinateFormat.DD)
            {
                formattedCoordinatesDto = FormatDD(latitude, longitude);
            }
            else if (format == CoordinateFormat.DDM)
            {
                formattedCoordinatesDto = FormatDDM(latitude, longitude);
            }
            else if (format == CoordinateFormat.DMS)
            {
                formattedCoordinatesDto = FormatDMS(latitude, longitude);
            }
            else if (format == CoordinateFormat.UTM)
            {
                formattedCoordinatesDto = FormatUTM(latitude, longitude);
            }
            else
            {
                formattedCoordinatesDto = new FormattedCoordinatesDto();
                formattedCoordinatesDto.success = false;
                formattedCoordinatesDto.error = "Unknown format";
            }

            return formattedCoordinatesDto;
        }

        public FormattedCoordinatesDto FormatDD(double latitude, double longitude)
        {
            // DD - Desimalgrader. Eksempel 33.268602° -45.543819°
            FormattedCoordinatesDto formattedCoordinatesDto = new FormattedCoordinatesDto();

            formattedCoordinatesDto.success = true;

            string latitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_North);

            if (latitude < 0)
            {
                latitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_South);
                latitude = Math.Abs(latitude);
            }

            string longitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_East);

            if (longitude < 0)
            {
                longitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_West);
                longitude = Math.Abs(longitude);
            }

            string format = "00.0000";
            string latitudeDescription = latitude.ToString(format) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Degrees) + " " + latitudeDirection;
            string longitudeDescription = longitude.ToString(format) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Degrees) + " " + longitudeDirection; ;

            formattedCoordinatesDto.success = true;
            formattedCoordinatesDto.latitudeDescription = latitudeDescription;
            formattedCoordinatesDto.longitudeDescription = longitudeDescription;

            return formattedCoordinatesDto;
        }

        public FormattedCoordinatesDto FormatDDM(double latitude, double longitude)
        {
            // DDM - Desimalgrader og minutter. Eksempel: 33° 16.116'N  45° 32.629'W
            FormattedCoordinatesDto formattedCoordinatesDto = new FormattedCoordinatesDto();

            formattedCoordinatesDto.success = true;

            string latitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_North);

            if (latitude < 0)
            {
                latitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_South);
                latitude = Math.Abs(latitude);
            }

            string longitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_East);

            if (longitude < 0)
            {
                longitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_West);
                longitude = Math.Abs(longitude);
            }
            string latitudeDescription = GetDegreesValue(latitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Degrees) + " " + GetDecimalMinutes(latitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Minutes) + " " + latitudeDirection;
            string longitudeDescription = GetDegreesValue(longitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Degrees) + " " + GetDecimalMinutes(longitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Minutes) + " " + longitudeDirection;

            formattedCoordinatesDto.success = true;
            formattedCoordinatesDto.latitudeDescription = latitudeDescription;
            formattedCoordinatesDto.longitudeDescription = longitudeDescription;

            return formattedCoordinatesDto;
        }

        public FormattedCoordinatesDto FormatDMS(double latitude, double longitude)
        {
            // DMS - Desimalgrader, minutter og sekunder. Eksempel: 33° 16' 6.97"N  45° 32' 37.75"W
            FormattedCoordinatesDto formattedCoordinatesDto = new FormattedCoordinatesDto();

            formattedCoordinatesDto.success = true;

            string latitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_North);

            if (latitude < 0)
            {
                latitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_South);
                latitude = Math.Abs(latitude);
            }

            string longitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_East);

            if (longitude < 0)
            {
                longitudeDirection = GetLanguageString(CoordinateServiceLanguageKey.Coordinate_West);
                longitude = Math.Abs(longitude);
            }
            string latitudeDescription = GetDegreesValue(latitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Degrees) + " " + GetMinutesValue(latitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Minutes) + " " + GetDecimalSeconds(latitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Seconds) + " " + latitudeDirection;
            string longitudeDescription = GetDegreesValue(longitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Degrees) + " " + GetMinutesValue(longitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Minutes) + " " + GetDecimalSeconds(longitude) + " " + GetLanguageString(CoordinateServiceLanguageKey.Coordinate_Seconds) + " " + longitudeDirection;

            formattedCoordinatesDto.success = true;
            formattedCoordinatesDto.latitudeDescription = latitudeDescription;
            formattedCoordinatesDto.longitudeDescription = longitudeDescription;

            return formattedCoordinatesDto;
        }

        public FormattedCoordinatesDto FormatUTM(double latitude, double longitude)
        {
            // UTM - Universal Transverse Mercator. Eksempel: 17N 630084 4833438
            FormattedCoordinatesDto formattedCoordinatesDto = new FormattedCoordinatesDto();

            formattedCoordinatesDto.success = true;

            if (latitude < 0)
            {
                latitude = Math.Abs(latitude);
            }

            if (longitude < 0)
            {
                longitude = Math.Abs(longitude);
            }

            LatLongtoUTM(latitude, longitude, out double utmNorthing, out double utmEasting, out string zone);

            string latitudeDescription = zone + " " + utmNorthing.ToString("000000");
            string longitudeDescription = utmEasting.ToString("0000000");

            formattedCoordinatesDto.success = true;
            formattedCoordinatesDto.latitudeDescription = latitudeDescription;
            formattedCoordinatesDto.longitudeDescription = longitudeDescription;

            return formattedCoordinatesDto;
        }

        private string GetDegreesValue(double inputValue)
        {
            int mainValue = (int)inputValue;
            double mainValueDouble = (double)mainValue;

            string mainValueFormattedString = mainValueDouble.ToString("00");

            return mainValueFormattedString;
        }

        private string GetDecimalMinutes(double inputValue)
        {
            int mainValue = (int)inputValue;
            double mainValueDouble = (double)mainValue;

            double decimalValue = (inputValue - mainValueDouble) * 60;

            string decimalFormattedString = decimalValue.ToString("00.00");

            return decimalFormattedString;
        }

        private string GetMinutesValue(double inputValue)
        {
            int mainValue = (int)inputValue;
            double mainValueDouble = (double)mainValue;

            double decimalValue = (inputValue - mainValueDouble) * 60;
            int minuteValue = (int)decimalValue;

            string minuteValueFormattedString = minuteValue.ToString("00");

            return minuteValueFormattedString;
        }

        private string GetDecimalSeconds(double inputValue)
        {
            int mainValue = (int)inputValue;
            double mainValueDouble = (double)mainValue;

            double minutesValue = (inputValue - mainValueDouble) * 60;
            int minutesValueInt = (int)minutesValue;
            double minutesValueDouble = (double)minutesValueInt;

            double secondsValue = (minutesValue - minutesValueDouble) * 60;

            string result = secondsValue.ToString("00.00");

            return result;
        }

        private void LatLongtoUTM(double Lat, double Long,
          out double UTMNorthing, out double UTMEasting,
          out string Zone)
        {
            double a = 6378137; //WGS84
            double eccSquared = 0.00669438; //WGS84
            double k0 = 0.9996;

            double LongOrigin;
            double eccPrimeSquared;
            double N, T, C, A, M;

            //Make sure the longitude is between -180.00 .. 179.9
            double LongTemp = (Long + 180) - ((int)((Long + 180) / 360)) * 360 - 180; // -180.00 .. 179.9;

            double LatRad = DegreesToRadians(Lat);
            double LongRad = DegreesToRadians(LongTemp);
            double LongOriginRad;
            int ZoneNumber;

            ZoneNumber = ((int)((LongTemp + 180) / 6)) + 1;

            if (Lat >= 56.0 && Lat < 64.0 && LongTemp >= 3.0 && LongTemp < 12.0)
                ZoneNumber = 32;

            // Special zones for Svalbard
            if (Lat >= 72.0 && Lat < 84.0)
            {
                if (LongTemp >= 0.0 && LongTemp < 9.0) ZoneNumber = 31;
                else if (LongTemp >= 9.0 && LongTemp < 21.0) ZoneNumber = 33;
                else if (LongTemp >= 21.0 && LongTemp < 33.0) ZoneNumber = 35;
                else if (LongTemp >= 33.0 && LongTemp < 42.0) ZoneNumber = 37;
            }
            LongOrigin = (ZoneNumber - 1) * 6 - 180 + 3; //+3 puts origin in middle of zone
            LongOriginRad = DegreesToRadians(LongOrigin);

            //compute the UTM Zone from the latitude and longitude
            Zone = ZoneNumber.ToString() + UTMLetterDesignator(Lat);

            eccPrimeSquared = (eccSquared) / (1 - eccSquared);

            N = a / Math.Sqrt(1 - eccSquared * Math.Sin(LatRad) * Math.Sin(LatRad));
            T = Math.Tan(LatRad) * Math.Tan(LatRad);
            C = eccPrimeSquared * Math.Cos(LatRad) * Math.Cos(LatRad);
            A = Math.Cos(LatRad) * (LongRad - LongOriginRad);

            M = a * ((1 - eccSquared / 4 - 3 * eccSquared * eccSquared / 64 - 5 * eccSquared * eccSquared * eccSquared / 256) * LatRad
            - (3 * eccSquared / 8 + 3 * eccSquared * eccSquared / 32 + 45 * eccSquared * eccSquared * eccSquared / 1024) * Math.Sin(2 * LatRad)
            + (15 * eccSquared * eccSquared / 256 + 45 * eccSquared * eccSquared * eccSquared / 1024) * Math.Sin(4 * LatRad)
            - (35 * eccSquared * eccSquared * eccSquared / 3072) * Math.Sin(6 * LatRad));

            UTMEasting = (double)(k0 * N * (A + (1 - T + C) * A * A * A / 6
            + (5 - 18 * T + T * T + 72 * C - 58 * eccPrimeSquared) * A * A * A * A * A / 120)
            + 500000.0);

            UTMNorthing = (double)(k0 * (M + N * Math.Tan(LatRad) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
            + (61 - 58 * T + T * T + 600 * C - 330 * eccPrimeSquared) * A * A * A * A * A * A / 720)));
            if (Lat < 0)
                UTMNorthing += 10000000.0; //10000000 meter offset for southern hemisphere
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        static private char UTMLetterDesignator(double Lat)
        {
			//TODO return directly. No else. Only one && and ordered in relevance...?
            char LetterDesignator;

            if ((84 >= Lat) && (Lat >= 72)) LetterDesignator = 'X';
            else if ((72 > Lat) && (Lat >= 64)) LetterDesignator = 'W';
            else if ((64 > Lat) && (Lat >= 56)) LetterDesignator = 'V';
            else if ((56 > Lat) && (Lat >= 48)) LetterDesignator = 'U';
            else if ((48 > Lat) && (Lat >= 40)) LetterDesignator = 'T';
            else if ((40 > Lat) && (Lat >= 32)) LetterDesignator = 'S';
            else if ((32 > Lat) && (Lat >= 24)) LetterDesignator = 'R';
            else if ((24 > Lat) && (Lat >= 16)) LetterDesignator = 'Q';
            else if ((16 > Lat) && (Lat >= 8)) LetterDesignator = 'P';
            else if ((8 > Lat) && (Lat >= 0)) LetterDesignator = 'N';
            else if ((0 > Lat) && (Lat >= -8)) LetterDesignator = 'M';
            else if ((-8 > Lat) && (Lat >= -16)) LetterDesignator = 'L';
            else if ((-16 > Lat) && (Lat >= -24)) LetterDesignator = 'K';
            else if ((-24 > Lat) && (Lat >= -32)) LetterDesignator = 'J';
            else if ((-32 > Lat) && (Lat >= -40)) LetterDesignator = 'H';
            else if ((-40 > Lat) && (Lat >= -48)) LetterDesignator = 'G';
            else if ((-48 > Lat) && (Lat >= -56)) LetterDesignator = 'F';
            else if ((-56 > Lat) && (Lat >= -64)) LetterDesignator = 'E';
            else if ((-64 > Lat) && (Lat >= -72)) LetterDesignator = 'D';
            else if ((-72 > Lat) && (Lat >= -80)) LetterDesignator = 'C';
            else LetterDesignator = 'Z'; //Latitude is outside the UTM limits
            return LetterDesignator;
        }
    }
}

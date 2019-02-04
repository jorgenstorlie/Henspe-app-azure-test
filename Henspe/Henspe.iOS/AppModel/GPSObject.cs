using System;
using CoreLocation;
using SNLA.Core.Util;

namespace Henspe.iOS.AppModel
{
    public class GPSObject
    {
        public double _accuracy;
        public double accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                _accuracy = Math.Round(value, 0, MidpointRounding.AwayFromZero);
            }
        }

        private CLLocationCoordinate2D _gpsCoordinates { get; set; }
        public CLLocationCoordinate2D gpsCoordinates
        {
            get
            {
                return _gpsCoordinates;
            }
            set
            {
                _gpsCoordinates = value;

                //string degrees = LangUtil.Get("Location.Element.Degrees.Text");
                //string minutes = LangUtil.Get("Location.Element.Minutes.Text");
                //string seconds = LangUtil.Get("Location.Element.Seconds.Text");

                //string north = LangUtil.Get("Location.Element.North.Text");
                //string east = LangUtil.Get("Location.Element.East.Text");
                //string south = LangUtil.Get("Location.Element.South.Text");
                //string west = LangUtil.Get("Location.Element.West.Text");

                var formattedCoordinatesDto = AppDelegate.current.coordinateService.GetFormattedCoordinateDescription(UserUtil.Current.format, value.Latitude, value.Longitude);

                if (formattedCoordinatesDto.success)
                {
                    latitudeDescription = formattedCoordinatesDto.latitudeDescription;
                    longitudeDescription = formattedCoordinatesDto.longitudeDescription;
                }
            }
        }

        public DateTime storedDateTime { get; set; }

        public string latitudeDescription { get; set; }
        public string longitudeDescription { get; set; }
    }
}
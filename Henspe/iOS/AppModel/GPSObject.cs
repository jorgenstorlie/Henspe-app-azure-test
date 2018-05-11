using System;
using CoreLocation;
using Henspe.Core.Communication.Dto;
using Henspe.Core.Util;

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
				_accuracy = MathUtil.Round(value);
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

                string degrees = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.Degrees.Text", null);
                string minutes = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.Minutes.Text", null);
                string seconds = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.Seconds.Text", null);

                string north = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.North.Text", null);
                string east = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.East.Text", null);
                string south = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.South.Text", null);
                string west = Foundation.NSBundle.MainBundle.LocalizedString("Hjelp113.iOS.Element.West.Text", null);

                if (AppDelegate.current.coordinateFormat == CoordinateUtil.undefinedFormat)
                    AppDelegate.current.coordinateFormat = CoordinateUtil.ddm; // Default value

                FormattedCoordinatesDto formattedCoordinatesDto = CoordinateUtil.GetFormattedCoordinateDescription(AppDelegate.current.coordinateFormat, value.Latitude, value.Longitude, degrees, minutes, seconds, north, east, south, west);

                if(formattedCoordinatesDto.success)
                {
					latitudeDescription = formattedCoordinatesDto.latitudeDescription;
					longitudeDescription = formattedCoordinatesDto.longitudeDescription;
                }
			}
		}

		public DateTime storedDateTime { get; set; }

        public string latitudeDescription { get; set; }
        public string longitudeDescription { get; set; }

		public GPSObject ()
		{
		}
	}
}
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using MapKit;
using CoreLocation;
using System.Collections.Generic;
using Henspe.Core.Const;
using CoreGraphics;
using UIKit;

namespace Henspe.iOS.Util
{
	public class iOSMapUtil
	{
		private const double EARTH_EQUATORIAL_RADIUS = 6378137.0f;
		private const double  WGS84_CONSTANT = 0.99664719f;

        public static bool mapInZoom = false;

        public iOSMapUtil ()
		{
		}

		static public void ZoomToCoordinateAndCenter (MKMapView mapView, CLLocationCoordinate2D coordinate, double meters, bool animate)
		{
			if (!coordinate.IsValid ())
				return;
            
            if(mapInZoom == false)
            {
				mapInZoom = true;
				
				mapView.SetCenterCoordinate (coordinate, animate);

                UIView.Animate(2.0f, 0, UIViewAnimationOptions.CurveEaseIn, () =>
                {
                    // To state
                    mapView.SetRegion (MKCoordinateRegion.FromDistance (coordinate, meters, meters), animate);  
                },
                () =>
                {
                    // Completed
                    mapInZoom = false;
                });
            }
		}

        static public void Center (MKMapView mapView, CLLocationCoordinate2D coordinate, bool animate)
		{
			if (!coordinate.IsValid ())
				return;

            if(mapView != null)
    			mapView.SetCenterCoordinate (coordinate, animate);    
		}

        static public double SpanOfMetersAtDegreeLongitude(double degrees, double meters) 
		{
			double tanDegrees = Math.Tanh(Deg2rad(degrees));
			double beta =  tanDegrees * WGS84_CONSTANT;
			double lengthOfDegree = Math.Cos(Math.Atan(beta)) * EARTH_EQUATORIAL_RADIUS * Math.PI / 180.0;
			double measuresInDegreeLength = lengthOfDegree / meters;
			return 1.0 / measuresInDegreeLength;
		}
			
		static public double Distance(double lat1, double lon1, double lat2, double lon2) 
		{
			CLLocation pos1 = new CLLocation (lat1, lon1);
			CLLocation pos2 = new CLLocation (lat2, lon2);

			double distanceInMeters = pos1.DistanceFrom(pos2);
			return distanceInMeters;
		}

		static public double Deg2rad(double deg) 
		{
			return (deg * Math.PI / 180.0);
		}

        static public double Rad2deg(double rad) 
		{
			return (rad / Math.PI * 180.0);
		}

		static public double GetRangeForCenterAndCoordinates(double centerLatitude, double centerLongitude, List<GeoCoordinate> coordList)
		{
			double longestRange = 0;
			foreach (GeoCoordinate geoCoordinate in coordList)
			{
				double range = Distance (centerLatitude, centerLongitude, geoCoordinate.Latitude, geoCoordinate.Longitude);
				if (range > longestRange)
					longestRange = range;
			}

			return longestRange;
		}

		static public GeoCoordinate GetCentrePointFromListOfCoordinates(List<GeoCoordinate> coordList)
		{
			int total = coordList.Count;

			double X = 0;
			double Y = 0;
			double Z = 0;

			foreach(var i in coordList)
			{
				double lat = i.Latitude * Math.PI / 180;
				double lon = i.Longitude * Math.PI / 180;

				double x = Math.Cos(lat) * Math.Cos(lon);
				double y = Math.Cos(lat) * Math.Sin(lon);
				double z = Math.Sin(lat);

				X += x;
				Y += y;
				Z += z;
			}

			X = X / total;
			Y = Y / total;
			Z = Z / total;

			double Lon = Math.Atan2(Y, X);
			double Hyp = Math.Sqrt(X * X + Y * Y);
			double Lat = Math.Atan2(Z, Hyp);

			return new GeoCoordinate(Lat * 180 / Math.PI, Lon * 180 / Math.PI);
		}

		public struct GeoCoordinate
		{
			private readonly double latitude;
			private readonly double longitude;

			public double Latitude { get { return latitude; } }
			public double Longitude { get { return longitude; } }

			public GeoCoordinate(double latitude, double longitude)
			{
				this.latitude = latitude;
				this.longitude = longitude;
			}

			public override string ToString()
			{
				return string.Format("{0},{1}", Latitude, Longitude);
			}
		}

		static public double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
		{
			double fLat = DegreesToRadians(lat1);
			double fLng = DegreesToRadians(lon1);
			double tLat = DegreesToRadians(lat2);
			double tLng = DegreesToRadians(lon2);

			double degree = RadiansToDegrees(Math.Atan2(Math.Sin(tLng-fLng) * Math.Cos(tLat), Math.Cos(fLat) * Math.Sin(tLat) - Math.Sin(fLat) * Math.Cos(tLat) * Math.Cos(tLng-fLng)));

			if (degree >= 0) 
			{
				return degree;
			} 
			else 
			{
				return 360+degree;
			}
		}

		static public double DegreesToRadians(double degrees) 
		{
			return degrees * Math.PI / 180;
		}

		static public double RadiansToDegrees(double radians) 
		{
			return radians * 180 / Math.PI;
		}

		static public int GetCardinalDirectionForBeardingDegrees(double bearingDegrees)
		{
			bearingDegrees = bearingDegrees - 180;
			int cardinalDirection = GpsCardinalDirectionConst.unknown;

			if(bearingDegrees >= -181 && bearingDegrees < -157.5)
				cardinalDirection = GpsCardinalDirectionConst.south;
			else if(bearingDegrees >= -157.5 && bearingDegrees < -112.5)
				cardinalDirection = GpsCardinalDirectionConst.southWest;
			else if(bearingDegrees >= -112.5 && bearingDegrees < -67.5)
				cardinalDirection = GpsCardinalDirectionConst.west;
			else if(bearingDegrees >= -67.5 && bearingDegrees < -22.5)
				cardinalDirection = GpsCardinalDirectionConst.northWest;
			else if(bearingDegrees >= -22.5 && bearingDegrees < 22.5)
				cardinalDirection = GpsCardinalDirectionConst.north;
			else if(bearingDegrees >= 22.5 && bearingDegrees < 67.5)
				cardinalDirection = GpsCardinalDirectionConst.northEast;
			else if(bearingDegrees >= 67.5 && bearingDegrees < 112.5)
				cardinalDirection = GpsCardinalDirectionConst.east;
			else if(bearingDegrees >= 112.5 && bearingDegrees < 157.5)
				cardinalDirection = GpsCardinalDirectionConst.southEast;
			else if(bearingDegrees >= 157.5 && bearingDegrees <= 181)
				cardinalDirection = GpsCardinalDirectionConst.south;
			else
				cardinalDirection = GpsCardinalDirectionConst.unknown;

			return cardinalDirection;
		}

		static public int GetAccuracyType(double accuracy, double divider, int lowValue, int highValue)
		{
			if (accuracy <= divider)
				return highValue;
			else
				return lowValue;
		}

        static public void CenterToCoordinate(MKMapView mapView, CLLocationCoordinate2D coordinate, bool animate)
        {
            if (!coordinate.IsValid())
                return;

            mapView.SetCenterCoordinate(coordinate, animate);
        }
	}
}
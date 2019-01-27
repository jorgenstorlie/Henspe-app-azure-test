using System;

namespace Henspe.Core.Communication
{
	public interface ICredentials
	{
		bool isAuthenticated { get; set; }
		bool hasAutoLogin { get; set; }
		int key { get; set; }
		string mobilnr { get; set; }
		string mail { get; set; }
		string navn { get; set; }
		string username { get; set; }
		string signatur { get; set; }
		string password { get; set; }
		string pushDeviceToken { get; set; }
		int avdelingsId { get; set; }
		string avdeling { get; set; }
		int stillingsId { get; set; }
		string stilling { get; set; }
		string landkode { get; set; }
		string brukerOk { get; set; }
		int pinKode { get; set; }
		DateTime loginTime { get; set; }
		DateTime updated { get; set; }
		string igaLastOpenedArea { get; set; }
		string igaLastOpenedCountry { get; set; }
		string igaLastOpenedBackgroundColor { get; set; }
		string igaLastOpenedTextColor { get; set; }
		string igaLastTitle { get; set; }
		string igaLastImage { get; set; }
		string radarLastOpenedUrl { get; set; }
		string radarLastOpenedTitle { get; set; }
		DateTime poiUpdated { get; set; }
		int intervall { get; set; }
		double server_dato { get; set; }
		int server_kl { get; set; }
		string siste_synk_id { get; set; }
		double server_Local_Millisec_Diff { get; set; }
		string amazonbase { get; set; }
		string routebase { get; set; }
		string ippcurl { get; set; }
		string radarbase { get; set; }
		string yrUrl { get; set; }
		string fixedImg { get; set; }
		string lastSync_date { get; set; }
		string lastSync_time { get; set; }
		bool isOpenFlightHazard { get; set; }
		bool isOpenAmbulanceHeli { get; set; }
		bool isOpenRescueHeli { get; set; }
		bool isOpenAmbulancePlane { get; set; }
		bool isOpenHospital { get; set; }
		bool isOpenFuel { get; set; }
		bool isOpenAirport { get; set; }
		bool isOpenWebcam { get; set; }
		bool isOpenWeatherCam { get; set; }
		string menuItem1 { get; set; }
		string menuItem2 { get; set; }
		string menuItem3 { get; set; }
		string menuItem4 { get; set; }
		float zoom { get; set; }
		float satX { get; set; }
		float satY { get; set; }
		bool deletedDatabaseOnStartup { get; set; }
		bool trackingEnabled { get; set; }
		int trackingMetersGpsWakeup { get; set; }
		int trackingMetersSendInflight { get; set; }
		int trackingSecondsBetweenSendInflight { get; set; }
		int trackingSecondsBetweenSendParked { get; set; }
	}
}
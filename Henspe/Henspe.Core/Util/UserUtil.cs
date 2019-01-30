using Henspe.Core.Service;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Security;

namespace SNLA.Core.Util
{
	public enum ConsentAgreed
	{
		NotSet = 0,
		False = 1,
		True = 2
	}

	public static class UserUtil
	{
		/// <summary>
		/// Initializes the <see cref="T:SNLA.Core.Util.User"/> class. LoadSettings when first accessed
		/// </summary>
		static User()
		{
			LoadSettings();
		}
		/// <summary>
		/// Consetable. Interface to be used if application requires Consent preferences
		/// </summary>
		public interface IConsetable
		{
			string PhoneNumber 			{ get; set; }
			string EmailAddress 		{ get; set; }
			bool ConsentEmail 			{ get; set; }
			bool ConsentSMS				{ get; set; }
			ConsentAgreed ConsentAgreed { get; set; }
		}

		/// <summary>
		/// The current settings.
		/// </summary>
		private static Settings _currentSettings;
		public static Settings Current
		{
			get { return _currentSettings; }
			set {
				_currentSettings = value;
				SaveSettings();
			}
		}

		public static string PreferenceFile = "settings";
		//public static string SecureFile = "secure";

		/// <summary>
		/// Settings. All Settings for Application
		/// </summary>
		public class Settings : IConsetable
		{
			public string PhoneNumber					{ get; set; }
			public string EmailAddress 					{ get; set; }
			public bool ConsentEmail 					{ get; set; }
			public bool ConsentSMS 						{ get; set; }
			public CoordinateFormat CoordinateFormat	{ get; set; }
			public ConsentAgreed ConsentAgreed 			{ get; set; }
			public bool OnboardingCompleted 			{ get; set; }
			public string CameraLastSyncId 				{ get; set; }

			/*[JsonIgnore]
			public class SecureStorage
			{

			}*/
		}

		/// <summary>
		/// Loads the settings.
		/// </summary>
		private static void LoadSettings()
		{
			string userSettings = Preferences.Get(PreferenceFile, null, PreferenceFile);
			if(string.IsNullOrWhiteSpace(userSettings))
			{
				//If user settings is null, set Current to new Settings(), which will call SaveSettings
				ResetSettings();
				return;
			}
			_currentSettings = userSettings.JsonToObject<Settings>();
		}

		/// <summary>
		/// Saves the settings.
		/// </summary>
		private static void SaveSettings()
		{
			var json = Current.ToJson();
			if (!string.IsNullOrWhiteSpace(json))
			{
				Preferences.Set(PreferenceFile, json, PreferenceFile);
			}

			/*var secureJson = Current.ToJson();
			if (!string.IsNullOrWhiteSpace(json))
			{
				SecureStorage.SetAsync(SecureFile, secureJson);
			}*/
		}

		/// <summary>
		/// Resets the settings.
		/// </summary>
		public static void ResetSettings()
		{
			Current = new Settings();
		}
	}
}

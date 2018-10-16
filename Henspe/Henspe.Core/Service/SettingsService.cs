using System;
using Henspe.Core.Util;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Henspe.Core.Service
{
    public enum ResponseType
    {
        NotDecided,
        SMS,
        Email,
        None
    }

    public class Settings
    {
        public CoordinateFormat CoordinateFormat { get; set; }
        public ResponseType ResponseType { get; set; }
    }


    public class SettingsService
    {
        private const string SettingsKey = "henspe_settings";

        public Settings GetSettings()
        {
            var serializedSettings = Preferences.Get(SettingsKey, string.Empty);
            if(string.IsNullOrEmpty(serializedSettings))
            {
                return CreateDefaultSettings();
            }
            return JsonConvert.DeserializeObject<Settings>(serializedSettings);
        }

        public void SaveSettings(Settings settings)
        {
            var serializedSettings = JsonConvert.SerializeObject(settings);
            Preferences.Set(SettingsKey, serializedSettings);
        }

        private Settings CreateDefaultSettings()
        {
            return new Settings
            {
                CoordinateFormat = CoordinateFormat.DDM,
                ResponseType = ResponseType.NotDecided
            };
        }
    }
}

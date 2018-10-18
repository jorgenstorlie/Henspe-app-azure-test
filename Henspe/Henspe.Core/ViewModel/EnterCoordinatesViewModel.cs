using Henspe.Core.Service;

namespace Henspe.Core.ViewModel
{
    public class EnterCoordinatesViewModel
    {
        private readonly SettingsService _settingsService;

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public EnterCoordinatesViewModel()
        {
            _settingsService = new SettingsService();
        }

        public void Init()
        {
            var settings = _settingsService.GetSettings();
            Longitude = settings.SetLongitude;
            Latitude = settings.SetLatitude;
        }

        public void Save()
        {
            var settings = _settingsService.GetSettings();
            settings.SetLatitude = Latitude;
            settings.SetLongitude = Longitude;
            _settingsService.SaveSettings(settings);
        }
    }
}

using System;
using Henspe.Core.Service;
using Henspe.Core.Util;

namespace Henspe.Core.ViewModel
{
    public class MainViewModel
    {
        private readonly SettingsService _settingsService;

        public bool ShowHelpUs => _settingsService.GetSettings().ResponseType == ResponseType.NotDecided;

        public bool CoordiantesSet => _settingsService.GetSettings().SetLatitude.HasValue;

        public double? SetLatitude => _settingsService.GetSettings().SetLatitude;

        public double? SetLongitude => _settingsService.GetSettings().SetLongitude;

        public CoordinateFormat CoordinateFormat => _settingsService.GetSettings().CoordinateFormat;

        public MainViewModel()
        {
            _settingsService = new SettingsService();
        }
    }
}

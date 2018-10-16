using System;
using Henspe.Core.Service;

namespace Henspe.Core.ViewModel
{
    public class MainViewModel
    {
        private readonly SettingsService _settingsService;

        public bool ShowHelpUs => _settingsService.GetSettings().ResponseType == ResponseType.NotDecided;

        public MainViewModel()
        {
            _settingsService = new SettingsService();
        }
    }
}

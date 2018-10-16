using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Henspe.Core.Communication;
using Henspe.Core.Communication.Dto;
using Henspe.Core.Service;

namespace Henspe.Core.ViewModel
{
    public class RegisterEmailViewModel
    {
        public enum OS
        {
            iOS,
            Android
        }

        private readonly CallRegEmailSMS _callRegEmail;
        private readonly SettingsService _settingsService;
        private readonly Regex _regex;
        private string _os;
        private string _noContactWithServerString;

        public string Email { get; set; }

        public RegisterEmailViewModel()
        {
            Email = string.Empty;
            _callRegEmail = new CallRegEmailSMS();
            _settingsService = new SettingsService();
            _regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        public void Init(OS os, string noContactWithServerString)
        {
            _noContactWithServerString = noContactWithServerString;

            if (os == OS.iOS)
                _os = "i";
            else
                _os = "a";
        }

        public bool EnableOKButton => _regex.IsMatch(Email);

        public bool ValidateEmail(string email)
        {
            return _regex.IsMatch(email);
        }

        public async Task<bool> RegisterEmailAsync()
        {
            var result = await _callRegEmail.RegEmailSMS(_noContactWithServerString, null, Email, _os);
            if(result.success)
            {
                var settings = _settingsService.GetSettings();
                settings.ResponseType = ResponseType.Email;
                _settingsService.SaveSettings(settings);
            }
            return result.success;
        }
    }
}

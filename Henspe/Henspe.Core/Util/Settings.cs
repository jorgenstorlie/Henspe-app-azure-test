using Henspe.Core.Services;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Security;
using SNLA.Core.Services;

namespace SNLA.Core.Util
{
    /// <summary>
    /// Settings. All Settings for Application
    /// </summary>
    public class Settings : IConsetable
    {
        public string phoneNumber { get; set; }
        public string consentEmailAddress { get; set; }
        public bool consentEmail { get; set; }
        public bool consentSMS { get; set; }
        public CoordinateFormat format { get; set; }
        public ConsentAgreed consentAgreed { get; set; }
        public bool onboardingCompleted { get; set; }
        public string cameraLastSyncId { get; set; }

        [JsonIgnore] //HACK
        public string mobilnr => phoneNumber;

		public string mobilnr => string.Empty;

        public bool Upgrade()
        {
            return false;
        }
    }
}
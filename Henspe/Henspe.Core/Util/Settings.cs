using Henspe.Core.Service;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Security;
using SNLA.Core.Service;

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
	}
}
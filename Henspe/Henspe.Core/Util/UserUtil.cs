using Henspe.Core.Service;
using Xamarin.Essentials;

namespace SNLA.Core.Util
{
    public enum ConsentAgreed
    {
        NotSet = 0,
        False = 1,
        True = 2
    }

    public class UserUtil
    {
        private const string PreferenceFile = "settings";

        private const string keyPhoneNumber = "phone_number";
        private const string keyConsentEmailAddress = "consent_email_address";
        private const string keyConsentEmail = "consent_email";
        private const string keyConsentSMS = "consent_sms";
        private const string keyFormat = "format";
        private const string keyConsentAgreed = "consent_agreed";
        private const string keyOnboardingCompleted = "onboarding_completed";
        private const string keyCameraLastSyncId = "camera_last_sync_id";

        public static Settings settings = new Settings();

        public class Settings
        {
            public string phoneNumber
            {
                get { return Preferences.Get(keyPhoneNumber, null, PreferenceFile); }
                set
                {
                    onboardingCompleted = true;
                    Preferences.Set(keyPhoneNumber, value, PreferenceFile);
                }
            }

            public string consentEmailAddress
            {
                get { return Preferences.Get(keyConsentEmailAddress, null, PreferenceFile); }
                set { Preferences.Set(keyConsentEmailAddress, value, PreferenceFile); }
            }

            public bool consentEmail
            {
                get { return Preferences.Get(keyConsentEmail, false, PreferenceFile); }
                set { Preferences.Set(keyConsentEmail, value, PreferenceFile); }
            }

            public bool consentSMS
            {
                get { return Preferences.Get(keyConsentSMS, false, PreferenceFile); }
                set { Preferences.Set(keyConsentSMS, value, PreferenceFile); }
            }

            public CoordinateFormat format
            {
                get { return (CoordinateFormat)Preferences.Get(keyFormat, (int)CoordinateFormat.DDM, PreferenceFile); }
                set { Preferences.Set(keyFormat, (int)value, PreferenceFile); }
            }

            public ConsentAgreed consentAgreed
            {
                get { return (ConsentAgreed)Preferences.Get(keyConsentAgreed, 0, PreferenceFile); }
                set { Preferences.Set(keyConsentAgreed, (int)value, PreferenceFile); }
            }

            public bool onboardingCompleted
            {
                get { return Preferences.Get(keyOnboardingCompleted, false, PreferenceFile); }
                set { Preferences.Set(keyOnboardingCompleted, value, PreferenceFile); }
            }

            public string cameraLastSyncId
            {
                get { return Preferences.Get(keyCameraLastSyncId, null, PreferenceFile); }
                set { Preferences.Set(keyCameraLastSyncId, value, PreferenceFile); }
            }

            public bool isAuthenticated => !string.IsNullOrEmpty(phoneNumber);

            public void ConvertSettings(string version)
            {

            }
        }
    }
}
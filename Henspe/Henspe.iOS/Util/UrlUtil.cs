using System;
using System.Linq;
using Xamarin.Essentials;

namespace Henspe.Core.Util
{
    public enum InfoUrl
    {
        Undefined,
        Consent,
        Defibrillator,
        Permissions,
        Phone,
        PrivacyPolicy,
        Terms
    }

    public static class UrlUtil
    {
        private const string UrlBase = "https://snla-apps.no/web/apps/henspe/1.10/";

        private static readonly string[] AsNorwegian = { "no", "nb", "nn", "dk" };

        public static string GetUrl(InfoUrl urlType, string twoLetterIsoLanguageCode)
        {
            twoLetterIsoLanguageCode = AsNorwegian.Any(x => x == twoLetterIsoLanguageCode) ? "no" : "en";

            var appVersionLastDelimiterIndex = AppInfo.VersionString.LastIndexOf('.');
            var appVersion = AppInfo.VersionString.Substring(0, appVersionLastDelimiterIndex);
            var osVersion = DeviceInfo.Platform.ToString().ToLower();

            var urlPath = string.Empty;

            if (urlType == InfoUrl.Consent)
            {
                urlPath = $"consent_{twoLetterIsoLanguageCode}";
            }
            else if (urlType == InfoUrl.Defibrillator)
            {
                urlPath = $"defibrillator_{twoLetterIsoLanguageCode}";
            }
            else if (urlType == InfoUrl.Permissions)
            {
                urlPath = $"permissions_{osVersion}_{twoLetterIsoLanguageCode}";
            }
            else if (urlType == InfoUrl.Phone)
            {
                urlPath = $"phone_{twoLetterIsoLanguageCode}";
            }
            else if (urlType == InfoUrl.PrivacyPolicy)
            {
                urlPath = $"privacy_policy_{twoLetterIsoLanguageCode}";
            }
            else if (urlType == InfoUrl.Terms)
            {
                urlPath = $"terms_{twoLetterIsoLanguageCode}";
            }
            else
            {
                throw new Exception("Unrecognized URL");
            }

            var url = $"{UrlBase}{urlPath}.html";

            return System.Text.RegularExpressions.Regex.Escape(url);
        }
    }
}

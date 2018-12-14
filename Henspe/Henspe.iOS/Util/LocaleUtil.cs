using System;
using System.Linq;
using Foundation;

namespace Henspe.iOS.Util
{
    public static class LocaleUtil
    {
        public static string GetLanguage()
        {
            string language = NSLocale.CurrentLocale.LanguageCode;
            return language;
        }
    }
}

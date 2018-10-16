using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Henspe.Core.Service
{
    public class TranslationService
    {
        private const string FilenameBase = "Translations_{0}.json";

        private string _locale;

        private Dictionary<string, string> _translations = new Dictionary<string, string>();

        public async Task InitAsync(string locale)
        {
            _locale = locale;
            _translations.Clear();

            var filename = string.Format(FilenameBase, locale);

            using (var stream = await FileSystem.OpenAppPackageFileAsync(filename))
            {
                using (var reader = new StreamReader(stream))
                {
                    var deserializer = new JsonSerializer();
                    _translations = (Dictionary<string, string>)deserializer.Deserialize(reader, typeof(Dictionary<string, string>));
                }                
            }
        }

        public string Get(string key)
        {
            if (_translations.ContainsKey(key))
                return _translations[key];
            return key;
        }
    }

    public static class TranslationUtil
    {
        private static readonly TranslationService _service = new TranslationService();

        public static Task Init(string locale)
        {
            return _service.InitAsync(locale);
        }

        public static string Translate(this string toTranslate)
        {
            return _service.Get(toTranslate);
        }
    }
}

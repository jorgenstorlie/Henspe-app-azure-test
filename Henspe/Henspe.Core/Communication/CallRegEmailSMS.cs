using System.Net;
using System.Threading.Tasks;
using Henspe.Core.Communication;
using System;
using System.IO;
using System.Text;
using Henspe.Core.Const;
using System.Net.Http;
using System.Collections.Generic;
using SNLA.Core.Communication;
using Newtonsoft.Json;

namespace Henspe.Core.Communication
{
	public class RegEmailSMSResultDto
	{
		public bool success { get; set; }
		public bool resultat { get; set; }
		public bool epost_on { get; set; }
		public bool mob_on { get; set; }
		public bool epost_off { get; set; }
		public bool mob_off { get; set; }
		public string pin { get; set; }
		public string error_message { get; set; }
	}

	public class CallRegEmailSMS : HttpClientBase
    {
        public async Task<RegEmailSMSResultDto> RegEmailSMS(string noContactWithServerString, string mobil, string epost, string os)
        {
            RegEmailSMSResultDto regEmailSMSResultDto = new RegEmailSMSResultDto();
            regEmailSMSResultDto.success = false;

            string apikey = String.Join("", UrlConst.NLAMobAutApiKey);

            os = Uri.EscapeUriString(os);



            using (SNLAHttpClient client = new SNLAHttpClient())
            {
                string url = UrlConst.NLARegEmailSMSUrl + "?action=reg&apikey=" + apikey + "&os=" + os;

                if (mobil != null)
                    url = url + "&mobil=" + Uri.EscapeUriString(mobil);

                if (epost != null)
                    url = url + "&epost=" + Uri.EscapeUriString(epost);

                Task<HttpContent> contentTask = client.DoGet(url);
                HttpContent content = await contentTask;
                if (content == null)
                {
                    regEmailSMSResultDto.error_message = noContactWithServerString; // Localized no contact with server string
                    return regEmailSMSResultDto;
                }

                var stringJson = content.ReadAsStringAsync().Result;
                regEmailSMSResultDto = JsonConvert.DeserializeObject<RegEmailSMSResultDto>(stringJson);
            }

            regEmailSMSResultDto.success = regEmailSMSResultDto.resultat;

            return regEmailSMSResultDto;
        }

        public async Task<RegEmailSMSResultDto> UnRegEmailSMS(string noContactWithServerString, string mobil, string epost, string os)
        {
            RegEmailSMSResultDto regEmailSMSResultDto = new RegEmailSMSResultDto();
            regEmailSMSResultDto.success = false;

            string apikey = String.Join("", UrlConst.NLAMobAutApiKey);

            os = Uri.EscapeUriString(os);

            using (SNLAHttpClient client = new SNLAHttpClient())
            {
                string url = UrlConst.NLARegEmailSMSUrl + "?action=avreg&apikey=" + apikey + "&os=" + os;

                if (mobil != null)
                    url = url + "&mobil=" + Uri.EscapeUriString(mobil);

                if (epost != null)
                    url = url + "&epost=" + Uri.EscapeUriString(epost);

                Task<HttpContent> contentTask = client.DoGet(url);
                HttpContent content = await contentTask;
                if (content == null)
                {
                    regEmailSMSResultDto.error_message = noContactWithServerString; // Localized no contact with server string
                    return regEmailSMSResultDto;
                }

                var stringJson = content.ReadAsStringAsync().Result;
                regEmailSMSResultDto = JsonConvert.DeserializeObject<RegEmailSMSResultDto>(stringJson);
            }

            regEmailSMSResultDto.success = regEmailSMSResultDto.resultat;

            return regEmailSMSResultDto;
        }
    }
}
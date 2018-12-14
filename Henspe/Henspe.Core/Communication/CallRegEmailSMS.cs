using System.Net;
using Henspe.Core.Storage;
using System.Threading.Tasks;
using Henspe.Core.Communication.Dto;
using System;
using System.IO;
using System.Text;
using Henspe.Core.Util;
using Henspe.Core.Const;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using Henspe.Core.Communication;

namespace Henspe.Core.Communication
{
    public class CallRegEmailSMS
    {
        public CallRegEmailSMS()
        {
        }

        public async Task<RegEmailSMSResultDto> RegEmailSMS(string noContactWithServerString, string mobil, string epost, string os)
        {
            RegEmailSMSResultDto regEmailSMSResultDto = new RegEmailSMSResultDto();
            regEmailSMSResultDto.success = false;

            string apikey = String.Join("", UrlConst.NLAMobAutApiKey);

            os = Uri.EscapeUriString(os);

            using (CxHttpClient client = new CxHttpClient())
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

            using (CxHttpClient client = new CxHttpClient())
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
using System.Net;
using System.Threading.Tasks;
using Henspe.Core.Communication;
using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using SNLA.Core.Communication;
using SNLA.Core.Const;
using SNLA.Core.Util;
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
        public async Task<RegEmailSMSResultDto> RegEmailSMS(string mobil, string epost, string os)
        {
            string apikey = String.Join("", UrlConst.NLAMobAutApiKey);

            os = Uri.EscapeUriString(os);

			string url = UrlConst.NLARegEmailSMSUrl + "?action=reg&apikey=" + apikey + "&os=" + os;

			if (mobil != null)
				url = url + "&mobil=" + Uri.EscapeUriString(mobil);

			if (epost != null)
				url = url + "&epost=" + Uri.EscapeUriString(epost);

			var regEmailSMSResultDto = await client.Get<RegEmailSMSResultDto>(new Uri(url));
			if (!regEmailSMSResultDto.IsNullOrDefault())
			{
				regEmailSMSResultDto.success = regEmailSMSResultDto.resultat;
				return regEmailSMSResultDto;
			}

			//FIXME remove
			regEmailSMSResultDto.error_message = "Error";
			return regEmailSMSResultDto;
        }

        public async Task<RegEmailSMSResultDto> UnRegEmailSMS(string mobil, string epost, string os)
        {
            //RegEmailSMSResultDto regEmailSMSResultDto = new RegEmailSMSResultDto();

            string apikey = String.Join("", UrlConst.NLAMobAutApiKey);

            os = Uri.EscapeUriString(os);

			string url = UrlConst.NLARegEmailSMSUrl + "?action=avreg&apikey=" + apikey + "&os=" + os;

			if (mobil != null)
				url = url + "&mobil=" + Uri.EscapeUriString(mobil);

			if (epost != null)
				url = url + "&epost=" + Uri.EscapeUriString(epost);

			var regEmailSMSResultDto = await client.Get<RegEmailSMSResultDto>(new Uri(url));
			if (!regEmailSMSResultDto.IsNullOrDefault())
			{
				regEmailSMSResultDto.success = regEmailSMSResultDto.resultat;
				return regEmailSMSResultDto;
			}

			regEmailSMSResultDto.error_message = "Error";
            return regEmailSMSResultDto;
        }
    }
}
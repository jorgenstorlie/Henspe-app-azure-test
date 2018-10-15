using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Henspe.Core.Communication.Dto;

namespace Henspe.Core.Communication
{
    public class CallRegEmailSMS
	{
        const string NLA113Base = "https://ws.snla-it.no/api/h113/";
        const string NLARegEmailSMSUrl = NLA113Base + "h113_register.php";
        static readonly string[] NLAMobAutApiKey = { "0", "e", "a", "7", "a", "b", "2", "f", "5", "3", "8", "2", "7", "8", "0", "f", "7", "b", "c", "6", "4", "f", "e", "6", "4", "e", "c", "f", "a", "1", "c", "9" };

        public async Task<RegEmailSMSResultDto> RegEmailSMS(string noContactWithServerString, string mobil, string epost, string os)
		{
            RegEmailSMSResultDto regEmailSMSResultDto = new RegEmailSMSResultDto ();
			regEmailSMSResultDto.success = false;

            string apikey = String.Join("", NLAMobAutApiKey);

            if(mobil != null)
                mobil = Uri.EscapeUriString (mobil);

            if(epost != null)
                epost = Uri.EscapeUriString (epost);
    
            os = Uri.EscapeUriString (os);

			using (CxHttpClient client = new CxHttpClient ())
			{
                string url = NLARegEmailSMSUrl + "?action=reg&apikey=" + apikey + "&mobil=" + mobil + "&epost=" + epost + "&os=" + os;
				Task<HttpContent> contentTask = client.DoGet (url);
				HttpContent content = await contentTask;
				if(content == null)
				{
                    regEmailSMSResultDto.error_message = noContactWithServerString; // Localized no contact with server string
					return regEmailSMSResultDto;
				}

				var stringJson = content.ReadAsStringAsync().Result;
                regEmailSMSResultDto = JsonConvert.DeserializeObject<RegEmailSMSResultDto> (stringJson);
			}

            regEmailSMSResultDto.success = regEmailSMSResultDto.resultat;

            return regEmailSMSResultDto;
		}
	}
}
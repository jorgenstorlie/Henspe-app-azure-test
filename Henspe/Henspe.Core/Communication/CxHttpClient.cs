using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Henspe.Core.Communication
{
    public class CxHttpClient : HttpClient
    {
		public const string BugtrackUrl = "http://www.arerefsdal.com/bugtrack/trackbug.php?key=2p49fhiruhssdf&app=SMSApp&header=Bugtrack from app&receiver=are.refsdal@computas.com";
		public const string DebugInfoUrl = "http://www.arerefsdal.com/bugtrack/trackbug.php?key=2p49fhiruhssdf&app=SMSApp&header=App info from app&receiver=webmaster@snla-system.no, are.refsdal@computas.com";

		public const string Apikey = "1c86eb46656555c3383cfe75515b0161";

		public const string Base = "https://ws.snla-it.no/api/lat-samband/";

		public const string LoginUrl = Base + "sms_login.php?apikey=" + Apikey + "&action=login";
		public const string HalloUrl = Base + "sms_hallo.php?apikey=" + Apikey + "&action=hallo";
		public const string BasisUrl = Base + "sms_hent_basis.php?apikey=" + Apikey + "&action=basis";
		public const string TelefonlisteUrl = Base + "sms_hent_telefonliste.php?apikey=" + Apikey + "";
		public const string SendGroupUrl = Base + "sms_send_gruppe.php?apikey=" + Apikey + "";

        //private readonly ICredentials _credentials;

        public CxHttpClient()
        {
			DefaultRequestHeaders.Add ("Connection", "close");
        }

        public async Task<HttpContent> DoGet(string url)
        {
			/*
			Task<string> contentsTask = httpClient.GetStringAsync("http://xamarin.com"); // async method!

			// await! control returns to the caller and the task continues to run on another thread
			string contents = await contentsTask;
*/
            // Send a request asynchronously 
            //SetOutgoingHeaders();
			//try
			//{
				Task<HttpResponseMessage> responseTask = GetAsync (url);
				HttpResponseMessage response = await responseTask; 
				if (response.IsSuccessStatusCode)
				{
					//UpdateToken(response);
					return response.Content;
				}

				if (response.StatusCode.Equals(HttpStatusCode.PreconditionFailed))
				{
					//bool authSuccess = await ReAuthorize();
					//if (!authSuccess)
					//{
					//    throw new SecurityAccessDeniedException("Authorization failed to CX after token-failure. Return to login screen!");
					//}

					//Retry failed call
					//SetOutgoingHeaders();
					/*response = await GetAsync(url).ConfigureAwait(false);
					if (response.IsSuccessStatusCode)
					{
						//UpdateToken(response);
						return response.Content;
					}*/
				}
			//}
			//catch(HttpRequestException e)
			//{
			//}

            return null;
        }

        public async Task<HttpContent> DoGetUnsecure(string url)
        {
            // Send a request asynchronously 
			HttpResponseMessage response = await GetAsync(url);
            return response.IsSuccessStatusCode ? response.Content : null;
        }

        public async Task<bool> DoPost(string url, object postContent)
        {
            // Send a request asynchronously 
            //SetOutgoingHeaders();
            var json = JsonConvert.SerializeObject(postContent);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			HttpResponseMessage response = await PostAsync(url, content).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                UpdateToken(response);
                return true;
            }

            if (response.StatusCode.Equals(HttpStatusCode.PreconditionFailed))
            {
                //bool authSuccess = await ReAuthorize();
                //if (!authSuccess)
                //{
                //    throw new SecurityAccessDeniedException("Authorization failed to CX after token-failure. Return to login screen!");
                //}

                //Retry failed call
                //SetOutgoingHeaders();
				response = await PostAsync(url, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    UpdateToken(response);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DoDelete(string deleteUrl)
        {
            // Send a request asynchronously 
            //SetOutgoingHeaders();
			HttpResponseMessage response = await DeleteAsync(deleteUrl).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                UpdateToken(response);
                return true;
            }

            if (response.StatusCode.Equals(HttpStatusCode.PreconditionFailed))
            {
                //bool authSuccess = await ReAuthorize();
                //if (!authSuccess)
                //{
                //    throw new SecurityAccessDeniedException("Authorization failed to CX after token-failure. Return to login screen!");
                //}

                //Retry failed call
                //SetOutgoingHeaders();
				response = await DeleteAsync(deleteUrl).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    UpdateToken(response);
                    return true;
                }
            }
            return false;
        }

		/*
        private async Task<bool> ReAuthorize()
        {
            var credentials = new CredentialsDto
            {
                userName = _credentials.Username,
                password = _credentials.Password
            };

            var contentOut = new StringContent(JsonConvert.SerializeObject(credentials));
            contentOut.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage authResponse = await PostAsync(AuthUrl, contentOut);
            if (authResponse.IsSuccessStatusCode)
            {
                UpdateToken(authResponse);
                return true;
            }
            return false;
        }
        */

        private void UpdateToken(HttpResponseMessage response)
        {
			/*
            var token = response.Headers.FirstOrDefault(h => h.Key.Equals("X-Token")).Value.FirstOrDefault();
            _credentials.Token = token;
            */
        }

		/*
        private void SetOutgoingHeaders()
        {
            DefaultRequestHeaders.Remove("X-Token");
            DefaultRequestHeaders.Add("X-Token", _credentials.Token);
            DefaultRequestHeaders.Remove("X-User-Name");
            DefaultRequestHeaders.Add("X-User-Name", _credentials.Username);
        }
        */
    }
}

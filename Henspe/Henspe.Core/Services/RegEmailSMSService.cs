using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using Henspe.Core.Communication;
using System.Net.Http.Headers;
using System.Net.Http;
using Henspe.Core.Model;
using System.Linq;
using SNLA.Core.Communication;
using static SNLA.Core.Util.UserUtil;
using Xamarin.Essentials;

namespace Henspe.Core.Services
{
    public class RegEmailSMSService : HttpClientBase
    {
		private CallRegEmailSMS callRegEmailSMS;

		public RegEmailSMSService() : base()
		{
			callRegEmailSMS = new CallRegEmailSMS();
		}

		public async Task<RegEmailSMSResultDto> RegEmailSMS(string mobil, string epost, string os)
        {
            return await callRegEmailSMS.RegEmailSMS(mobil, epost, os);
        }

        public async Task<RegEmailSMSResultDto> UnRegEmailSMS(string mobil, string epost, string os)
        {
            return await callRegEmailSMS.UnRegEmailSMS(mobil, epost, os);
        }
    }
}
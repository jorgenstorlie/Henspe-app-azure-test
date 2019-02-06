using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Henspe.Core.Const;
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

namespace Henspe.Core.Communication
{
    public class RegEmailSMSService : HttpClientBase
    {
		private CallRegEmailSMS callRegEmailSMS;

		public RegEmailSMSService() : base()
		{
			callRegEmailSMS = new CallRegEmailSMS();
		}

		public async Task<RegEmailSMSResultDto> RegEmailSMS(string noContactWithServerString, string mobil, string epost, string os)
        {
            return await callRegEmailSMS.RegEmailSMS(noContactWithServerString, mobil, epost, os);
        }

        public async Task<RegEmailSMSResultDto> UnRegEmailSMS(string noContactWithServerString, string mobil, string epost, string os)
        {
            return await callRegEmailSMS.UnRegEmailSMS(noContactWithServerString, mobil, epost, os);
        }
    }
}
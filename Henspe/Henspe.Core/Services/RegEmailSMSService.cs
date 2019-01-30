using System;
using System.Net;
using Henspe.Core.Storage;
using System.Threading.Tasks;
using Henspe.Core.Communication.Dto;
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
using static SNLA.Core.Util.UserUtil;

namespace Henspe.Core.Communication
{
    public class RegEmailSMSService
    {
        private readonly CxHttpClient client;
        private readonly Repository repository;
        private readonly Settings settings;
        private readonly string version;
        private readonly string os;

        private CallRegEmailSMS callRegEmailSMS;

        public RegEmailSMSService(CxHttpClient client, Repository repository, Settings settings, string version, string os)
        {
            this.client = client;
            this.repository = repository;
            this.settings = settings;
            this.version = version;
            this.os = os;

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
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
using System.Threading;
using System.Linq;
using static SNLA.Core.Util.UserUtil;

namespace Henspe.Core.Communication
{
    public class CallHjertestarter
    {
        private readonly CxHttpClient cxHttpClient;
        private readonly Repository repository;
        private readonly Settings settings;
        private readonly string version;
        private readonly string os;

        public CallHjertestarter(CxHttpClient cxHttpClient, Repository repository, Settings settings, string version, string os)
        {
            this.cxHttpClient = cxHttpClient;
            this.repository = repository;
            this.settings = settings;
            this.version = version;
            this.os = os;
        }

        public async Task<GetHjertestartersResultDto> GetHjertestartersSinceLastSync(string lastSyncId)
        {
            GetHjertestartersResultDto getHjertestartersResultDto = new GetHjertestartersResultDto();
            getHjertestartersResultDto.success = false;

            string apikey = String.Join("", UrlConst.defibrillatorApiKey);

            using (CxHttpClient client = new CxHttpClient())
            {
                try
                {
                    string url = UrlConst.defibrillatorGet + "?action=hent&apikey=" + apikey + "&synk_id=" + lastSyncId;
                    Task<HttpContent> contentTask = client.DoGet(url);

                    HttpContent content = await client.DoGet(url);
                    if (content == null)
                    {
                        return getHjertestartersResultDto;
                    }

                    string result = await content.ReadAsStringAsync();
                    getHjertestartersResultDto = JsonConvert.DeserializeObject<GetHjertestartersResultDto>(result);
                    getHjertestartersResultDto.success = true;
                    return getHjertestartersResultDto;
                }
                catch (Exception ex)
                {
                    getHjertestartersResultDto.success = false;
                    getHjertestartersResultDto.error_message = ex.ToString();
                    return getHjertestartersResultDto;
                }
            }
        }
    }
}
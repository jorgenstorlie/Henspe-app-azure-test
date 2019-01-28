using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using SNLA.Core.Util;
using Newtonsoft.Json;
using System.Net.Http;
using Henspe.Core.Model.Dto;

namespace Henspe.Core.Communication
{
	public class CallDebugInfo
	{
		private readonly CxHttpClient client;
		private readonly float version;
		private readonly string user;

		public CallDebugInfo(CxHttpClient inputClient, float inputVersion, string inputUser)
		{
			client = inputClient;
			version = inputVersion;
			user = inputUser;
		}

		public async Task<BugtrackResultDto> SendDebugInfo(string message)
		{
			BugtrackResultDto bugtrackResultDto = new BugtrackResultDto ();
			bugtrackResultDto.result = "";
			bugtrackResultDto.error = "";

			string url = CxHttpClient.DebugInfoUrl + "&message=" + message;
			Task<HttpContent> contentTask = client.DoGet (url);
			HttpContent content = await contentTask;
			if(content == null)
			{
				bugtrackResultDto.error = ""; // No contact with server
				return bugtrackResultDto;
			}

			var stringJson = content.ReadAsStringAsync().Result;
			string errorMessage = JsonUtil.CheckForError(stringJson);
			if (errorMessage == null)
			{
				try
				{
					bugtrackResultDto = JsonConvert.DeserializeObject<BugtrackResultDto> (stringJson);
				}
				catch(Exception e)
				{
					AppCenterUtil.SendBugTrack("CallBugtrack error", e, stringJson, client, version, user);
				}
			} 
			else
			{
				bugtrackResultDto.error = errorMessage;
				return bugtrackResultDto;
			}

			return bugtrackResultDto;
		}
	}
}
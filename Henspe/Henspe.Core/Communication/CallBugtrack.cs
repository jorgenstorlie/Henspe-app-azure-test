using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using SNLA.Core.Util;
using System.Net.Http;
using Henspe.Core.Model.Dto;
using Newtonsoft.Json;
using SNLA.Core.Communication;

namespace Henspe.Core.Communication
{
	public class BugtrackResultDto 
	{
		public string result { get; set; }
		public string error { get; set; }
	}

	public class CallBugtrack : HttpClientBase
	{
		public CallBugtrack() : base()
		{}

		public async Task<BugtrackResultDto> TrackBug(string message)
		{
			BugtrackResultDto bugtrackResultDto = new BugtrackResultDto ();
			bugtrackResultDto.result = "";
			bugtrackResultDto.error = "";

			/*string url = SNLAHttpClient.BugtrackUrl + "&message=" + message;
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
			}*/

			return bugtrackResultDto;
		}
	}
}
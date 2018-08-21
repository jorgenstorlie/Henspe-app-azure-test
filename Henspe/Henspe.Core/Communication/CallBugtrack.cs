﻿using System.Net;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using Henspe.Core.Util;
using System.Net.Http;
using Henspe.Core.Model.Dto;
using Newtonsoft.Json;

namespace Henspe.Core.Communication
{
	public class CallBugtrack
	{
		private readonly CxHttpClient client;
		private readonly float version;
		private readonly string user;

		public CallBugtrack(CxHttpClient inputClient, float inputVersion, string inputUser)
		{
			client = inputClient;
			version = inputVersion;
			user = inputUser;
		}

		public async Task<BugtrackResultDto> TrackBug(string message)
		{
			BugtrackResultDto bugtrackResultDto = new BugtrackResultDto ();
			bugtrackResultDto.result = "";
			bugtrackResultDto.error = "";

			string url = CxHttpClient.BugtrackUrl + "&message=" + message;
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
					BugtrackUtil.SendBugtrack("CallBugtrack error", e, stringJson, client, version, user);
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
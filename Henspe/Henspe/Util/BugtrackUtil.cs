using System;
using System.Threading.Tasks;
using Henspe.Core.Communication;
using Henspe.Core.Model.Dto;

namespace Henspe.Core.Util
{
	public class BugtrackUtil
	{
		public async static void SendBugtrack(string errorHeading, Exception e, string jsonFile, CxHttpClient inputClient, float inputVersion, string inputUser)
		{
			try
			{
				CallBugtrack callBugtrack = new CallBugtrack (inputClient, inputVersion, inputUser);

				string message = "AppVersion: " + inputVersion + " User: " + inputUser + " " + errorHeading + ". " + e.Message + ". " + e.StackTrace + " " + e.ToString() + "Json file not parsed: " + jsonFile;
				message = System.Uri.EscapeDataString (message);
				Task<BugtrackResultDto> bugTrackerResultTask = callBugtrack.TrackBug (message);
				await bugTrackerResultTask; 
			}
			catch(Exception ignore)
			{
				// Do nothing. Silent fail.
			}
		}

		public async static void SendSettingsInfo(string settings, CxHttpClient inputClient, float inputVersion, string inputUser)
		{
			try
			{
				CallDebugInfo callDebugInfo = new CallDebugInfo (inputClient, inputVersion, inputUser);

				string message = "AppVersion: " + inputVersion + "\n\rUser: " + inputUser + "\n\rSettings:\n\r" + settings;
				message = System.Uri.EscapeDataString (message);
				Task<BugtrackResultDto> bugTrackerResultTask = callDebugInfo.SendDebugInfo (message);
				await bugTrackerResultTask; 
			}
			catch(Exception ignore)
			{
				// Do nothing. Silent fail.
			}
		}
	}
}
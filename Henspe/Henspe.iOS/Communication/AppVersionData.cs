using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Henspe.Core;
using SNLA.Core.Util;
using SNLA.Core.Const;

namespace Henspe.iOS.Communication
{
	public class AppVersionData
	{
		public AppVersionData ()
		{

		}

		public delegate void SuccessCallback(String appVersion);
		public delegate void FaultCallback(String reason);

		public void GetIphoneVersion(SuccessCallback successCallback, FaultCallback faultCallback) 
		{
			String myURI = "";

			if (AppDelegate.current.mode == ModeConst.prod)
				myURI = AppDelegate.current.prodUrlString + AppDelegate.current.plistFile; 
			else
				myURI = AppDelegate.current.testUrlTest + AppDelegate.current.plistFile; 

			WebRequest webRequest = WebRequest.Create (myURI);

			webRequest.BeginGetResponse ((IAsyncResult final_result) => 
			{
				WebRequest req = final_result.AsyncState as WebRequest;
				if(req != null) 
				{
					try 
					{
						WebResponse response = req.EndGetResponse(final_result);
						/*HttpWebResponse response = state.Request.EndGetResponse(result) as HttpWebResponse;*/

						Stream receiveStream = response.GetResponseStream();
						Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
						StreamReader readStream = new StreamReader( receiveStream, encode );
						String streamResult = readStream.ReadToEnd ();

						string bundleVersion = GetBundleVersion(streamResult);

						successCallback(bundleVersion);
					}
					catch (WebException e) 
					{
						faultCallback(e.Message);
						return;
					}
				}
			}, webRequest);
		}

		public string GetBundleVersion(string streamResult)
		{
			string versionTempString = StringUtil.FindStringBetween (streamResult, "<key>bundle-version</key>", "</string>");
			string versionString = StringUtil.FindStringAfter (versionTempString, "<string>");
			versionString = versionString.Trim ();
			return versionString;
		}
	}
}

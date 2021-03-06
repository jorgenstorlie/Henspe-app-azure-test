﻿using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using SNLA.Core.Communication;
using Henspe.Core.Util;
using Henspe.iOS.Const;
using UIKit;
using Newtonsoft.Json;
using SNLA.Core.Util;

namespace Henspe.iOS
{
    public partial class InfoViewController : UIViewController
    {
        public InfoUrl infoUrl = InfoUrl.Consent;

        public InfoViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView(infoUrl);
        }

        public string GetBaseUrl()
        {
            return NSBundle.MainBundle.BundlePath;
        }

        private bool HandleShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (!request.Url.ToString().Contains("file://"))
            {
                if (UIApplication.SharedApplication.CanOpenUrl(request.Url))
                    return UIApplication.SharedApplication.OpenUrl(request.Url);
                return true;
            }
            else
                return true;
        }

        public void SetupView(InfoUrl urlType)
        {
            actIndicator.Hidden = false;

            this.View.BackgroundColor = UIColor.Orange;

            webView.ShouldStartLoad = HandleShouldStartLoad;
            webView.Opaque = false;
            webView.BackgroundColor = UIColor.Clear;

            string style = "<style>";
            style = style + " @font-face {font-family: 'Montserrat-Regular';src: url('Montserrat-Regular.ttf'), format('truetype') ;}";
            style = style + " @font-face {font-family: 'Montserrat-Medium';src: url('Fonts/Montserrat-Medium.ttf'), format('truetype');}";
            style = style + " h1 {font-family: 'Montserrat-Medium'; font-size: 16px;font-weight: normal;}";
            style = style + " h2 {font-family: 'Montserrat-Medium'; font-size: 14px;font-weight: normal;}";
            style = style + " h3 {font-family: 'Montserrat-Regular'; font-size: 14px;}";
            style = style + " body {font-family: 'Montserrat-Regular'; font-size: 14px;margin-left: 20px;margin-right: 20px;}";
            style = style + " }</style>";

            string languageCode = LangUtil.GetLanguage();
            string url = UrlUtil.GetUrl(urlType, languageCode);

            //https://blog.xamarin.com/getting-started-with-async-await/
            CancellationTokenSource cts;

            // Create new CancellationTokenSource for this view's async call
            cts = new CancellationTokenSource();

            // Get the cancellation token to pass into the async method
            var ct = cts.Token;
            Task.Run(async () =>
            {
                try
                {
                    // Call my async method, whihc in turn calls an HttpClient async method.
                    string html = await GetTextAsync(url, ct);
                    html = html.Replace("<head>", "<head> " + style);
                    string title = GetTitle(html);

                    BeginInvokeOnMainThread(delegate 
                    {
                        Title = title;
                        string hh = GetBaseUrl();
                        webView.LoadHtmlString(html, new NSUrl(GetBaseUrl()));
                        actIndicator.Hidden = true;
                    });
                }
                // Catch the exception when the async method is cancelled
                catch (System.OperationCanceledException ex)
                {
                    //   Log.Debug("WEB", $"Text load cancelled: {ex.Message}");
                }
                // Catch any other exceptinons that may have occured
                catch (Exception ex)
                {
                    // Log.Debug("WEB", ex.Message);
                }
            }, ct);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.NavigationController.SetNavigationBarHidden(false, false);
        }

        static string GetTitle(string file)
        {
            Match m = Regex.Match(file, @"<title>\s*(.+?)\s*</title>");
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            else
            {
                return string.Empty;
            }
        }

		public class TextDto : IServerDto
		{
			[JsonIgnore]
			public bool success { get; set; }
			[JsonIgnore]
			public string error_message { get; set; }

			public string contentString { get; set; }
		}

		async Task<string> GetTextAsync(string url)
        {
			var client = AppDelegate.current.ApplicationService.client;
            var textDto = await client.Get<TextDto>(new Uri(url));
			return textDto.contentString;
        }
        async Task<string> GetTextAsync(string url, CancellationToken ct)
        {
            // Check to see if a task was canceled; if so throw a canceled exception.
            // Good to check at several points, including just prior to returning the string.
            ct.ThrowIfCancellationRequested();
            string response = await GetTextAsync(url);
            ct.ThrowIfCancellationRequested();
            return response;
        }
    }
}
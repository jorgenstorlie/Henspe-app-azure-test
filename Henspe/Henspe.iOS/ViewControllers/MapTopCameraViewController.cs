using System;
using Foundation;
using Henspe.iOS.Const;
using UIKit;

namespace Henspe.iOS
{
	public partial class MapTopCameraViewController : UIViewController
	{
        // Events
        private NSObject observerHjertestarterShow;

        public MapTopCameraViewController (IntPtr handle) : base (handle)
		{
            SetupEvents();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();
        }

        private void SetupEvents()
        {
            observerHjertestarterShow = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.hjertestarterShow), HandleHjertestarterShow);
        }

        #region event handlers
        private void HandleHjertestarterShow(NSNotification obj)
        {
            /*
            if (AppDelegate.current.currentHjertestarter != null)
            {
                //UpdateData(AppDelegate.current.currentHjertestarter);
            }
            */
        }
        #endregion

        private void SetupView()
        {
            /*
            labHeader.TextColor = ColorConst.snlaText;
            labHeader.AdjustsFontSizeToFitWidth = true;
            labHeader.MinimumScaleFactor = 0.5f;
            labHeader.Font = FontConst.fontMediumRegular;

            labDistance.TextColor = ColorConst.snlaText;
            labDistance.MinimumScaleFactor = 0.5f;
            labDistance.Font = FontConst.fontSmall;

            labOpeningHours.TextColor = ColorConst.snlaText;
            labOpeningHours.Text = "";
            labOpeningHours.Font = FontConst.fontSmall;

            btnLink.SetTitle(LangUtil.Get("Hjelp113.iOS.MainViewController.OpeningHoursDisclaimer"), UIControlState.Normal);
            btnLink.TintColor = ColorConst.snlaBlue;
            btnLink.Font = FontConst.fontSmallRegular;

            txtInfo.BackgroundColor = UIColor.Clear;
            txtInfo.TextContainerInset = new UIEdgeInsets(0, -4, 0, 0);
            txtInfo.TextColor = ColorConst.snlaText;
            txtInfo.Font = FontConst.fontSmall;

            labWarning.TextColor = ColorConst.snlaText;
            labWarning.Font = FontConst.fontSmallRegular;
            labWarning.Text = LangUtil.Get("Hjelp113.iOS.MainViewController.Warning");

            btnButton.SetTitle(LangUtil.Get("Hjelp113.iOS.MainViewController.GoogleMaps"), UIControlState.Normal);
            btnButton.SetBackgroundImage(UIImage.FromFile("btn_outlined_blue"), UIControlState.Normal);
            btnButton.SetBackgroundImage(UIImage.FromFile("btn_outlined_blue_down"), UIControlState.Highlighted | UIControlState.Selected);
            btnButton.TintColor = ColorConst.snlaBlue;
            */
        }
    }
}
using System;
using CoreGraphics;
using Foundation;
using Henspe.iOS;
using Henspe.iOS.Const;
using UIKit;

namespace Henspe.iOS
{
    public partial class MapTopViewController : UIViewController
    {
        public enum SubView
        {
            undefined,
            camera,
        }

        private nfloat lastAlpha = 0.0f;
        private int lastView = -1;

        private MapTopCameraViewController mapTopCameraViewController;

        // Events
        private NSObject observerShowTopInfoEvent;
        private NSObject observerAlphaFadeEvent;
        private NSObject observerHjertestarterSelected;

        private string clickedHjertestarterParameter = "";

        public MapTopViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupSubViews();
            SetupEvents();
            SetupView();
        }

        private void SetupEvents()
        {
            observerShowTopInfoEvent = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.showTopInfo), HandleShowTopInfo);
            observerAlphaFadeEvent = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.alphaFadeIn), HandleAlphaFadeIn);
            observerHjertestarterSelected = NSNotificationCenter.DefaultCenter.AddObserver(new NSString(EventConst.hjertestarterSelected), HandleHjertestarterSelected);
        }

        private void SetupView()
        {
        }

        #region event handlers
        private void HandleAlphaFadeIn(NSNotification obj)
        {
            NSDictionary parameters = obj.UserInfo;

            NSObject objectFade = parameters[NSObject.FromObject("fadeIn")];
            string objectFadeParameter = objectFade.ToString();
            int fadeNumber = Henspe.Core.Util.ConvertUtil.ConvertStringToInt(objectFadeParameter);

            NSObject objectView = parameters[NSObject.FromObject("view")];
            string objectViewParameter = objectView.ToString();
            int view = Henspe.Core.Util.ConvertUtil.ConvertStringToInt(objectViewParameter);

            nfloat value = 0.0f;
            if (fadeNumber == 1)
                value = 1.0f;

            InvokeOnMainThread(async delegate
            {
                if (value != lastAlpha || view != lastView)
                {
                    lastAlpha = value;
                    lastView = view;

                    await UIView.AnimateAsync(0.5, () =>
                    {
                        this.View.Subviews[0].Alpha = value;
                    });
                }
            });
        }

        private void HandleShowTopInfo(NSNotification obj)
        {
            NSDictionary parameters = obj.UserInfo;

            NSObject objectShowInfo = parameters[NSObject.FromObject("showInfo")];
            string showInfoParameter = objectShowInfo.ToString();
            int showInfoParameterNumber = Core.Util.ConvertUtil.ConvertStringToInt(showInfoParameter);
            ShowInfo(showInfoParameterNumber);
        }

        private void ShowInfo(int showInfoParameterNumber)
        {
            if (showInfoParameterNumber == (int)SubView.camera)
                SetView(SubView.camera);
        }

        private void HandleHjertestarterSelected(NSNotification obj)
        {
            ShowInfo((int)SubView.camera);
        }
        #endregion

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        private void SetupSubViews()
        {
            var storyboard = UIStoryboard.FromName("Main", null);
            if (storyboard != null)
            {
                mapTopCameraViewController = storyboard.InstantiateViewController("mapTopCamera") as MapTopCameraViewController;
            }
        }

        public void SetView(SubView subView)
        {
            NSDictionary parameters;
            RemoveSubViewsFor(this.View);

            if (subView == SubView.camera)
            {
                SetFrame(mapTopCameraViewController, this.View);
                this.View.AddSubview(mapTopCameraViewController.View);
            }

            if ((int)subView != lastView)
                this.View.Subviews[0].Alpha = 0.0f;

            InvokeOnMainThread(delegate
            {
                parameters = new NSDictionary("subView", subView);
                NSNotificationCenter.DefaultCenter.PostNotificationName(EventConst.prepareTopInfoView, this, parameters);
            });
        }

        private void SetFrame(UIViewController viewController, UIView view)
        {
            viewController.View.Frame = new CGRect(0, 0, view.Frame.Width, view.Frame.Height);
        }

        private void RemoveSubViewsFor(UIView view)
        {
            foreach (UIView subView in view.Subviews)
                subView.RemoveFromSuperview();
        }
    }
}
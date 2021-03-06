﻿using System;
using Foundation;
using SNLA.Core.Util;
using UIKit;

namespace Henspe.iOS
{
	public partial class InitialViewController : UIViewController
	{
		public InitialViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

       //     UserUtil.Current.onboardingCompleted = false;

            if (UserUtil.Current.onboardingCompleted == false)
                GoToOnboarding();
            else
                GoToMain();
        }

        private void GoToOnboarding()
        {
            InvokeOnMainThread(delegate
            {
                this.PerformSegue("segueOnboarding", this);
            });
        }

        private void GoToMain()
        {
            InvokeOnMainThread(delegate
            {
                this.PerformSegue("segueMain", this);
            });
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueOnboarding")
            {
                segue.DestinationViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
            else if (segue.Identifier == "segueMain")
            {
                segue.DestinationViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
        }
    }
}

﻿// This file has been autogenerated from a class added in the UI designer.
using System;
using System.Collections.Generic;
using System.Drawing;
using Airbnb.Lottie;
using CoreGraphics;
using Foundation;
using Henspe.iOS.Const;
using Henspe.iOS.Util;
using SNLA.Core.Util;
using UIKit;

namespace Henspe.iOS
{
	public partial class InitialViewController : UIViewController
	{
		int totalPages = 3;
		int currentPage = 0;

        private LOTAnimationView _animation1;
        private LOTAnimationView _animation2;
        private LOTAnimationView _animation3;
        private List<LOTAnimationView> _animations;

        public enum NextType
		{
			Next,
            Finished
		}

        public InitialViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _animation1 = LOTAnimationView.AnimationNamed("intro");
            _animation2 = LOTAnimationView.AnimationNamed("HENSPEintropart2");
            _animation3 = LOTAnimationView.AnimationNamed("HENSPEintropart3");

            _animations = new List<LOTAnimationView>() { _animation1, _animation2, _animation3 };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (UserUtil.settings.onboardingCompleted == false)
            {
                ClearAllBeforeDrawing();
                SetupView();
                PopulateInitialPage();
            }
            else
            {
                GoToMainScreen();
            }
        }

		private void ClearAllBeforeDrawing()
		{
			currentPage = 0;
		}

		void SetupView()
        {
			pagPager.PageIndicatorTintColor = UIColor.White;
			pagPager.CurrentPageIndicatorTintColor = ColorConst.snlaRed;

			if (UserUtil.settings.onboardingCompleted == false)
            {
                labHeader.Text = LangUtil.Get("Initial.Header");
                labHeader.TextColor = ColorConst.snlaRed;

                btnSkip.SetTitleColor(ColorConst.snlaRed, UIControlState.Normal);
                btnSkip.SetTitle(LangUtil.Get("Initial.Skip"), UIControlState.Normal);
                btnSkip.Hidden = false;

                pagPager.Pages = totalPages;

                btnNext.SetTitleColor(ColorConst.snlaRed, UIControlState.Normal);
                btnNext.SetTitle(LangUtil.Get("Initial.Next") + " >", UIControlState.Normal);
            }
            else
            {
                btnSkip.Hidden = true;
                pagPager.Hidden = true;
                btnNext.Hidden = true;
            }

			ShowActivityIndicatorForNext(NextType.Next);
        }

		private void ShowActivityIndicatorForNext(NextType nextType)
        {
			btnNext.Hidden = false;

			if (nextType == NextType.Next)
            {
                btnNext.SetTitle(LangUtil.Get("Initial.Next") + " >", UIControlState.Normal);
            }
			else if(nextType == NextType.Finished)
            {
				btnNext.SetTitle(LangUtil.Get("Initial.Finished"), UIControlState.Normal);
            }
        }

        void PopulateInitialPage()
        {
            int pages = totalPages;
            pagPager.Pages = pages;

            var view = ChangeAnimationView(0);

            view.Play();
        }

        private LOTAnimationView ChangeAnimationView(int index)
        {
            if(index > 0)
            {
                var oldView = _animations[index - 1];
                oldView.RemoveFromSuperview();
            }
            var newView = _animations[index];
            newView.ContentMode = UIViewContentMode.ScaleAspectFit;

            _animation1.TranslatesAutoresizingMaskIntoConstraints = false;
            viewAnimation.AddSubview(newView);

            var views = new NSMutableDictionary();
            views.Add(new NSString("animationView"), newView);
            var constraintsH = NSLayoutConstraint.FromVisualFormat("H:|-[animationView]-|", NSLayoutFormatOptions.AlignAllTop, null, views);
            var constraintsV = NSLayoutConstraint.FromVisualFormat("V:|-[animationView]-|", NSLayoutFormatOptions.AlignAllLeft, null, views);
            NSLayoutConstraint.ActivateConstraints(constraintsH);
            NSLayoutConstraint.ActivateConstraints(constraintsV);

            return newView;
        }

        partial void OnSkipClicked(NSObject sender)
        {
            GoToMain();
        }

        partial void OnNextClicked(NSObject sender)
        {
            currentPage = currentPage + 1;

            if (currentPage == totalPages)
                GoToMain();
            else
                GotoPage(currentPage);
        }

        void GotoPage(int gotoPage)
        {
            var view = ChangeAnimationView(gotoPage);
            pagPager.CurrentPage = gotoPage;
            view.PlayAsync();
        }

        private void TellThatUserNeedNetworkAtFirstRun()
        {
            UIAlertView alert = new UIAlertView(LangUtil.Get("Initial.Alert.NoNetwork.Title"),
                 LangUtil.Get("Initial.Alert.NoNetwork.Message"),
                 null,
                 LangUtil.Get("Alert.OK"),
                 null);

            alert.Clicked += (s, b) =>
            {
                if (b.ButtonIndex == 0)
                {
                    // OK chosen
                    //GetEvent();
                }
            };

            alert.Show();
        }

		private void GoToMain()
        {
            UserUtil.settings.onboardingCompleted = true;
            GoToMainScreen();
        }

        private void GoToMainScreen()
        {
            InvokeOnMainThread(delegate 
            {
                this.PerformSegue("segueInitialized", this);
            });
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueInitialized")
            {
				ClearAllBeforeDrawing();
            }
        }

        [Action("UnwindToInit:")]
        public void UnwindToInitViewController(UIStoryboardSegue segue)
        {
        }
    }
}
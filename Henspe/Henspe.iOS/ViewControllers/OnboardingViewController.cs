using System;
using Airbnb.Lottie;
using Foundation;
using Henspe.iOS.Const;
using SNLA.Core.Util;
using SNLA.iOS.Util;
using UIKit;

namespace Henspe.iOS
{
    public partial class OnboardingViewController : UIViewController
    {
        int totalPages = 3;
        int currentPage = 0;

        private LOTAnimationView animation;

        public enum NextType
        {
            Next,
            Finished
        }

        public OnboardingViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            animation = LOTAnimationView.AnimationNamed("intro");
            this.viewAnimation.AddSubview(animation);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            animation.ContentMode = UIViewContentMode.ScaleAspectFit;
            animation.Frame = this.viewAnimation.Bounds;
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            View.BackgroundColor = ColorHelper.FromType(ColorType.SystemBackground);

            ClearAllBeforeDrawing();
            SetupView();
            PopulateInitialPage();
        }

        private void ClearAllBeforeDrawing()
        {
            currentPage = 0;
        }

        void SetupView()
        {
            pagPager.PageIndicatorTintColor = ColorHelper.FromType(ColorType.SecondaryRedBackground);
            pagPager.UserInteractionEnabled = false;
            pagPager.CurrentPageIndicatorTintColor = ColorHelper.FromType(ColorType.RedBackground); 
            labHeader.Text = LangUtil.Get("Initial.Header");
            labHeader.TextColor = ColorHelper.FromType(ColorType.RedBackground);
            labDescription.TextColor = ColorHelper.FromType(ColorType.Label);
            labDescription.Text = string.Empty;
            //  viewAnimation.BackgroundColor = UIColor.FromRGBA(50, 0, 0, 40);

            if (UserUtil.Current.onboardingCompleted == false)
            {
                btnSkip.SetTitleColor(ColorHelper.FromType(ColorType.RedBackground) ,UIControlState.Normal);
                btnSkip.SetTitle(LangUtil.Get("Initial.Skip"), UIControlState.Normal);
                btnSkip.Hidden = false;

                pagPager.Pages = totalPages;

                btnNext.SetTitleColor(ColorHelper.FromType(ColorType.RedBackground), UIControlState.Normal);
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
                btnSkip.Hidden = false;
                btnNext.SetTitle(LangUtil.Get("Initial.Next") + " >", UIControlState.Normal);
            }
            else if (nextType == NextType.Finished)
            {
                btnSkip.Hidden = true;
                btnNext.SetTitle(LangUtil.Get("Initial.Finished"), UIControlState.Normal);
            }
        }

        void PopulateInitialPage()
        {
            int pages = totalPages;
            pagPager.Pages = pages;

            var view = ChangeAnimationView(0);
        }

        private LOTAnimationView ChangeAnimationView(int index)
        {
            int start = 0;
            int stop = 0;

            if (index >= 2)
                ShowActivityIndicatorForNext(NextType.Finished);
            else
                ShowActivityIndicatorForNext(NextType.Next);

            if (index != 3)
            {
                InvokeOnMainThread(() =>
            {
                labDescription.Alpha = 1.0f;
                UIView.AnimateAsync(0.2, () =>
                {
                    labDescription.Alpha = 0.0f;
                    View.LayoutSubviews(); // <- Dette er moder-objektet til txtEmail
                });
            });
            }
            switch (index)
            {
                case 0:
                    start = 0;
                    stop = 125;
                    break;
                case 1:
                    start = 125;
                    stop = 199;
                    break;
                case 2:
                    start = 199;
                    stop = 270;
                    break;
                case 3:
                    start = 270;
                    stop = 318;
                    break;
            }
            //    animation.BackgroundColor = UIColor.FromRGBA(50, 50, 50, 40);

            animation.PlayFromFrame(start, stop, (animationFinished) =>
            {
                if (index == 2)
                    ChangeAnimationView(3);

                if (index != 3)
                {
                    switch (index)
                    {
                        case 0:
                            labDescription.Text = LangUtil.Get("Initial.PageOne.Text");
                            break;
                        case 1:
                            labDescription.Text = LangUtil.Get("Initial.PageTwo.Text");
                            break;
                        case 2:
                            labDescription.Text = LangUtil.Get("Initial.PageThree.Text");
                            break;
                    }

                    InvokeOnMainThread(() =>
                    {
                        labDescription.Alpha = 0.0f;
                        UIView.AnimateAsync(0.2, () =>
                        {
                            labDescription.Alpha = 1.0f;
                            View.LayoutSubviews(); // <- Dette er moder-objektet til txtEmail
                        });
                    });
                }
            });

            var newView = animation;
            //    animation.TranslatesAutoresizingMaskIntoConstraints = false;
            //      animation.ContentMode = UIViewContentMode.ScaleAspectFit;
            //    animation.Frame = new CoreGraphics.CGRect(0, 0, this.viewAnimation.Bounds.Size.Width, this.viewAnimation.Bounds.Size.Height);

            /*
            if (index > 0)
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
            */
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
            UserUtil.Current.onboardingCompleted = true;
            GoToMainScreen();
        }

        private void GoToMainScreen()
        {
            InvokeOnMainThread(delegate
            {
                this.PerformSegue("segueMain", this);
            });
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueMain")
            {
                segue.DestinationViewController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                ClearAllBeforeDrawing();
            }
        }
    }
}
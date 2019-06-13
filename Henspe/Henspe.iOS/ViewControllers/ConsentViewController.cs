﻿// This file has been autogenerated from a class added in the UI designer.
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AVFoundation;
using AVKit;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using Henspe.Core.Communication;
using Henspe.iOS.Const;
using UIKit;
using SNLA.Core.Util;
using SNLA.Core.Const;
using SNLA.iOS.Util;

namespace Henspe.iOS
{
    public partial class ConsentViewController : UIViewController, ICALayerDelegate
    {
        private nfloat lastConstant;
        private CGPoint lastOffset;
        private UITapGestureRecognizer tapGesture;
        private bool isEmailFieldActive = false;
        private nfloat lastBottom;

        RegEmailSMSService regEmailSMSService;

        private nfloat originalHeightTxtEmail = 0;
        private AVPlayer avPlayer;
        private AVPlayerViewController avPlayerViewController;

        private string emailAddress = "";
        private bool originalSMSValue = true;
        private bool originalEmailValue = false;
        private string originalEmailAddress = "";

        // Events
        private NSObject observerKeyboardShow;
        private NSObject observerKeyboardHide;

        public ConsentViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupView();
            SetupVideo();
            SetupValues();
        }

        private void SetupValues()
        {
            if (UserUtil.Current.consentAgreed == ConsentAgreed.NotSet)
            {
                // No selection done before
                emailAddress = "";
                originalSMSValue = true;
                originalEmailValue = false;
                originalEmailAddress = "";
            }
            else if (UserUtil.Current.consentAgreed == ConsentAgreed.False)
            {
                // No thanks selected before
                emailAddress = "";
                originalSMSValue = false;
                originalEmailValue = false;
                originalEmailAddress = "";
            }
            else
            {
                // Some agreement done before
                originalSMSValue = UserUtil.Current.consentSMS;
                originalEmailValue = UserUtil.Current.consentEmail;

                if (originalEmailValue == true)
                {
                    originalEmailAddress = UserUtil.Current.consentEmailAddress;
                    emailAddress = UserUtil.Current.consentEmailAddress;
                }
                else
                {
                    originalEmailAddress = "";
                    emailAddress = "";
                }
            }
        }

        private void SetupView()
        {
            this.Title = LangUtil.Get("Consent.Title.Text");

            View.BackgroundColor = ColorConst.snlaBackground;

            labHeader.TextColor = ColorConst.snlaText;
            labHeader.Font = FontConst.fontMedium;
            labHeader.Text = LangUtil.Get("Consent.Header.Text");

            labConsentHeader.TextColor = ColorConst.snlaText;
            labConsentHeader.Font = FontConst.fontMediumRegular;
            labConsentHeader.Text = LangUtil.Get("Consent.Approval.Text");

            labSMS.TextColor = ColorConst.snlaBlue;
            labSMS.Font = FontConst.fontMediumRegular;
            labSMS.Text = LangUtil.Get("Consent.SMS.Text");

            labEmail.TextColor = ColorConst.snlaBlue;
            labEmail.Font = FontConst.fontMediumRegular;
            labEmail.Text = LangUtil.Get("Consent.Email.Text");

            btnReadMore.SetTitle(LangUtil.Get("Consent.ReadMore.Text"), UIControlState.Normal);
            btnReadMore.TitleLabel.Font = FontConst.fontHeading;
            btnReadMore.SetTitleColor(ColorConst.snlaBlue, UIControlState.Normal);

            btnAccept.SetTitle(LangUtil.Get("Consent.UpdateButton.Text"), UIControlState.Normal);
            btnAccept.SetTitleColor(ColorConst.snlaTextLight, UIControlState.Normal);
            btnAccept.SetTitleColor(ColorConst.snlaTextLightDisabled, UIControlState.Disabled);
            btnAccept.SetBackgroundImage(UIImage.FromFile("btn_blue.png"), UIControlState.Normal);
            btnAccept.SetBackgroundImage(UIImage.FromFile("btn_blue_down.png"), UIControlState.Highlighted | UIControlState.Selected);
            btnAccept.SetBackgroundImage(UIImage.FromFile("btn_blue_disabled.png"), UIControlState.Disabled);
            btnAccept.TitleLabel.Font = FontConst.fontMediumRegular;

            btnDeny.SetTitleColor(ColorConst.snlaBlue, UIControlState.Normal);
            btnDeny.TitleLabel.Font = FontConst.fontMediumRegular;
            btnDeny.SetTitle(LangUtil.Get("Consent.NoThanksButton.Text"), UIControlState.Normal);

            txtEmail.Placeholder = LangUtil.Get("Consent.TextInput.Placeholder");
            txtEmail.Hidden = true;
            originalHeightTxtEmail = txtEmail.Frame.Height;
            constraintEmailHeight.Constant = 0;

            swiSMS.SetState(UserUtil.Current.consentSMS, originalSMSValue);
            swiEmail.SetState(UserUtil.Current.consentEmail, originalEmailValue);

            txtEmail.Text = originalEmailAddress;

            swiSMS.AccessibilityLabel = LangUtil.Get("Consent.SMS.Accessibility.Label");
            swiSMS.AccessibilityHint = LangUtil.Get("Consent.SMS.Accessibility.Hint");
            swiEmail.AccessibilityLabel = LangUtil.Get("Consent.Email.Accessibility.Label");
            swiEmail.AccessibilityHint = LangUtil.Get("Consent.Email.Accessibility.Hint");
            btnAccept.AccessibilityLabel = LangUtil.Get("Consent.UpdateButton.Accessibility.Label");
            btnAccept.AccessibilityHint = LangUtil.Get("Consent.UpdateButton.Accessibility.Label");
            btnReadMore.AccessibilityLabel = LangUtil.Get("Consent.ReadMore.Accessibility.Label");
            btnReadMore.AccessibilityHint = LangUtil.Get("Consent.ReadMore.Accessibility.Label");
            btnDeny.AccessibilityLabel = LangUtil.Get("Consent.NoThanksButton.Accessibility.Label");
            btnDeny.AccessibilityHint = LangUtil.Get("Consent.NoThanksButton.Accessibility.Label");
            txtEmail.AccessibilityLabel = LangUtil.Get("Consent.EmailAdress.Accessibility.Label");
            txtEmail.AccessibilityHint = LangUtil.Get("Consent.EmailAdress.Accessibility.Label");
        }

        private void SetupVideo()
        {
            var url = NSUrl.FromFilename("NLAInfo.mp4");
            avPlayer = new AVPlayer(url);
            avPlayerViewController = new AVPlayerViewController();
            avPlayerViewController.Player = avPlayer;
            viewMovieContainer.AddSubview(avPlayerViewController.View);
            avPlayerViewController.ShowsPlaybackControls = true;
            avPlayer.Muted = true;

            NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, (obj) =>
            {
                avPlayer.Seek(CoreMedia.CMTime.Zero);
                avPlayer.Play(); obj.Dispose();
            },
            avPlayer.CurrentItem);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.NavigationController.SetNavigationBarHidden(false, false);

            avPlayerViewController.View.Frame = viewMovieContainer.Bounds;

            SetSMS(originalSMSValue);
            SetEmail(originalEmailValue);
            txtEmail.Text = originalEmailAddress;

            if (UserUtil.Current.consentAgreed != ConsentAgreed.NotSet)
            {
                btnAccept.SetTitle(LangUtil.Get("Consent.UpdateButton.Text"), UIControlState.Normal);
                btnDeny.Hidden = true;
                constraintNoThankYouHeight.Constant = 0;
            }
            else
            {
                btnAccept.SetTitle(LangUtil.Get("Consent.SendButton.Text"), UIControlState.Normal);
                btnDeny.Hidden = false;
                constraintNoThankYouHeight.Constant = 33;
            }

            SetupContentHeight();
            SetupVideoHeight();
            SetupEvents();
            SetupTouchGesture();
            SetStatusAcceptButton();
        }

        private void SetupContentHeight()
        {
            nfloat navBarHeight = this.NavigationController.NavigationBar.Frame.Size.Height;
            if (DeviceUtil.HasExtraTop() == true)
            {
                nfloat extraTop = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Top;
                constraintContentHeight.Constant = -navBarHeight - extraTop;
            }
            else
            {
                constraintContentHeight.Constant = -navBarHeight;
            }
        }

        private void SetupVideoHeight()
        {
            nfloat width = this.View.Frame.Width;

            if (DeviceUtil.IsIPhoneOlderThanIPhone5())
            {
                // iPhone4
                constraintVideoWidth.Constant = width;
                constraintVideoHeight.Constant = 0;
                viewMovieContainer.Hidden = true;
            }
            else
            {
                // iPhone5 or higher
                nfloat height = width / 1.75f;
                constraintVideoWidth.Constant = width;
                constraintVideoHeight.Constant = height;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            if (observerKeyboardShow != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(observerKeyboardShow);

            if (observerKeyboardHide != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(observerKeyboardHide);

            if (tapGesture != null)
                viewContent.RemoveGestureRecognizer(tapGesture);

            base.ViewWillDisappear(animated);
        }

        private void SetupTouchGesture()
        {
            tapGesture = new UITapGestureRecognizer(OnViewContentTouched);
            tapGesture.NumberOfTapsRequired = 1;
            tapGesture.CancelsTouchesInView = true;
            viewContent.AddGestureRecognizer(tapGesture);
        }

        private void OnViewContentTouched()
        {
            if (isEmailFieldActive == false)
            {
                return;
            }
            else
            {
                txtEmail.ResignFirstResponder();
                isEmailFieldActive = false;
            }
        }

        private void SetupEvents()
        {
            observerKeyboardShow = UIKeyboard.Notifications.ObserveWillShow(HandleKeyboardShow);
            observerKeyboardHide = UIKeyboard.Notifications.ObserveWillHide(HandleKeyboardHide);
        }

        #region keyboard
        private void HandleKeyboardShow(object sender, UIKeyboardEventArgs e)
        {
            var height = KeyboardUtil.GetKeyboardHeight(e.FrameEnd, this.View);

            // Increase contentView's height by keyboard height
            UIView.Animate(0.3f, () =>
            {
                constraintContentHeight.Constant = height;
                constraintBottom.Constant = height;
            });

            nfloat collapseSpace = height; //  - distanceToBottom;

            if (collapseSpace < 0)
            {
                // no collapse
                return;
            }

            // set new offset for scroll view
            UIView.Animate(0.3f, () =>
            {
                // scroll to the position above keyboard 10 points
                scrView.ContentOffset = new CGPoint(lastOffset.X, collapseSpace + 10);
            });
        }

        private void HandleKeyboardHide(object sender, UIKeyboardEventArgs e)
        {
            UIView.Animate(0.3f, () =>
            {
                constraintContentHeight.Constant = lastConstant; // viewContent.Frame.Height;
                constraintBottom.Constant = lastBottom;
                scrView.ContentOffset = lastOffset;
            });
        }

        partial void OnEmailShouldBeginEditing(NSObject sender)
        {
            isEmailFieldActive = true;
            lastConstant = constraintContentHeight.Constant;
            lastOffset = scrView.ContentOffset;
            lastBottom = constraintBottom.Constant;
        }

        partial void OnEmailShouldReturn(NSObject sender)
        {
            txtEmail.ResignFirstResponder();
            isEmailFieldActive = false;
        }
        #endregion

        private void SetSMS(bool value)
        {
            swiSMS.On = value;
        }

        private void SetEmail(bool value)
        {
            swiEmail.On = value;

            InvokeOnMainThread(delegate
            {
                if (swiEmail.On)
                {
                    txtEmail.Alpha = 0.0f;
                    UIView.AnimateAsync(0.2, () =>
                    {
                        constraintEmailHeight.Constant = originalHeightTxtEmail;
                        txtEmail.Hidden = false;
                        txtEmail.Alpha = 1.0f;
                        View.LayoutSubviews();
                    });
                }
                else
                {
                    UIView.AnimateAsync(0.2, () =>
                    {
                        constraintEmailHeight.Constant = 0;
                        txtEmail.Hidden = true;
                        txtEmail.Alpha = 0.0f;
                        View.LayoutSubviews();
                    });
                }
            });
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            avPlayer.Play();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            avPlayer.Pause();
        }

        #region validate
        private void SetStatusAcceptButton()
        {
            if (UserUtil.Current.consentAgreed != ConsentAgreed.NotSet)
            {
                // Set before. React on changes
                bool isSMSSwitchChanged = swiSMS.On != originalSMSValue ? true : false;
                bool isEmailSwitchChanged = swiEmail.On != originalEmailValue ? true : false;
                bool isOriginalEmailChanged = originalEmailAddress != txtEmail.Text ? true : false;

                if (isSMSSwitchChanged == true)
                {
                    if (swiEmail.On == true && isEmailSwitchChanged == true)
                    {
                        // Email is on. Any email changes?
                        if (AppUserUtil.IsValidEmail(txtEmail.Text) == true && isOriginalEmailChanged)
                            btnAccept.Enabled = true;
                        else
                            btnAccept.Enabled = false;
                    }
                    else
                    {
                        btnAccept.Enabled = true;
                    }
                }
                else if (swiEmail.On == false && isEmailSwitchChanged == true)
                {
                    btnAccept.Enabled = true;
                }
                else if (swiEmail.On == true)
                {
                    // Email is on. Any email changes?
                    if (AppUserUtil.IsValidEmail(txtEmail.Text) == true && isOriginalEmailChanged)
                        btnAccept.Enabled = true;
                    else
                        btnAccept.Enabled = false;
                }
                else
                {
                    // No changes
                    btnAccept.Enabled = false;
                }
            }
            else
            {
                // Not set earlier
                if (swiSMS.On == false && swiEmail.On == false)
                {
                    btnAccept.Enabled = false;
                }
                else if (swiSMS.On == false && swiEmail.On == true)
                {
                    // Email selected
                    if (AppUserUtil.IsValidEmail(txtEmail.Text) == true)
                        btnAccept.Enabled = true;
                    else
                        btnAccept.Enabled = false;
                }
                else if (swiSMS.On == true && swiEmail.On == false)
                {
                    btnAccept.Enabled = true;
                }
                else if (swiSMS.On == true && swiEmail.On == true)
                {
                    // Email selected
                    if (AppUserUtil.IsValidEmail(txtEmail.Text) == true)
                        btnAccept.Enabled = true;
                    else
                        btnAccept.Enabled = false;
                }
            }
        }
        #endregion

        #region control events
        partial void OnReadMoreClicked(NSObject sender)
        {
            this.PerformSegue("segueInfo", this);
        }

        partial void OnSwiSMSChanged(NSObject sender)
        {
            SetSMS(swiSMS.On);
            SetStatusAcceptButton();
        }

        partial void OnSwiEmailChanged(NSObject sender)
        {
            SetEmail(swiEmail.On);
            SetStatusAcceptButton();
        }

        partial void OnEmailEditingChanged(NSObject sender)
        {
            SetStatusAcceptButton();
        }

        partial void OnAcceptClicked(NSObject sender)
        {
            InvokeOnMainThread(delegate
            {
                UserUtil.Current.consentSMS = swiSMS.On;
                UserUtil.Current.consentEmail = swiEmail.On;

                if (swiEmail.On)
                    UserUtil.Current.consentEmailAddress = txtEmail.Text;

                RegisterOrDeregister();
            });
        }

        private async void RegisterOrDeregister()
        {
            InvokeOnMainThread(async delegate
            {
                btnAccept.Enabled = false;

                // ShowWaitSpinnerWithMessage(AppDelegate.current.mainViewController.View, LangUtil.Get("Spinner.PleaseWait"));
                await SpinnerUtil.ShowWaitSpinner(AppDelegate.current.mainViewController.View, 10, LangUtil.Get("Spinner.PleaseWait"), SpinnerUtil.SpinnerType.text);// ShowWaitSpinnerWithMessage(AppDelegate.current.mainViewController.View, LangUtil.Get("Spinner.PleaseWait"));
                await Task.Delay(300);

                if (regEmailSMSService == null)
                    regEmailSMSService = new RegEmailSMSService();

                if (UserUtil.Current.consentAgreed != ConsentAgreed.NotSet)
                {
                    // Change
                    bool isSMSChanged = originalSMSValue != swiSMS.On ? true : false;
                    bool isEmailChanged = originalEmailValue != swiEmail.On ? true : false;
                    bool isEmailAddressChanged = originalEmailAddress != txtEmail.Text ? true : false;

                    await UnregSMSEmail(isSMSChanged, isEmailChanged, isEmailAddressChanged);
                    await RegSMSEmail(isSMSChanged, isEmailChanged, isEmailAddressChanged);
                }
                else
                {
                    // First time
                    bool hasEmailText = swiEmail.On ? true : false;
                    await RegSMSEmail(swiSMS.On, swiEmail.On, hasEmailText);

                    UserUtil.Current.consentAgreed = ConsentAgreed.True;
                }

                await SpinnerUtil.HideWaitSpinner(AppDelegate.current.mainViewController.View);
                ShowSuccessMessage();

                btnAccept.Enabled = true;
            });
        }

        private async Task UnregSMSEmail(bool isSMSChanged, bool isEmailChanged, bool isEmailAddressChanged)
        {
            bool isSMSUnreg = isSMSChanged == true && swiSMS.On == false ? true : false;
            bool isEmailUnreg = isEmailChanged == true && swiEmail.On == false ? true : false;

            string mobilUnreg = isSMSUnreg ? UserUtil.Current.phoneNumber : null;
            string epostUnreg = isEmailUnreg ? UserUtil.Current.consentEmailAddress : null;

            if (isSMSUnreg == true || isEmailUnreg == true || (swiEmail.On == true && isEmailAddressChanged == true && originalEmailAddress.Length > 0))
            {
                if (swiEmail.On == true && isEmailAddressChanged == true)
                    epostUnreg = originalEmailAddress;

                RegEmailSMSResultDto regEmailSMSUnregResultDto = await regEmailSMSService.UnRegEmailSMS(mobilUnreg, epostUnreg, SettingsConst.OsTypes.iOS);

                if (regEmailSMSUnregResultDto.success == true)
                {
                    if (regEmailSMSUnregResultDto.epost_off == true)
                        UserUtil.Current.consentEmail = false;

                    if (regEmailSMSUnregResultDto.mob_off == true)
                        UserUtil.Current.consentSMS = false;
                }
            }
        }

        private async Task RegSMSEmail(bool isSMSChanged, bool isEmailChanged, bool isEmailAddressChanged)
        {
            bool isSMSReg = isSMSChanged == true && swiSMS.On == true ? true : false;
            bool isEmailReg = isEmailChanged == true && swiEmail.On == true ? true : false;

            string mobilReg = isSMSReg ? UserUtil.Current.phoneNumber : null;
            string epostReg = isEmailReg ? txtEmail.Text : null;

            if (isSMSReg == true || isEmailReg == true || (swiEmail.On == true && isEmailAddressChanged == true))
            {
                if (swiEmail.On == true && isEmailAddressChanged == true)
                    epostReg = txtEmail.Text;

                RegEmailSMSResultDto regEmailSMSResultDto = await regEmailSMSService.RegEmailSMS(mobilReg, epostReg, SettingsConst.OsTypes.iOS);

                if (regEmailSMSResultDto.success == true)
                {
                    if (regEmailSMSResultDto.epost_on == true)
                        UserUtil.Current.consentEmail = true;

                    if (regEmailSMSResultDto.mob_on == true)
                        UserUtil.Current.consentSMS = true;
                }
            }
        }

        private async void ShowSuccessMessage()
        {
            await SpinnerUtil.ShowWaitSpinner(AppDelegate.current.mainViewController.View, 2, LangUtil.Get("Consent.Thanks.Text"), SpinnerUtil.SpinnerType.text);
            //await SpinnerUtil.ShowSpinnerMessage(this.View, LangUtil.Get("Consent.Thanks.Text"), 2, SpinnerUtil.IconType.Check);

            NavigationController.PopViewController(true);
        }

        partial void OnDenyClicked(NSObject sender)
        {
            StoreNoThankYou();
        }

        private async Task StoreNoThankYou()
        {
            UserUtil.Current.consentAgreed = ConsentAgreed.False;

            await SpinnerUtil.ShowWaitSpinner(AppDelegate.current.mainViewController.View, 2, LangUtil.Get("Consent.ThanksForNoThanks.Text"), SpinnerUtil.SpinnerType.text);
            //await SpinnerUtil.ShowSpinnerMessage(this.View, LangUtil.Get("Consent.ThanksForNoThanks.Text"), 2, SpinnerUtil.IconType.Check);

            NavigationController.PopViewController(true);
        }
        #endregion

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "segueInfo")
            {
            }
        }

        /*
        partial void AcceptClicked(NSObject sender)
        {
            Task.Run(async () =>
            {
                return await _viewmodel.Update();
            }).ContinueWith((result) =>
            {
                BeginInvokeOnMainThread(delegate 
                {
                    if (result.Result)
                    {
                        UIAlertController alert = UIAlertController.Create(null, LangUtil.Get("HelpUs.Thanks.Text"), UIAlertControllerStyle.Alert);
                        PresentViewController(alert, true, null);
                        DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, new TimeSpan(0, 0, 1)), () =>
                        {
                            alert.DismissViewController(true, () => NavigationController.PopViewController(true));
                        });
                    }
                    else
                    {
                        UIAlertController alert = UIAlertController.Create(null, LangUtil.Get("HelpUs.Error.Text"), UIAlertControllerStyle.Alert);
                        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (a) => alert.DismissViewController(true, null)));
                        PresentViewController(alert, true, null);
                    }
                });
            });
        }
        */
    }
}
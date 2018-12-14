using System;
using System.Threading.Tasks;
using CoreGraphics;
using MBProgressHUD;
using UIKit;

namespace Henspe.iOS.Util
{
	public class SpinnerUtil
	{
        static public MTMBProgressHUD hud;
        static public bool hudVisible;

        public enum IconType
        {
            None,
            Check,
            Cross
        } 

        public SpinnerUtil()
        {
        }

        static public async Task ShowWaitSpinner(UIKit.UIView view)
        {
            if (hudVisible == false)
            {
                view.InvokeOnMainThread(delegate
                {
                    view.UserInteractionEnabled = false;
                    hud = showSpinner(hud, view, LangUtil.Get("Spinner.PleaseWait"), 0, IconType.None);
                    //inputHud = SpinnerUtil.showSpinner(inputHud, view, LangUtil.Get("Spinner.PleaseWait", null), 0);
                });

                hudVisible = true;

                await Task.Delay(100);

                view.InvokeOnMainThread(delegate
                {
                    view.UserInteractionEnabled = true;
                });
            }

            return;
        }

        static public async Task ShowWaitSpinnerWithMessage(UIKit.UIView view, string message)
        {
            if (hudVisible == false)
            {
                view.InvokeOnMainThread(delegate
                {
                    view.UserInteractionEnabled = false;
                    hud = showSpinner(hud, view, LangUtil.Get(message), 0, IconType.None);
                    //inputHud = SpinnerUtil.showSpinner(inputHud, view, LangUtil.Get("Spinner.PleaseWait", null), 0);
                });

                hudVisible = true;

                await Task.Delay(100);

                view.InvokeOnMainThread(delegate
                {
					view.UserInteractionEnabled = true;
                });
            }

            return;
        }

        static public async Task HideWaitSpinner(UIKit.UIView view)
        {
            if (hud != null && hudVisible)
            {
                view.InvokeOnMainThread(delegate
                {
                    view.UserInteractionEnabled = true;
                    hideSpinner(hud);
                    //SpinnerUtil.hideSpinner(inputHud);
                });

                await Task.Delay(100);
            }

            hudVisible = false;
        }

        static public async Task ShowSpinnerMessage(UIKit.UIView view, string message, int autoHideSeconds, IconType iconType)
        {
            if (hudVisible == false)
            {
                view.InvokeOnMainThread(delegate
                {
                    view.UserInteractionEnabled = false;
                    hud = showSpinner(hud, view, message, autoHideSeconds, iconType);
                });

                await Task.Delay(autoHideSeconds * 1000);

                view.InvokeOnMainThread(delegate
                {
                    view.UserInteractionEnabled = true;
                });
            }

            return;
        }

        static private MTMBProgressHUD showSpinner(MTMBProgressHUD inputHud, UIKit.UIView view, string spinnerText, int autohideSeconds, IconType iconType)
        {
            if (inputHud != null)
                inputHud = null;

            if (autohideSeconds > 0)
            {
                string iconString = "";
                if(iconType == IconType.None)
                {
                    hud = new MTMBProgressHUD(view)
                    {
                        CustomView = new UIImageView(UIImage.FromBundle(iconString)),
                        LabelText = spinnerText,
                        DimBackground = true,
                        Mode = MBProgressHUDMode.Text,
                        RemoveFromSuperViewOnHide = true
                    };
                }
                else
                {
                    if(iconType == IconType.Check)
                    {
                        iconString = "37x-Checkmark.png";
                    }
                    else if(iconType == IconType.Cross)
                    {
                        iconString = "ic_cross_white.png";
                    }

                    hud = new MTMBProgressHUD(view)
                    {
                        CustomView = new UIImageView(UIImage.FromFile(iconString)),
                        LabelText = spinnerText,
                        DimBackground = true,
                        Mode = MBProgressHUDMode.CustomView,
                        RemoveFromSuperViewOnHide = true
                    };

                    hud.CustomView.Frame = new CGRect(hud.CustomView.Frame.X, hud.CustomView.Frame.Y, 30, 30);
                }
            }
            else
            {
                hud = new MTMBProgressHUD(view)
                {
                    LabelText = spinnerText,
                    DimBackground = true,
                    Mode = MBProgressHUDMode.Indeterminate,
                    RemoveFromSuperViewOnHide = true
                };
            }

            view.AddSubview(hud);

            hud.Show(animated: true);

            if (autohideSeconds > 0)
            {
                hud.Hide(animated: true, delay: autohideSeconds);
            }

            return hud;
        }

        static private void hideSpinner(MTMBProgressHUD hud)
        {
            if (hud != null)
                hud.Hide(animated: true);

            if (hud != null)
                hud = null;
        }
    }
}
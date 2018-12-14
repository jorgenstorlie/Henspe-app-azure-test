using System;
using System.Text;
using System.IO;
using Foundation;
using UIKit;
using System.Drawing;
using CoreGraphics;
using System.Diagnostics;

namespace Henspe.iOS.Util
{
	public class KeyboardUtil
	{
        public KeyboardUtil ()
		{
		}

        static public nfloat GetKeyboardHeight(NSNotification notification, UIView view)
		{
            //Debug.WriteLine("notification: " + notification);

            // Get the size of the keyboard.
            var value = (NSValue)notification.UserInfo.ValueForKey(UIKeyboard.FrameEndUserInfoKey);
            RectangleF rawFrame = value.RectangleFValue;
            CGRect keyboardFrame = view.ConvertRectFromView(rawFrame, null);
            return GetKeyboardHeight(keyboardFrame, view);
		}

        static public nfloat GetKeyboardHeight(CGRect frame, UIView view)
        {
            nfloat keyboardHeight;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                keyboardHeight = frame.Height - view.SafeAreaInsets.Bottom;
            }
            else
            {
                keyboardHeight = frame.Height;
            }

            return keyboardHeight;
        }
    }
}
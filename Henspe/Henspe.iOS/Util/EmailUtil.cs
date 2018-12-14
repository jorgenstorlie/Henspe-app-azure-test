using System;
using UIKit;
using Foundation;
using CoreAnimation;
using Henspe.iOS.Const;
using CoreGraphics;
using System.Text.RegularExpressions;

namespace Henspe.iOS.Util
{
	public class EmailUtil
	{
        public EmailUtil()
		{
		}

        static public bool IsEmailValid(string email)
        {
            Regex regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (!string.IsNullOrWhiteSpace(email))
                return regex.IsMatch(email);
            else
                return false;
        }
	}
}
using System;
using Foundation;
using System.Collections.Generic;
using UIKit;
using Henspe.Core.Const;
using System.Linq;
using System.Diagnostics;
using System.Timers;
using Henspe.iOS.Const;

namespace Henspe.iOS.Util
{
	public class FlashTextUtil
	{
		static private bool positionAnimationInProgress = false;
		static private bool addressAnimationInProgress = false;
		static private Timer checkTimer = null;

		public enum Type
        {
            Position,
            Address,
        }

		public FlashTextUtil ()
		{
		}
			
		static public string FlashChangedText (string lastText, string newText, UILabel labText, Type type)
		{
            if (labText == null)
                return ""; 

   //         if (newText == null || (newText != null && newText.Length == 0))
   //         {
			//	return lastText;
   //         }

			//if ((type == Type.Position && positionAnimationInProgress == true) && (type == Type.Address && addressAnimationInProgress == true))
   //         {
			//	return newText;
   //         }

			//if (lastText == newText || lastText.Length == 0)
   //         {
   //             labText.Alpha = 1.0f;
			//	labText.Text = newText;

			//	return newText;
   //         }
            
			//var attributedString = new NSMutableAttributedString (newText);
			//if ((lastText.Length > 0 && lastText != newText) || ((type == Type.Position && positionAnimationInProgress == true) || (type == Type.Address && addressAnimationInProgress == true)))
			//{
			//	List<string> diff = GetWordDiffFor (newText, lastText);
			//	foreach (string word in diff)
			//	{
			//		int pos = newText.IndexOf (word);
			//		attributedString = SetAttributeInString (attributedString, pos, word.Length, AttributeTypeConst.enhanced);
			//	}

			//	// Create temporary enhanced Text field
			//	UILabel labEnhancedText = CloneUtil.CloneLabel (labText);

			//	labEnhancedText.Alpha = 1.0f;
			//	labEnhancedText.AttributedText = attributedString;

			//	UIView uiView = labText.Superview;
			//	uiView.AddSubview (labEnhancedText);

			//	lastText = newText;

			//	FadeEnhancedText (labText, labEnhancedText, newText, type);
			//}
			//else
			//{
			//	labText.Alpha = 1.0f;
			//	labText.Text = newText;
			//	labText.AccessibilityLabel = newText;
			//}

			return lastText;
		}

		private static List<string> GetWordDiffFor(string inputNewText, string inputOldText)
		{
			IEnumerable<string> set1 = inputOldText.Split(' ').Distinct();
			IEnumerable<string> set2 = inputNewText.Split(' ').Distinct();

			List<string> diff = set2.Except(set1).ToList();

			return diff;
		}

		private static void FadeEnhancedText(UILabel labText, UILabel labEnhancedText, string newText, Type type)
		{
			if(type == Type.Position)
				positionAnimationInProgress = true;
			else if (type == Type.Address)
				addressAnimationInProgress = true;

            StopCheckTimer();

			double delayInterval = 1000 * 1; // 1 Second

			checkTimer = new Timer (delayInterval);

			if (type == Type.Position)
    			checkTimer.Elapsed += OnTimerPositionElapsed;
			else if (type == Type.Address)
				checkTimer.Elapsed += OnTimerAddressElapsed;
			
			checkTimer.Start ();

			// Fading in text and out enhanced text 
			labEnhancedText.Alpha = 1.0f;
			labText.Alpha = 0.0f;
			labText.Text = newText;
			labText.AccessibilityLabel = newText;

			UIView.Animate (1.0f, 0, UIViewAnimationOptions.CurveEaseIn, () =>
			{
				// To state
				labText.Alpha = 1.0f;
				labEnhancedText.Alpha = 0.0f;
			},
				() =>
			{
				// After effect
				labText.Alpha = 1.0f;
				labEnhancedText.Alpha = 0.0f;

				labEnhancedText.RemoveFromSuperview();
			});
		}

        static void StopCheckTimer()
        {
            if (checkTimer != null)
            {
				checkTimer.Stop();
				checkTimer = null;
            }
        }

		static void OnTimerPositionElapsed (object sender, ElapsedEventArgs e)
		{
            StopCheckTimer();
			positionAnimationInProgress = false;
		}

		static void OnTimerAddressElapsed(object sender, ElapsedEventArgs e)
        {
            StopCheckTimer();
            addressAnimationInProgress = false;
        }
	}

	public class FlashTextObject
	{
		public string lastText { get; set; }
		public string newText { get; set; }
		public UILabel labText { get; set; }

		public FlashTextObject (string _lastText, string _newText, UILabel _labText)
		{
			lastText = _lastText;
			newText = _newText;
			labText = _labText;
		}
	}
}
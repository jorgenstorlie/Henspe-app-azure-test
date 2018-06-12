using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Android.Graphics;
using Android.Views.Animations;
using Android.Content;
using Android.App;

namespace Henspe.Droid.Util
{
    public class FlashTextUtil
    {
		public enum Type
		{
			LatText,
            LonText,
            AddressText,
            Accuracy
		}

		static private bool animationInProgressLat = false;
		static private bool animationInProgressLon = false;
		static private bool animationInProgressAddress = false;
		static private bool animationInProgressAccuracy = false;

		public FlashTextUtil()
        {
        }

		static public string FlashChangedText(Activity activity, Context context, string lastText, string newText, TextView textView, Type type)
        {
            if (textView == null)
				return lastText;

            if (newText == null || (newText != null && newText.Length == 0))
            {
				return lastText;
            }

            /*
			if(progressId == 1 && animationInProgress1 == true)
			{
				return newText;
			}
			else if (progressId == 2 && animationInProgress2 == true)
            {
                return newText;
            }
*/
			if (lastText == newText || lastText.Length == 0)
            {
				/*
				activity.RunOnUiThread(() =>
                {
					textView.Alpha = 0.0f; // Invisible
					//SpannableString spannableText = new SpannableString(newText);
                    //textView.TextFormatted = spannableText;
                });
                */

				return newText;
            }
            
			if (lastText.Length > 0 && lastText != newText && IsAnimationInProgress(type) == false)
            {
                List<string> diff = GetWordDiffFor(newText, lastText);

                foreach (string word in diff)
                {
                    int pos = newText.IndexOf(word);
					SetRedTextInString(activity, textView, newText, pos, word.Length);
                }

				activity.RunOnUiThread(() =>
                {
                    textView.Alpha = 1.0f; // Totally visible
                });

                lastText = newText;

				FadeEnhancedText(activity, context, textView, newText, type);
            }
            /*else
            {
				return newText;
				activity.RunOnUiThread(() =>
                {
					textView.Alpha = 0.0f; // Invisible
					SpannableString spannableText = new SpannableString(newText);
					textView.TextFormatted = spannableText;
                });
            }
	*/

			return lastText;
        }

		private static bool IsAnimationInProgress(Type type)
        {
			if ((type == Type.LatText && animationInProgressLat) ||
			    (type == Type.LonText && animationInProgressLon) ||
			    (type == Type.Accuracy && animationInProgressAccuracy) || 
			    (type == Type.AddressText && animationInProgressAddress))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        private static List<string> GetWordDiffFor(string inputNewText, string inputOldText)
        {
            IEnumerable<string> set1 = inputOldText.Split(' ').Distinct();
            IEnumerable<string> set2 = inputNewText.Split(' ').Distinct();

            List<string> diff = set2.Except(set1).ToList();

            return diff;
        }

		private static void FadeEnhancedText(Activity activity, Context context, TextView textView, string newText, Type type)
        {
			activity.RunOnUiThread(() =>
            {
				if (type == Type.LatText)
                    animationInProgressLat = true;
                else if (type == Type.LonText)
                    animationInProgressLon = true;
                else if (type == Type.Accuracy)
                    animationInProgressAccuracy = true;

                textView.Alpha = 1.0f;

				Animation hideAnimation = AnimationUtils.LoadAnimation(context, Resource.Animator.abc_fade_out);
                hideAnimation.Duration = 1000; // 1 second
                hideAnimation.AnimationEnd += delegate
                {
                    textView.Alpha = 0.0f;

					if (type == Type.LatText)
                        animationInProgressLat = false;
                    else if (type == Type.LonText)
                        animationInProgressLon = false;
					else if (type == Type.AddressText)
						animationInProgressAddress = false;
                    else if (type == Type.Accuracy)
                        animationInProgressAccuracy = false;
                };

                textView.StartAnimation(hideAnimation);
            });
        }

		private static void SetRedTextInString(Activity activity, TextView textView, string inputString, int fromPos, int len)
        {
			activity.RunOnUiThread(() =>
            {
				SpannableString spannableText = new SpannableString(inputString);

				spannableText.SetSpan(new ForegroundColorSpan(Color.Red), fromPos, fromPos + len, SpanTypes.ExclusiveExclusive);

                textView.TextFormatted = spannableText;
            });
        }
    }
}
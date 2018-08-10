using System;
using System.Text.RegularExpressions;

namespace Henspe.Core.Util
{
	public class StringUtil
	{
		public StringUtil ()
		{
		}

		static public string FindStringBetween (string stringValue, string start, string end)
		{
			int startPos = stringValue.IndexOf (start) + start.Length;

			if (startPos < 0) {
				return "";
			}

			int endPos = stringValue.IndexOf (end, startPos + 1);

			if (endPos < 0) {
				return "";
			}

			return stringValue.Substring (startPos, endPos - startPos).Trim();
		}

		static public string FindStringAfter (string stringValue, string after)
		{
			int startPos = stringValue.IndexOf (after) + after.Length;

			if (startPos < 0) {
				return "";
			}

			int endPos = stringValue.Length;

			return stringValue.Substring (startPos, endPos - startPos).Trim();
		}

		static public string FindStringBefore (string stringValue, string before)
		{
			if (stringValue == null || stringValue.Length == 0)
				return "" ;

			int startPos = 0;

			int endPos = stringValue.IndexOf (before, startPos + 1);

			if (endPos < 0) {
				return "";
			}

			return stringValue.Substring (startPos, endPos - startPos).Trim();
		}

		static public string TrimString (string stringValue)
		{
			return stringValue.Trim ();
		}

		static public string DecodeHtml (string data)
		{
			data = data.Replace("&quot;", "\"");
			data = data.Replace("&quot;", "\'");
			data = data.Replace("&lt;", "<");
			data = data.Replace("&gt;", ">");
			data = data.Replace("\n", " ");
			return data;
		}

		static public string FlattenHtml (string htmlString)
		{
			Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
			return reg.Replace(htmlString, "");
		}

		static public bool StringContains (string stringValue, string contains)
		{
			if (stringValue == null || contains == null) {
				return false;
			}

			stringValue = stringValue.ToLower ();
			contains = contains.ToLower ();

			int startPos = stringValue.IndexOf (contains);

			if (startPos < 0) {
				return false;
			} else {
				return true;
			}
		}

		static public bool StringStartsWith (string stringValue, string startsWith)
		{
			if (stringValue == null || startsWith == null) {
				return false;
			}

			stringValue = stringValue.ToLower ();
			startsWith = startsWith.ToLower ();

			int startPos = stringValue.IndexOf (startsWith);

			if (startPos < 0 || startPos > 0) {
				return false;
			} else {
				return true;
			}
		}

		static public bool StringEndsWith (string stringValue, string endsWith)
		{
			if (stringValue == null || endsWith == null) {
				return false;
			}

			stringValue = stringValue.ToLower ();
			endsWith = endsWith.ToLower ();

			int startPos = stringValue.LastIndexOf (endsWith);

			int startPosShouldBe = stringValue.Length - endsWith.Length;

			if (startPos < startPosShouldBe || startPos > startPosShouldBe) {
				return false;
			} else {
				return true;
			}
		}

		static public string FindDigitsInString(string inputString)
		{
			string result = string.Empty;

			for (int i=0; i< inputString.Length; i++)
			{
				if (Char.IsDigit (inputString [i])) 
				{
					result += inputString [i];
				}
			}

			return result;
		}

		static public string FindFirstNumberOfCharacters(int numberOfCharacters, string stringValue)
		{
			int startPos = 0;

			if (startPos < 0) {
				return "";
			}

			int endPos = numberOfCharacters;

			if (endPos < 0) {
				return "";
			}

			return stringValue.Substring (startPos, endPos - startPos).Trim();
		}

		static public string RemoveFirstNumberOfCharacters(int numberOfCharacters, string stringValue)
		{
			int startPos = numberOfCharacters;

			if (startPos < 0) {
				return "";
			}

			int endPos = stringValue.Length;

			if (endPos < 0) {
				return "";
			}

			return stringValue.Substring (startPos, endPos - startPos).Trim();
		}

		static public string DecodeUnicodeString(string unicodeString)
		{
			return System.Text.RegularExpressions.Regex.Unescape(unicodeString);
		}

		static public string RemoveWhitespace(string stringValue)
		{
			if (stringValue == null)
				return null;

			return stringValue.Replace(" ", string.Empty);
		}

		static public string RemoveStringFromString(string stringValue, string stringToRemove)
		{
			if (stringValue == null)
				return null;

			return stringValue.Replace(stringToRemove, string.Empty);
		}

		static public string ReplaceStringInStringWithString(string stringValue, string stringToRemove, string stringToReplace)
		{
			if (stringValue == null)
				return null;

			return stringValue.Replace(stringToRemove, stringToReplace);
		}
	}
}


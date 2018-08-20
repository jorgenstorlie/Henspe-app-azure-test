using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Foundation;

namespace Henspe.iOS.Util
{
	public class StoreUtil
	{
		public StoreUtil ()
		{
		}

		static public void saveString(string stringValue, string key)
		{
			if (stringValue == null) {
				stringValue = "";
			}

			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			prefs.SetString (stringValue, key);
			prefs.Synchronize ();
		}

		static public string loadStringForKey(string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			string value = prefs.StringForKey (key);
			return value;
		}

		static public void saveInt(int intValue, string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			prefs.SetInt (intValue, key);
			prefs.Synchronize ();
		}

		static public int loadIntForKey(string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			int value = (int)prefs.IntForKey (key);
			return value;
		}

		static public void saveDouble(double value, string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			prefs.SetDouble (value, key);
			prefs.Synchronize ();
		}

		static public double loadDoubleForKey(string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			double value = prefs.DoubleForKey (key);
			return value;
		}

		static public void saveBool(bool boolValue, string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			prefs.SetBool (boolValue, key);
			prefs.Synchronize ();
		}

		static public bool loadBoolForKey(string key)
		{
			NSUserDefaults prefs = NSUserDefaults.StandardUserDefaults;
			bool value = prefs.BoolForKey (key);
			return value;
		}
	}
}
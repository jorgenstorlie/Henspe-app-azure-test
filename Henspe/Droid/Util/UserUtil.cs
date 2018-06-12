using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Henspe.Core.Communication;

namespace Henspe.Droid.Utils
{
    public class UserUtil
    {
		private const string PrefInstructionsFinished = "is_instructions_finished";

		private const string PreferenceFile = "settings";

		public static ISettings settings = new AndroidSettings();

		public class AndroidSettings : ISettings
		{
			public bool instructionsFinished
			{
				get
				{
					ISharedPreferences sharedPref = Henspe.Current.ApplicationContext.GetSharedPreferences(PreferenceFile, FileCreationMode.Private);
					return sharedPref.GetBoolean(PrefInstructionsFinished, false);
				}

				set
				{
					ISharedPreferences sharedPref = Henspe.Current.ApplicationContext.GetSharedPreferences(PreferenceFile, FileCreationMode.Private);
					sharedPref.Edit().PutBoolean(PrefInstructionsFinished, value).Commit();
				}
			}
		}

		public static void Reset()
		{
			settings.instructionsFinished = false;
		}
    }
}
using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Henspe.Core.Communication;

namespace Henspe.Droid.Utils
{
    public class UserUtil
    {
		public interface IPrefs
		{
			bool instructionsFinished { get; set; }
		}

		private const string PrefInstructionsFinished = "is_instructions_finished";

		private const string PreferenceFile = "settings";

		public static IPrefs settings = new AndroidSettings();

		public class AndroidSettings : IPrefs
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
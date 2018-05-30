using Henspe.Core.Communication;

namespace Henspe.iOS.Util
{
	public class UserUtil
	{
		private const string PrefInstructionsFinished = "is_instructions_finished";

		public static ISettings settings = new iOSSettings();

		public class iOSSettings:ISettings
		{
			public bool instructionsFinished
			{ 	get {return StoreUtil.loadBoolForKey (PrefInstructionsFinished);}
				set {StoreUtil.saveBool (value, PrefInstructionsFinished);}
			}
		}

		public static void Reset()
		{
			settings.instructionsFinished = false;
		}
	}
}
using Henspe.Core.Communication;

namespace Henspe.iOS.Util
{
	public class UserUtil
	{
		private const string PrefInstructionsFinished = "is_instructions_finished";

		public static ICredentials credentials = new iOSCredentials();

		public class iOSCredentials:ICredentials
		{
			public bool instructionsFinished
			{ 	get {return StoreUtil.loadBoolForKey (PrefInstructionsFinished);}
				set {StoreUtil.saveBool (value, PrefInstructionsFinished);}
			}
		}

		public static void Reset()
		{
			credentials.instructionsFinished = false;
		}
	}
}
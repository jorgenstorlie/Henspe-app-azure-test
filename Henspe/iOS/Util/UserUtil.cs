using Henspe.Core.Communication;

namespace Henspe.iOS.Util
{
	public class UserUtil
	{
		private const string PrefIsAuthenticated = "is_authenticated";
		private const string PrefKey = "key";
		private const string PrefMobilnr = "mobilnr";
		private const string PrefMail = "mail";
		private const string PrefNavn = "navn";
		private const string PrefAvdelings_id = "avdelings_id";
		private const string PrefAvdeling = "avdeling";
		private const string PrefStillings_id = "stillings_id";
		private const string PrefStilling = "stilling";
		private const string PrefPin_kode = "pin_kode";
		private const string PrefServer_dato = "server_dato";
		private const string PrefServer_kl = "server_kl";
		private const string PrefServerLocalMillisecDiff = "server_local_millisec_diff";

		public static ICredentials Credentials = new iOSCredentials();

		public class iOSCredentials:ICredentials
		{
			public bool IsAuthenticated
			{ 	get {return StoreUtil.loadBoolForKey (PrefIsAuthenticated);}
				set {StoreUtil.saveBool (value, PrefIsAuthenticated);}
			}

			public int Key
			{ 	get {return StoreUtil.loadIntForKey (PrefKey);}
				set {StoreUtil.saveInt (value, PrefKey);}
			}

			public string Mobilnr
			{ 	get {return StoreUtil.loadStringForKey (PrefMobilnr);}
				set {StoreUtil.saveString (value, PrefMobilnr);}
			}

			public string Mail
			{ 	get {return StoreUtil.loadStringForKey (PrefMail);}
				set {StoreUtil.saveString (value, PrefMail);}
			}

			public string Navn
			{ 	get {return StoreUtil.loadStringForKey (PrefNavn);}
				set {StoreUtil.saveString (value, PrefNavn);}
			}

			public int Avdelings_id
			{ 	get {return StoreUtil.loadIntForKey (PrefAvdelings_id);}
				set {StoreUtil.saveInt (value, PrefAvdelings_id);}
			}

			public string Avdeling
			{ 	get {return StoreUtil.loadStringForKey (PrefAvdeling);}
				set {StoreUtil.saveString (value, PrefAvdeling);}
			}

			public int Stillings_id
			{ 	get {return StoreUtil.loadIntForKey (PrefStillings_id);}
				set {StoreUtil.saveInt (value, PrefStillings_id);}
			}

			public string Stilling
			{ 	get {return StoreUtil.loadStringForKey (PrefStilling);}
				set {StoreUtil.saveString (value, PrefStilling);}
			}

			public int Pin_kode
			{ 	get {return StoreUtil.loadIntForKey (PrefPin_kode);}
				set {StoreUtil.saveInt (value, PrefPin_kode);}
			}

			public double Server_dato
			{ 	get {return StoreUtil.loadDoubleForKey (PrefServer_dato);}
				set {StoreUtil.saveDouble (value, PrefServer_dato);}
			}

			public int Server_kl
			{ 	get {return StoreUtil.loadIntForKey (PrefServer_kl);}
				set {StoreUtil.saveInt (value, PrefServer_kl);}
			}

			public double Server_Local_Millisec_Diff
			{ 	get {return StoreUtil.loadDoubleForKey (PrefServerLocalMillisecDiff);}
				set {StoreUtil.saveDouble (value, PrefServerLocalMillisecDiff);}
			}

			public bool AccessToPhone
			{ 
				get 
				{
					if (DeviceUtil.isIpad () == false)
						return true;
					else
						return false;
				} 
			}
		}

		public static void Reset()
		{
			Credentials.IsAuthenticated = false;
			Credentials.Key = 0;
			Credentials.Mobilnr = "";
			Credentials.Mail = "";
			Credentials.Navn = "";
			Credentials.Avdelings_id = 0;
			Credentials.Avdeling = "";
			Credentials.Stillings_id = 0;
			Credentials.Stilling = "";
			Credentials.Pin_kode = 0;
			Credentials.Server_dato = 0;
			Credentials.Server_kl = 0;
			Credentials.Server_Local_Millisec_Diff = 0;
		}
	}
}
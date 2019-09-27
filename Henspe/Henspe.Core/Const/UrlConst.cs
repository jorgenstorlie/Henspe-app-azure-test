
namespace SNLA.Core.Const
{
	public class UrlConst
	{
        public static string[] vianettUsernameSecret = { "h", "j", "e", "l", "p", "1", "1", "3" };
        public static string[] vianettPasswordSecret = { "w", "A", "r", "C", "r", "7", "e", "B", "J", "H" };

        public const string NLA113Base = "https://ws.snla-it.no/api/h113/";
        public const string NLABase = "https://ws.snla-it.no/api/mob_app/";
        public const string NRDBBase = "https://liag.test.systor.st/userdata?"; // https://liag.nrdb.no/userdata?
        public const string NLAMobilAuth = "https://ws.snla-it.no/api/h113/h113_mobaut.php?";
        public const string VianettBase = "https://smsc.vianett.no/v3/send";

		/// <summary>
		/// The base URL. Required by SNLAHttpClient
		/// </summary>
		public const string BaseUrl = "https://ws.snla-it.no/api/mob_app/";
        public const string BaseTestUrl = "https://wstest.snla-it.no/api/mob_app/";

        public const string NLAInfoPage = "https://norskluftambulanse.no/nrdb/";

        public const string NLARegEmailSMSUrl = NLA113Base + "h113_register.php";
        public const string NRDBSendPositionUrl = NRDBBase + "op=sendPosition";
        public const string NRDBEndOfDataUrl = NRDBBase + "op=endOfData";
        public const string NLAPinRequest = NLAMobilAuth + "action=aut";
        public const string VianettUrl = VianettBase + "?"; // username=hjelp113&password=wArCr7eBJH&tel=4711111111&msg=Hello+World&campaignid=username(hjelp113)

        public static string[] NLAMobAutApiKey = { "0", "e", "a", "7", "a", "b", "2", "f", "5", "3", "8", "2", "7", "8", "0", "f", "7", "b", "c", "6", "4", "f", "e", "6", "4", "e", "c", "f", "a", "1", "c", "9" };
      
        public UrlConst ()
		{
		}
	}
}
using System;

namespace Henspe.Core.Const
{
	public class UrlConst
	{
        public static string[] hjertestarerClientId = { "T", "k", "B", "f", "R", "u", "B", "P", "R", "f", "n", "k", "N", "f", "G", "z", "o", "q", "2", "2", "9", "Q", ".", "." };
        public static string[] hjertestarerClientSecret = { "V", "H", "g", "U", "6", "N", "K", "7", "F", "w", "A", "k", "_", "S", "U", "N", "3", "x", "Y", "Y", "-", "A", ".", "." };
        public static string[] vianettUsernameSecret = { "h", "j", "e", "l", "p", "1", "1", "3" };
        public static string[] vianettPasswordSecret = { "w", "A", "r", "C", "r", "7", "e", "B", "J", "H" };

        public const string NLA113Base = "https://ws.snla-it.no/api/h113/";
        public const string NLABase = "https://ws.snla-it.no/api/mob_app/";
        public const string NRDBBase = "https://liag.test.systor.st/userdata?"; // https://liag.nrdb.no/userdata?
        public const string NLAMobilAuth = "https://ws.snla-it.no/api/h113/h113_mobaut.php?";
        public const string VianettBase = "https://smsc.vianett.no/v3/send";

        public const string NLAInfoPage = "https://norskluftambulanse.no/nrdb/";

        //public const string NLAProxyBase1 = "https://ws1.Hjelp113gps.no/";
        //public const string NLAProxyBase2 = "https://ws2.Hjelp113gps.no/";
        //public const string NLAProxyBase3 = "https://ws3.Hjelp113gps.no/";

        public const string NLAProxyBase1 = "https://ws1.hjelp113.no/";
        public const string NLAProxyBase2 = "https://ws2.hjelp113.no/";
        public const string NLAProxyBase3 = "https://ws3.hjelp113.no/";
        public const string NLAProxySendNRDB = "sendNRDB.php";

        public const string NLARegEmailSMSUrl = NLA113Base + "h113_register.php";
        public const string NRDBSendPositionUrl = NRDBBase + "op=sendPosition";
        public const string NRDBEndOfDataUrl = NRDBBase + "op=endOfData";
        public const string NLAPinRequest = NLAMobilAuth + "action=aut";
        public const string VianettUrl = VianettBase + "?"; // username=hjelp113&password=wArCr7eBJH&tel=4711111111&msg=Hello+World&campaignid=username(hjelp113)

        public static string[] NLAMobAutApiKey = { "0", "e", "a", "7", "a", "b", "2", "f", "5", "3", "8", "2", "7", "8", "0", "f", "7", "b", "c", "6", "4", "f", "e", "6", "4", "e", "c", "f", "a", "1", "c", "9" };
        public static string[] defibrillatorApiKey = { "0", "e", "a", "7", "a", "b", "2", "f", "5", "3", "8", "2", "7", "8", "0", "f", "7", "b", "c", "6", "4", "f", "e", "6", "4", "e", "c", "f", "a", "1", "c", "9" };
        public const string defibrillatorBase = "https://ws.snla-it.no/api/h113/";
        public const string defibrillatorGet = defibrillatorBase + "h113_hent_hs.php";

        //public const string hjertestarterGetAccessToken = "curl -i --user {0}:{1} --data 'grant_type=client_credentials' https://www.113.no/ords/api/oauth/token ";
        public const string hjertestarterAuthorizeBase = "https://www.113.no/ords/api/";
        public const string hjertestarterAuthorize = "oauth/token";
        public const string hjertestarterBase = "https://www.113.no/ords/api/v1/";
        public const string hjertestarterSearch = "assets/search/";
        public const string hjertestarterGet = "assets/";

        //https://www.113.no/ords/api/v1/assets/search/?max_rows=2

        public UrlConst ()
		{
		}
	}
}
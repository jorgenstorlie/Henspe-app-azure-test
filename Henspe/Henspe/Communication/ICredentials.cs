namespace Henspe.Core.Communication
{
    public interface ICredentials
    {
		bool IsAuthenticated { get; set; }
		int Key { get; set; }
		string Mobilnr { get; set; }
		string Mail { get; set; }
		string Navn { get; set; }
		int Avdelings_id { get; set; }
		string Avdeling { get; set; }
		int Stillings_id { get; set; }
		string Stilling { get; set; }
		int Pin_kode { get; set; }
		double Server_dato { get; set; }
		int Server_kl { get; set; }
		double Server_Local_Millisec_Diff { get; set; }
		bool AccessToPhone { get; }
    }
}

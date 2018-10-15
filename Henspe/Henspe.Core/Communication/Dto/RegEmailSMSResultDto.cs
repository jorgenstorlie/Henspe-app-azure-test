namespace Henspe.Core.Communication.Dto
{
    public class RegEmailSMSResultDto
	{
		public bool success { get; set; }
        public bool resultat { get; set; }
        public bool epost_on { get; set; }
        public bool mob_on { get; set; }
        public string pin { get; set; }
        public string error_message { get; set; }

        public RegEmailSMSResultDto ()
		{
		}
	}
}
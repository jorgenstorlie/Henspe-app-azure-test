
namespace Henspe.Core.Model.Dto
{
	public class BrukerDto
	{
		public string mobilnr { get; set; }
		public string navn { get; set; }
		public string mail { get; set; }
		public int avdelings_id { get; set; }
		public string avdeling { get; set; }
		public int stillings_id { get; set; }
		public string stilling { get; set; }
		public string avsender_sms { get; set; }
		public string avsender_mail { get; set; }
		public int pin_kode { get; set; }
	}
}
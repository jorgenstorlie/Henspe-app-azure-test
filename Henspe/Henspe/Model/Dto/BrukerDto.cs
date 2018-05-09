using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Henspe.Core.Model.Dto
{
	public class BrukerDto
	{
		[JsonProperty("id")]
		public int key { get; set; }
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

		public BrukerDto ()
		{
		}
	}
}
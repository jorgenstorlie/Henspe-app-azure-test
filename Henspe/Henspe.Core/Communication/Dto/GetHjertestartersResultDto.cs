using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Henspe.Core.Model;
using static Henspe.Core.Communication.HjertestarterService;

namespace Henspe.Core.Communication.Dto
{
    public class GetHjertestartersResultDto
	{
		public bool success { get; set; }
        public string siste_synk_id { get; set; }
        public bool defibrillatorListStored { get; set; }
        public IEnumerable<Hjertestarter> hjerte { get; set; }
        public IEnumerable<double> slettes { get; set; }
        public string error_message { get; set; }

        public GetHjertestartersResultDto ()
		{
		}
	}
}
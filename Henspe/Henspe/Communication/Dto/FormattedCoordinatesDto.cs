using System;
using System.Collections.Generic;

namespace Henspe.Core.Communication.Dto
{
    public class FormattedCoordinatesDto
	{
        public bool success { get; set; }
        public string error { get; set; }
        public string latitudeDescription { get; set; }
        public string longitudeDescription { get; set; }

        public FormattedCoordinatesDto ()
		{
		}
	}
}
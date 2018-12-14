using System;
using System.Collections.Generic;
using Henspe.Core.Model;

namespace Henspe.Core.Communication.Dto
{
    public enum State
    {
        noAction,
        removeAll,
        cleanup,
        replace
    }

    public class HjertestartersInBoxIfNeededResultDto
    {
        public State state { get; set; }
        public IEnumerable<Hjertestarter> hjertestarters { get; set; }
        public double resultNorthEastLat { get; set; }
        public double resultNorthEastLong { get; set; }
        public double resultSouthWestLat { get; set; }
        public double resultSouthWestLong { get; set; }

        public HjertestartersInBoxIfNeededResultDto()
        {
        }
    }
}
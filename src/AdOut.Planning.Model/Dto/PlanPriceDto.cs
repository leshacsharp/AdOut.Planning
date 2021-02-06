using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanPriceDto
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<AdPointDto> AdPoints { get; set; }
    }
}

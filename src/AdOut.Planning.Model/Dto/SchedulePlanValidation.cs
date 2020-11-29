using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class SchedulePlanValidation
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<AdPointValidation> AdPoints { get; set; }
    }
}

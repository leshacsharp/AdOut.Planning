using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class ScheduleValidation
    {
        public DateTime PlanStartDateTime { get; set; }

        public DateTime PlanEndDateTime { get; set; }

        public IEnumerable<AdPointValidation> AdPoints { get; set; }
    }
}

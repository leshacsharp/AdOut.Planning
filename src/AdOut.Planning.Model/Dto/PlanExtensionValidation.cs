using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanExtensionValidation
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<AdPointValidation> AdPoints { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}

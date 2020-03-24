using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Api
{
    public class PlanValidationModel
    {
        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public TimeSpan AdsTimePlaying { get; set; }

        public List<ScheduleModel> Schedules { get; set; }
    }
}

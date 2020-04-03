using AdOut.Planning.Model.Enum;
using System;

namespace AdOut.Planning.Model.Dto
{
    public class SchedulePlan
    {
        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    //todo: replace to PlanExistingValidation
    public class PlanValidation
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }

        public IEnumerable<DayOfWeek> AdPointsDaysOff { get; set; }
    }

    //public class PlanValidation
    //{
    //    public PlanType Type { get; set; }

    //    public DateTime StartDateTime { get; set; }

    //    public DateTime EndDateTime { get; set; }

    //    public IEnumerable<ScheduleDto> Schedules { get; set; }
    //}
}

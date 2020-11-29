using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanTimeLine
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; } 

        public IEnumerable<ScheduleDto> Schedules { get; set; }

        public IEnumerable<DayOfWeek> AdPointsDaysOff { get; set; }
    }
}

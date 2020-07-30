using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointValidation
    {
        public string Location { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        //public IEnumerable<PlanValidation> Plans { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }

        public IEnumerable<DayOfWeek> DaysOff { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointPlanDto
    {
        public int Id { get; set; }

        public TimeSpan AdsTimePlaying { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}

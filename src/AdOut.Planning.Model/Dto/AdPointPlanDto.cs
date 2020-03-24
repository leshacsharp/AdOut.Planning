using AdOut.Planning.Model.Api;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointPlanDto
    {
        public int Id { get; set; }

        public TimeSpan AdsTimePlaying { get; set; }

        public IEnumerable<ScheduleModel> Schedules { get; set; }
    }
}

using AdOut.Planning.Model.Database;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanTimeDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<AdPlanTime> Ads { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }

        public IEnumerable<AdPointPlanTimeDto> AdPoints { get; set; }
    }
}

using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class StreamPlanTime
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<AdPlanTime> Ads { get; set; }

        public IEnumerable<SchedulePeriod> Schedules { get; set; }
    }
}

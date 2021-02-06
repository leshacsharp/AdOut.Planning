using AdOut.Extensions.Authorization;
using System;

namespace AdOut.Planning.Model.Api
{
    public class UpdateScheduleModel
    {
        [ResourceId]
        public string ScheduleId { get; set; }

        //todo: need to try this field from Api model
        public string PlanId { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan BreakTime { get; set; }

        public TimeSpan PlayTime { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public DateTime? Date { get; set; }
    }
}

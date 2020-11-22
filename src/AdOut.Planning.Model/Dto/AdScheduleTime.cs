using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdScheduleTime
    {
        public PlanType PlanType { get; set; }

        public DateTime PlanStartDateTime { get; set; }

        public DateTime PlanEndDateTime { get; set; }

        public TimeSpan AdPlayTime { get; set; }

        public TimeSpan AdBreakTime { get; set; }

        public TimeSpan ScheduleStartTime { get; set; }

        public TimeSpan ScheduleEndTime { get; set; }

        public DayOfWeek? ScheduleDayOfWeek { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public IEnumerable<DayOfWeek> AdPointsDaysOff { get; set; }
    }
}

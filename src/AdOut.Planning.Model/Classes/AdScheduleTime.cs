using System;

namespace AdOut.Planning.Model.Classes
{
    //add days off property for dayli time helper
    public class AdScheduleTime
    {
        public DateTime PlanStartDateTime { get; set; }

        public DateTime PlanEndDateTime { get; set; }

        public TimeSpan AdPlayTime { get; set; }

        public TimeSpan AdBreakTime { get; set; }

        public TimeSpan ScheduleStartTime { get; set; }

        public TimeSpan ScheduleEndTime { get; set; }

        public DayOfWeek? ScheduleDayOfWeek { get; set; }

        public DateTime? ScheduleDate { get; set; }
    }
}

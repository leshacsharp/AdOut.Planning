using System;

namespace AdOut.Planning.Model.Api
{
    public class ScheduleModel
    {
        public int[] AdPointIds { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan BreakTime { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public DateTime? Date { get; set; }
    }
}

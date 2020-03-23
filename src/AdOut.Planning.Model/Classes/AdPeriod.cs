using System;

namespace AdOut.Planning.Model.Classes
{
    public class AdPeriod
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime? Date { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }
    }
}

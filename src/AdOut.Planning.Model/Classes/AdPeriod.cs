using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class AdPeriod
    {
        //public TimeSpan StartTime { get; set; }

        //public TimeSpan EndTime { get; set; }

        //public DateTime? Date { get; set; }

        //public DayOfWeek? DayOfWeek { get; set; }



        public List<TimeRange> TimeRanges { get; set; }

        public List<DateTime> Dates { get; set; } 
    }
}

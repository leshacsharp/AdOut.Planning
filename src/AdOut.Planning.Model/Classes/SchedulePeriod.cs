using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class SchedulePeriod
    {
        public List<TimeRange> TimeRanges { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public List<DateTime> Dates { get; set; } 
    }
}

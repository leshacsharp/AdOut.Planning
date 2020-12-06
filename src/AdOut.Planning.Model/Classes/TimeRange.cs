using System;

namespace AdOut.Planning.Model.Classes
{
    public class TimeRange
    {
        public TimeRange(TimeSpan start, TimeSpan end)
        {
            Start = start;
            End = end;
        }

        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public bool IsInterescted(TimeRange timeRange)
        {
           return this.Start < timeRange.End && timeRange.Start < this.End;
        }
    }
}

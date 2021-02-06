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

        public bool IsInterescted(TimeSpan start, TimeSpan end)
        {
            return this.Start < end && start < this.End;
        }

        public bool IsLeftIntersected(TimeSpan start, TimeSpan end)
        {
            return start < this.Start && end > this.Start && end < this.End;
        }

        public bool IsRightIntersected(TimeSpan start, TimeSpan end)
        {
            return end > this.End && start < this.End && start > this.Start; 
        }
    }
}

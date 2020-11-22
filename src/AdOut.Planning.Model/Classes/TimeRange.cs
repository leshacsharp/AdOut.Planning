using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //todo: create methods for time interesactions as in the schedule validators
        public bool IsInterescted(TimeRange timeRange)
        {
            var left = timeRange.Start <= this.Start && timeRange.End >= this.Start;
            var right = timeRange.Start <= this.End && timeRange.End >= this.End;   
            var inner = timeRange.Start >= this.Start && timeRange.End <= this.End;

            return left || right || inner;
        }
    }
}

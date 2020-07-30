using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class AdPointTime
    {
        public string Location { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        public List<DayOfWeek> DaysOff { get; set; }

        public List<AdPeriod> AdPeriods { get; set; }

        public AdPointTime()
        {
            DaysOff = new List<DayOfWeek>();
            AdPeriods = new List<AdPeriod>();
        }
    }
}

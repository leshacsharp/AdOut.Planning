using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointPlanTimeDto
    {
        public string Id { get; set; }
        public IEnumerable<DayOfWeek> DaysOff { get; set; }
    }
}

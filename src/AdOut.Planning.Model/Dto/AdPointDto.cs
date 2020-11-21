using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointDto
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public IEnumerable<DayOfWeek> DaysOff { get; set; }
    }
}

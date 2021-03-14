using AdOut.Planning.Model.Classes;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanPeriod
    {
        public string PlanId { get; set; }
        public IEnumerable<SchedulePeriod> SchedulePeriods { get; set; }
    }
}

using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class PlanPeriod
    {
        public string PlanId { get; set; }

        public string PlanTitle { get; set; }

        public List<SchedulePeriod> SchedulePeriods { get; set; }

        public PlanPeriod()
        {
            SchedulePeriods = new List<SchedulePeriod>();
        }
    }
}

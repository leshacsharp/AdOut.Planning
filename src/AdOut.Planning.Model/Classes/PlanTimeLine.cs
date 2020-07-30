using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class PlanTimeLine
    {
        public int PlanId { get; set; }

        public string PlanTitle { get; set; }

        public List<AdPeriod> AdsPeriods { get; set; }

        public PlanTimeLine()
        {
            AdsPeriods = new List<AdPeriod>();
        }
    }
}

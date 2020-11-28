using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    //todo: think about name of class
    public class TempPlanValidation
    {
        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<AdPointValidation> AdPoints { get; set; }
    }
}

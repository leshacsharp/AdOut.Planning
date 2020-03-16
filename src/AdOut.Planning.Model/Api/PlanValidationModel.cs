using AdOut.Planning.Model.Enum;
using System;

namespace AdOut.Planning.Model.Api
{
    public class PlanValidationModel
    {
        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}

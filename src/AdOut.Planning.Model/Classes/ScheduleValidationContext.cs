using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class ScheduleValidationContext
    {
        public PlanValidationModel Plan { get; set; }

        public ScheduleValidation Schedule { get; set; }

        public List<AdPointValidation> AdPointValidations { get; set; }

        public List<AdPeriod> AdsPeriods { get; set; }

        public List<string> Errors { get; set; }

        public ScheduleValidationContext()
        {
            Errors = new List<string>();
        }
    }
}

using AdOut.Planning.Model.Dto;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class ScheduleValidationContext
    {
        public ScheduleDto Schedule { get; set; }

        public SchedulePlan Plan { get; set; }

        public List<AdPointValidation> AdPointsValidations { get; set; }

        public List<AdPeriod> NewAdsPeriods { get; set; }

        public List<AdPeriod> ExistingAdsPeriods { get; set; }

        public List<string> Errors { get; set; }

        public ScheduleValidationContext()
        {
            Errors = new List<string>();
        }
    }
}

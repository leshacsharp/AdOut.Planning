using AdOut.Planning.Model.Dto;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    //todo: need to think about steps of creating plans and schedules (order)
    public class ScheduleValidationContext
    {
        public ScheduleDto Schedule { get; set; }

        public SchedulePlan Plan { get; set; }

        public List<AdPointTime> AdPoints { get; set; }

        public List<AdPeriod> NewAdsPeriods { get; set; }

        //public List<AdPointValidation> AdPointsValidations { get; set; }

        //public List<AdPeriod> ExistingAdsPeriods { get; set; }

        public List<string> Errors { get; set; }

        public ScheduleValidationContext()
        {
            Errors = new List<string>();
        }
    }
}

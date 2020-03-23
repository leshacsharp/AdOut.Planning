using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.Model.Api
{
    public class ScheduleWithPlanValidationModel
    {
        public int[] AdPointIds { get; set; }

        public PlanValidationModel TempPlan { get; set; }

        public ScheduleValidation Schedule { get; set; }
    }
}

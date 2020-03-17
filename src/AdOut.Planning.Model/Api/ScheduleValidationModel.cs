using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.Model.Api
{
    public class ScheduleValidationModel
    {
        public int[] AdPointIds { get; set; }

        public PlanValidationModel Plan { get; set; }

        public ScheduleValidation Schedule { get; set; }
    }  
}

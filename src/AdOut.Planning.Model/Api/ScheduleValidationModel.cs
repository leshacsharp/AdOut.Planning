using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.Model.Api
{
    public class ScheduleValidationModel
    {
        public int PlanId { get; set; }

        public ScheduleValidation Schedule { get; set; }
    }  
}

using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.Model.Api
{
    public class TempScheduleValidationModel
    {
        public int[] AdPointIds { get; set; }

        public PlanValidation Plan { get; set; }

        public ScheduleDto Schedule { get; set; }
    }
}

namespace AdOut.Planning.Model.Api
{
    public class ScheduleWithPlanValidationModel
    {
        public int[] AdPointIds { get; set; }

        public PlanValidationModel TempPlan { get; set; }

        public ScheduleModel Schedule { get; set; }
    }
}

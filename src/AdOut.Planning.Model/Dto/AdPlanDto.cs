using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Dto
{
    public class AdPlanDto
    {
        public string Id { get; set; }

        public PlanStatus Status { get; set; }

        public string Title { get; set; }
    }
}

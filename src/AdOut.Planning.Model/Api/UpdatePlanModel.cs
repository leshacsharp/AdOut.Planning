using AdOut.Extensions.Authorization;
using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class UpdatePlanModel
    {
        [ResourceId]
        public string PlanId { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 1)]
        public string Title { get; set; }
    }
}

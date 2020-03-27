﻿using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class UpdatePlanModel
    {
        public int PlanId { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 1)]
        public string Title { get; set; }
    }
}

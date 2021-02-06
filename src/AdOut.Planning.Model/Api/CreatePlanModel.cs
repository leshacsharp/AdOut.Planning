using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class CreatePlanModel
    {
        [Required]
        [StringLength(70, MinimumLength = 1)]
        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<string> AdPointsIds { get; set; }
    }
}

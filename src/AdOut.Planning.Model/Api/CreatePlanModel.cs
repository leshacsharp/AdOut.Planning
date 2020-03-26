using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Enum;
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

        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public TimeSpan AdsTimePlaying { get; set; }

        public IEnumerable<int> AdPointsIds { get; set; }

        public IEnumerable<PlanAdModel> PlanAds { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}

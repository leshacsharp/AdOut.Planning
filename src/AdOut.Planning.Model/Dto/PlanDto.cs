using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanDto
    {
        public string Title { get; set; }
        
        public PlanType Type { get; set; }

        public PlanStatus Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public TimeSpan AdsTimePlaying { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }

        public IEnumerable<AdListDto> Ads { get; set; }

        public IEnumerable<AdPointDto> AdPoints { get; set; }
    }
}

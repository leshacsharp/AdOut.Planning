﻿using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanValidation
    {
        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<ScheduleValidation> Schedules { get; set; }

        public IEnumerable<PlanAdValidation> PlanAds { get; set; }
    }
}
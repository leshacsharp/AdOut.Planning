﻿using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanDto
    {
        public PlanType Type { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}

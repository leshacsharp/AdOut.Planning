﻿using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class PlanValidation
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}

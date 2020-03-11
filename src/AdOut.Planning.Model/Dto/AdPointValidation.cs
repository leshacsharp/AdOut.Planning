﻿using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointValidation
    {
        public string Location { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        public IEnumerable<PlanDto> Plans { get; set; }

        public IEnumerable<DayOfWeek> Weekends { get; set; }
    }
}

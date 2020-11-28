﻿using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdPointValidation
    {
        //delete
        //public string Location { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        //public IEnumerable<PlanValidation> Plans { get; set; }

        public IEnumerable<DayOfWeek> DaysOff { get; set; }
    }
}

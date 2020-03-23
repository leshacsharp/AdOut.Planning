﻿using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Dto;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class ScheduleValidationContext
    {
        public ScheduleValidation Schedule { get; set; }

        public List<AdPeriod> ScheduleAdsPeriods { get; set; }

        public PlanValidationModel Plan { get; set; }

        public List<AdPointValidation> AdPointValidations { get; set; }

        public List<AdPeriod> AdPointAdsPeriods { get; set; }

        public List<string> Errors { get; set; }

        public ScheduleValidationContext()
        {
            Errors = new List<string>();
        }
    }
}

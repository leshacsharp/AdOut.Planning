using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    public class ScheduleValidationContext
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan BreakTime { get; set; }

        public List<AdPointValidation> AdPointValidations { get; set; }

        public string Errors { get; set; }
    }
}

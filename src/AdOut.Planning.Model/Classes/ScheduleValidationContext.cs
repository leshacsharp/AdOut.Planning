using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Classes
{
    //todo: need to think about steps of creating plans and schedules (order)
    public class ScheduleValidationContext
    {  
        public TimeSpan AdPlayTime { get; set; }

        public TimeSpan AdBreakTime { get; set; }

        public TimeSpan ScheduleStartTime { get; set; }

        public TimeSpan ScheduleEndTime { get; set; }

        public DayOfWeek? ScheduleDayOfWeek { get; set; }

        public DateTime? ScheduleDate { get; set; }

        public ScheduleType ScheduleType { get; set; }

        public DateTime PlanStartDateTime { get; set; }

        public DateTime PlanEndDateTime { get; set; }

        public List<SchedulePeriod> ExistingSchedulePeriods { get; set; }

        public SchedulePeriod NewSchedulePeriod { get; set; }

        public List<AdPointValidation> AdPoints { get; set; }

        public List<string> Errors { get; set; }

        public ScheduleValidationContext()
        {
            Errors = new List<string>();
        }
    }
}

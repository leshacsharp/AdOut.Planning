using System;

namespace AdOut.Planning.Model.Api
{
    //todo: add annotations
    public class ScheduleValidationModel
    {
        public int[] AdPointIds { get; set; }

        public PlanValidationModel Plan { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan BreakTime { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public DateTime? Date { get; set; }
    }  
}

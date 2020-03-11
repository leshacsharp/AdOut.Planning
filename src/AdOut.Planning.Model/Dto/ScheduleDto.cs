using System;

namespace AdOut.Planning.Model.Dto
{
    public class ScheduleDto
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan BreakTime { get; set; }
    }
}

﻿using AdOut.Planning.Model.Enum;
using System;

namespace AdOut.Planning.Model.Dto
{
    public class ScheduleDto
    {
        public ScheduleType Type { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan BreakTime { get; set; }

        public TimeSpan PlayTime { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public DateTime? Date { get; set; }
    }
}

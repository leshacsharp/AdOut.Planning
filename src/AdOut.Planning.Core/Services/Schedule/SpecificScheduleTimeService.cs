﻿using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Core.Services.Schedule
{
    public class SpecificScheduleTimeService : BaseScheduleTimeService
    {
        protected override List<DateTime> GetPlanWorkingDays(ScheduleTime scheduleTime)
        {
            return new List<DateTime>() { scheduleTime.ScheduleDate.Value.Date };
        }
    }
}

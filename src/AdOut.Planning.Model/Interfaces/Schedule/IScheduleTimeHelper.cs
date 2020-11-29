﻿using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleTimeHelper
    {
        TimeSpan GetTimeOfAdsShowing(ScheduleTime scheduleTime);
        SchedulePeriod GetScheduleTimeLine(ScheduleTime scheduleTime);
    }
}

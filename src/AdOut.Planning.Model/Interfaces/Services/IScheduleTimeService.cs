using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System;

namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IScheduleTimeService
    {
        TimeSpan GetTimeOfAdsShowing(ScheduleTime scheduleTime);
        SchedulePeriod GetSchedulePeriod(ScheduleTime scheduleTime);
    }
}

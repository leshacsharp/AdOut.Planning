using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Core.Services.Schedule
{
    public abstract class BaseScheduleTimeService : IScheduleTimeService
    {
        public SchedulePeriod GetSchedulePeriod(ScheduleTime scheduleTime)
        {
            var adTimeRanges = new List<TimeRange>();
            var adTimeWithBreak = scheduleTime.AdPlayTime + scheduleTime.AdBreakTime;
            TimeRange currentTimeRange = null;

            do
            {
                var adStartTime = TimeSpan.Zero;
                if (currentTimeRange == null)
                {
                    adStartTime = scheduleTime.ScheduleStartTime;
                }
                else
                {
                    adStartTime = currentTimeRange.End.Add(scheduleTime.AdBreakTime);
                }

                var adEndTime = adStartTime.Add(scheduleTime.AdPlayTime);
                var adTimeRange = new TimeRange(adStartTime, adEndTime);

                currentTimeRange = adTimeRange;
                adTimeRanges.Add(adTimeRange);
            }
            while (currentTimeRange.End + adTimeWithBreak <= scheduleTime.ScheduleEndTime);

            var sceduleAdPeriod = new SchedulePeriod()
            {
                Dates = GetPlanWorkingDays(scheduleTime),
                TimeRanges = adTimeRanges
            };

            return sceduleAdPeriod;
        }

        public TimeSpan GetTimeOfAdsShowing(ScheduleTime scheduleTime)
        {
            if (scheduleTime == null)
            {
                throw new ArgumentNullException(nameof(scheduleTime));
            }

            var executionPlanTimePerDay = scheduleTime.ScheduleEndTime - scheduleTime.ScheduleStartTime;
            var countAdPlaysPerDay = executionPlanTimePerDay / (scheduleTime.AdPlayTime + scheduleTime.AdBreakTime);
            var timeShowingPerDay = countAdPlaysPerDay * scheduleTime.AdPlayTime;
            var planWorkingDays = GetPlanWorkingDays(scheduleTime).Count;
            var timeShowing = timeShowingPerDay * planWorkingDays;

            return timeShowing;
        }

        protected abstract List<DateTime> GetPlanWorkingDays(ScheduleTime scheduleTime);
    }
}

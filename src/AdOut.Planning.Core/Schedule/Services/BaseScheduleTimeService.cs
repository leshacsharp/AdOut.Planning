using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Core.Schedule.Services
{
    public abstract class BaseScheduleTimeService : IScheduleTimeService
    {
        public SchedulePeriod GetSchedulePeriod(ScheduleTime scheduleTime)
        {
            var adTimeRanges = new List<TimeRange>();
            var adTimeWithBreak = scheduleTime.AdPlayTime + scheduleTime.AdBreakTime;
            TimeRange currentTimeRange = null;

            while (currentTimeRange.End + adTimeWithBreak <= scheduleTime.ScheduleEndTime)
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

            var planWorkingDays = GetPlanWorkingDays(scheduleTime).Count;
            var ExecutionPlanTimeByDay = scheduleTime.ScheduleEndTime - scheduleTime.ScheduleStartTime;
            var executionPlanTime = planWorkingDays * ExecutionPlanTimeByDay;

            var adTimeWithBreak = scheduleTime.AdPlayTime + scheduleTime.AdBreakTime;
            var countOfShowingAds = executionPlanTime / adTimeWithBreak;
            var adsShowingTime = countOfShowingAds * scheduleTime.AdPlayTime;

            return adsShowingTime;
        }

        protected abstract List<DateTime> GetPlanWorkingDays(ScheduleTime scheduleTime);
    }
}

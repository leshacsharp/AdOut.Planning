using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public abstract class BaseScheduleTimeHelper : IScheduleTimeHelper
    {
        public TimeSpan GetTimeOfAdsShowing(AdScheduleTime scheduleInfo)
        {
            if (scheduleInfo == null)
            {
                throw new ArgumentNullException(nameof(scheduleInfo));
            }

            var timeOfExecutingPlan = GetTimeOfExecutingPlan(scheduleInfo);

            var timeOfAdTimeWithBreak = scheduleInfo.AdPlayTime + scheduleInfo.AdBreakTime;
            var countOfShowingAds = timeOfExecutingPlan / timeOfAdTimeWithBreak;
            var timeOfShowingAds = countOfShowingAds * scheduleInfo.AdPlayTime;

            return timeOfShowingAds;
        }

        protected abstract TimeSpan GetTimeOfExecutingPlan(AdScheduleTime scheduleInfo);
    }
}

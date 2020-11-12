using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public abstract class BaseScheduleTimeHelper : IScheduleTimeHelper
    {
        public TimeSpan GetTimeOfAdsShowing(AdScheduleTime timeInfo)
        {
            if (timeInfo == null)
            {
                throw new ArgumentNullException(nameof(timeInfo));
            }

            var timeOfAdTimeWithBreak = timeInfo.AdPlayTime + timeInfo.AdBreakTime;

            var timeOfExecutingPlan = GetTimeOfExecutingPlan(timeInfo);

            var countOfShowingAds = timeOfExecutingPlan / timeOfAdTimeWithBreak;
            var timeOfShowingAds = countOfShowingAds * timeInfo.AdPlayTime;

            return timeOfShowingAds;
        }

        protected abstract TimeSpan GetTimeOfExecutingPlan(AdScheduleTime timeInfo);
    }
}

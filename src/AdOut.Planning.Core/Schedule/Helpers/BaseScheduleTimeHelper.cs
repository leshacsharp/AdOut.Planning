using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public abstract class BaseScheduleTimeHelper : IScheduleTimeHelper
    {
        public TimeSpan GetTimeOfAdsShowing(Plan plan, Model.Database.Schedule schedule)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            var timeOfOneAdShowWithBreak = plan.AdsTimePlaying + schedule.BreakTime;

            var timeOfExecutingPlan = GetTimeOfExecutingPlan(plan, schedule);

            var countOfShowingAds = timeOfExecutingPlan / timeOfOneAdShowWithBreak;
            var timeOfShowingAds = countOfShowingAds * plan.AdsTimePlaying;

            return timeOfShowingAds;
        }

        protected abstract TimeSpan GetTimeOfExecutingPlan(Plan plan, Model.Database.Schedule schedule);
    }
}

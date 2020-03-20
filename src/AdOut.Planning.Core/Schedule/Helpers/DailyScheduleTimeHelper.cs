using AdOut.Planning.Model.Database;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class DailyScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(Plan plan, Model.Database.Schedule schedule)
        {
            var daysOfExecutionPlan = (plan.EndDateTime - plan.StartDateTime).Days;
            daysOfExecutionPlan = daysOfExecutionPlan != 0 ? daysOfExecutionPlan : 1;

            var timeOfExecutingPlanByDay = schedule.EndTime - schedule.StartTime;
            var timeOfExecutingPlan = daysOfExecutionPlan * timeOfExecutingPlanByDay;

            return timeOfExecutingPlan;
        }
    }
}

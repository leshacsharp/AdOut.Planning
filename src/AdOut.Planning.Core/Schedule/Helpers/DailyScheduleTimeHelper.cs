using AdOut.Planning.Model.Classes;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class DailyScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(AdScheduleTime timeInfo)
        {
            //todo: add checking on off days og the plan

            var daysOfExecutionPlan = (timeInfo.PlanEndDateTime - timeInfo.PlanStartDateTime).Days;
            daysOfExecutionPlan = daysOfExecutionPlan != 0 ? daysOfExecutionPlan : 1;

            var timeOfExecutingPlanByDay = timeInfo.ScheduleEndTime - timeInfo.ScheduleStartTime;
            var timeOfExecutingPlan = daysOfExecutionPlan * timeOfExecutingPlanByDay;

            return timeOfExecutingPlan;
        }
    }
}

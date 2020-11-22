using AdOut.Planning.Model.Dto;
using System;
using System.Linq;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class DailyScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(AdScheduleTime scheduleInfo)
        {
            var daysOfExecutionPlan = GetDaysOfExecutionPlan(scheduleInfo.PlanStartDateTime, scheduleInfo.PlanEndDateTime, scheduleInfo.AdPointsDaysOff.ToArray());
            daysOfExecutionPlan = daysOfExecutionPlan != 0 ? daysOfExecutionPlan : 1;

            var timeOfExecutingPlanByDay = scheduleInfo.ScheduleEndTime - scheduleInfo.ScheduleStartTime;
            var timeOfExecutingPlan = daysOfExecutionPlan * timeOfExecutingPlanByDay;

            return timeOfExecutingPlan;
        }

        private int GetDaysOfExecutionPlan(DateTime from, DateTime to, DayOfWeek[] daysOff)
        {
            var daysCount = 0;
            var currentDate = new DateTime(from.Year, from.Month, from.Day);

            while (currentDate <= to)
            {
                if (!daysOff.Contains(currentDate.DayOfWeek))
                {
                    daysCount++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return daysCount;
        }
    }
}

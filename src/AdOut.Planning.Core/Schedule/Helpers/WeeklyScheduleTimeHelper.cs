using AdOut.Planning.Model.Dto;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class WeeklyScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(AdScheduleTime scheduleInfo)
        {
            var daysOfExecutionPlan = GetWeekDays(scheduleInfo.PlanStartDateTime, scheduleInfo.PlanEndDateTime, scheduleInfo.ScheduleDayOfWeek.Value);

            var timeOfExecutingPlanByDay = scheduleInfo.ScheduleEndTime - scheduleInfo.ScheduleStartTime;
            var timeOfExecutingPlan = daysOfExecutionPlan * timeOfExecutingPlanByDay;

            return timeOfExecutingPlan;
        }

        private int GetWeekDays(DateTime from, DateTime to, DayOfWeek day)
        {
            var daysCount = 0;
            var currentDate = new DateTime(from.Year, from.Month, from.Day);

            while (currentDate <= to)
            {
                if (currentDate.DayOfWeek == day)
                {
                    daysCount++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return daysCount;
        }
    }
}

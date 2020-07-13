using AdOut.Planning.Model.Classes;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class WeeklyScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(AdScheduleTime timeInfo)
        {
            var daysOfExecutionPlan = GetCountOfWeekDay(timeInfo.PlanStartDateTime, timeInfo.PlanEndDateTime, timeInfo.ScheduleDayOfWeek.Value);

            var timeOfExecutingPlanByDay = timeInfo.ScheduleEndTime - timeInfo.ScheduleStartTime;
            var timeOfExecutingPlan = daysOfExecutionPlan * timeOfExecutingPlanByDay;

            return timeOfExecutingPlan;
        }

        private int GetCountOfWeekDay(DateTime from, DateTime to, DayOfWeek day)
        {
            var daysCount = 0;
            var currentDate = from;
            currentDate = currentDate.AddHours(currentDate.Hour * -1);

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

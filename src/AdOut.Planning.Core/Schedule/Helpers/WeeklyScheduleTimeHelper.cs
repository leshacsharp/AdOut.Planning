using AdOut.Planning.Model.Database;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class WeeklyScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(Plan plan, Model.Database.Schedule schedule)
        {
            var daysOfExecutionPlan = GetCountOfWeekDay(plan.StartDateTime, plan.EndDateTime, schedule.DayOfWeek.Value);

            var timeOfExecutingPlanByDay = schedule.EndTime - schedule.StartTime;
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

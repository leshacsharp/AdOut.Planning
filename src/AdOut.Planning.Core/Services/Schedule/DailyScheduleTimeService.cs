using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdOut.Planning.Core.Services.Schedule
{
    public class DailyScheduleTimeService : BaseScheduleTimeService
    {
        protected override List<DateTime> GetPlanWorkingDays(ScheduleTime scheduleTime)
        {
            var workingDays = new List<DateTime>();
            var currentDate = scheduleTime.PlanStartDateTime;

            while (currentDate <= scheduleTime.PlanEndDateTime)
            {
                if (!scheduleTime.AdPointsDaysOff.Contains(currentDate.DayOfWeek))
                {
                    workingDays.Add(currentDate.Date);
                }
                currentDate = currentDate.AddDays(1);
            }

            return workingDays;
        }
    }
}

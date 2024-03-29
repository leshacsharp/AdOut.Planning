﻿using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Core.Services.Schedule
{
    public class WeeklyScheduleTimeService : BaseScheduleTimeService
    {
        protected override List<DateTime> GetPlanWorkingDays(ScheduleTime scheduleTime)
        {
            var workingDays = new List<DateTime>();
            var currentDate = scheduleTime.PlanStartDateTime;

            while (currentDate <= scheduleTime.PlanEndDateTime)
            {
                if (currentDate.DayOfWeek == scheduleTime.ScheduleDayOfWeek.Value)
                {
                    workingDays.Add(currentDate.Date);
                }
                currentDate = currentDate.AddDays(1);
            }

            return workingDays;
        }
    }
}

﻿using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorOrder(5)]
    class WeeklyIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Weekly)
            {
                var schedule = context.Schedule;
                foreach (var apAdPeriod in context.AdPointAdsPeriods)
                {
                    if ((apAdPeriod.Date == null && apAdPeriod.DayOfWeek == null) ||  //daily adPeriods
                         apAdPeriod.Date?.DayOfWeek == schedule.DayOfWeek ||          //specific adPeriods
                         apAdPeriod.DayOfWeek == schedule.DayOfWeek)                  //weekly adPeriods
                    {
                        foreach (var sAdPeriod in context.ScheduleAdsPeriods)
                        {
                            if (sAdPeriod.StartTime <= apAdPeriod.StartTime && sAdPeriod.EndTime >= apAdPeriod.StartTime ||  //left intersection
                                sAdPeriod.StartTime <= apAdPeriod.EndTime && sAdPeriod.EndTime >= apAdPeriod.EndTime ||      //right intersection
                                sAdPeriod.StartTime >= apAdPeriod.StartTime && sAdPeriod.EndTime <= apAdPeriod.EndTime)      //inner intersection
                            {
                                var sAdPeriodTimeMode = $"{sAdPeriod.DayOfWeek.Value}, {sAdPeriod.StartTime} - {sAdPeriod.EndTime}";

                                var apAdPeriodDayOfWeek = apAdPeriod.DayOfWeek != null ? apAdPeriod.DayOfWeek.ToString() : "not day of week";
                                var apAdPeriodTimeMode = $"{apAdPeriodDayOfWeek}, {apAdPeriod.StartTime} - {apAdPeriod.EndTime}";

                                var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, sAdPeriodTimeMode, apAdPeriodTimeMode);
                                context.Errors.Add(validationMessage);
                            }
                        }
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

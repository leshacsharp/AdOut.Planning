using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorOrder(6)]
    public class SpecificIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Specific)
            {
                var schedule = context.Schedule;
                var scheduleDate = schedule.Date.Value;

                foreach (var apAdPeriod in context.AdPointAdsPeriods)
                {
                    if ((apAdPeriod.Date == null && apAdPeriod.DayOfWeek == null) ||  //daily adPeriods
                         apAdPeriod.DayOfWeek == scheduleDate.DayOfWeek ||            //weekly adPeriods
                         apAdPeriod.Date == scheduleDate)                             //specific adPeriods
                    {
                        foreach (var sAdPeriod in context.ScheduleAdsPeriods)
                        {
                            if (sAdPeriod.StartTime <= apAdPeriod.StartTime && sAdPeriod.EndTime >= apAdPeriod.StartTime ||  //left intersection
                                sAdPeriod.StartTime <= apAdPeriod.EndTime && sAdPeriod.EndTime >= apAdPeriod.EndTime ||      //right intersection
                                sAdPeriod.StartTime >= apAdPeriod.StartTime && sAdPeriod.EndTime <= apAdPeriod.EndTime)      //inner intersection
                            {
                                var scheduleTimeMode = $"{scheduleDate.ToShortDateString()}, {scheduleDate.DayOfWeek}, {sAdPeriod.StartTime} - {sAdPeriod.EndTime}";

                                var apAdPeriodDate = apAdPeriod.Date != null ? apAdPeriod.Date.Value.ToShortDateString() : "not date";
                                var apAdPeriodDayOfWeek = apAdPeriod.DayOfWeek != null ? apAdPeriod.DayOfWeek.ToString() : "not day of week";
                                var apAdPeriodTimeMode = $"{apAdPeriodDate}, {apAdPeriodDayOfWeek}, {apAdPeriod.StartTime} - {apAdPeriod.EndTime}";

                                var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, scheduleTimeMode, apAdPeriodTimeMode);
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


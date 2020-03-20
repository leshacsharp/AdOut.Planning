using AdOut.Planning.Core.Schedule.Validators.Base;
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
                throw new ArgumentNullException(nameof(context));

            if (context.Plan.Type == PlanType.Weekly)
            {
                var schedule = context.Schedule;
                foreach (var adPeriod in context.AdsPeriods)
                {
                    if ((adPeriod.Date == null && adPeriod.DayOfWeek == null) ||          
                         adPeriod.Date?.DayOfWeek == schedule.DayOfWeek ||
                         adPeriod.DayOfWeek == schedule.DayOfWeek)
                    {
                        if (adPeriod.StartTime <= schedule.StartTime && adPeriod.EndTime >= schedule.StartTime ||
                            adPeriod.StartTime <= schedule.EndTime && adPeriod.EndTime >= schedule.EndTime ||
                            adPeriod.StartTime >= schedule.StartTime && adPeriod.EndTime <= schedule.EndTime)
                        {
                            var scheduleTimeMode = $"{schedule.DayOfWeek}, {schedule.StartTime} - {schedule.EndTime}";

                            var adPeriodDayOfWeek = adPeriod.DayOfWeek != null ? adPeriod.DayOfWeek.ToString() : "not day of week";
                            var adPeriodTimeMode = $"{adPeriodDayOfWeek}, {adPeriod.StartTime} - {adPeriod.EndTime}";

                            var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, scheduleTimeMode, adPeriodTimeMode, adPeriod.AdPointLocation);
                            context.Errors.Add(validationMessage);
                        }
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

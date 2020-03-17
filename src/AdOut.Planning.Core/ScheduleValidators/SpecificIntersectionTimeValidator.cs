using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    [ValidatorOrder(6)]
    public class SpecificIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Plan.Type == PlanType.Specific)
            {
                var schedule = context.Schedule;
                var scheduleDate = schedule.Date.Value;

                foreach (var adPeriod in context.AdsPeriods)
                {
                    if ((adPeriod.Date == null && adPeriod.DayOfWeek == null) ||
                         adPeriod.DayOfWeek == scheduleDate.DayOfWeek ||
                         adPeriod.Date == scheduleDate)
                    {
                        if (adPeriod.StartTime <= schedule.StartTime && adPeriod.EndTime >= schedule.StartTime ||
                            adPeriod.StartTime <= schedule.EndTime && adPeriod.EndTime >= schedule.EndTime ||
                            adPeriod.StartTime >= schedule.StartTime && adPeriod.EndTime <= schedule.EndTime)
                        {
                            var scheduleTimeMode = $"{scheduleDate.ToShortDateString()}, {scheduleDate.DayOfWeek}, {schedule.StartTime} - {schedule.EndTime}";

                            var adPeriodDate = adPeriod.Date != null ? adPeriod.Date.Value.ToShortDateString() : "not date";
                            var adPeriodDayOfWeek = adPeriod.DayOfWeek != null ? adPeriod.DayOfWeek.ToString() : "not day of week";
                            var adPeriodTimeMode = $"{adPeriodDate}, {adPeriodDayOfWeek}, {adPeriod.StartTime} - {adPeriod.EndTime}";

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


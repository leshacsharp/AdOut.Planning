using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Classes;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class SpecificIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Valid(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.PlanType == Model.Enum.PlanType.Specific)
            {
                var scheduleDate = context.Date.Value;

                foreach (var adPeriod in context.AdsPeriods)
                {
                    if ((adPeriod.Date == null && adPeriod.DayOfWeek == null) ||
                         adPeriod.DayOfWeek == scheduleDate.DayOfWeek ||
                         adPeriod.Date == context.Date)
                    {
                        if (adPeriod.StartTime <= context.StartTime && adPeriod.EndTime >= context.StartTime ||
                            adPeriod.StartTime <= context.EndTime && adPeriod.EndTime >= context.EndTime ||
                            adPeriod.StartTime >= context.StartTime && adPeriod.EndTime <= context.EndTime)
                        {
                            var scheduleTimeMode = $"{context.Date.Value.ToShortDateString()}, {scheduleDate.DayOfWeek}, {context.StartTime} - {context.EndTime}";

                            var adPeriodDate = adPeriod.Date != null ? adPeriod.Date.Value.ToShortDateString() : "not date";
                            var adPeriodDayOfWeek = adPeriod.DayOfWeek != null ? adPeriod.DayOfWeek.ToString() : "not day of week";
                            var adPeriodTimeMode = $"{adPeriodDate}, {adPeriodDayOfWeek}, {adPeriod.StartTime} - {adPeriod.EndTime}";

                            var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, scheduleTimeMode, adPeriodTimeMode, adPeriod.AdPointLocation);
                            context.Errors.Add(validationMessage);
                        }
                    }
                }
            }

            _nextValidator?.Valid(context);
        }
    }
}


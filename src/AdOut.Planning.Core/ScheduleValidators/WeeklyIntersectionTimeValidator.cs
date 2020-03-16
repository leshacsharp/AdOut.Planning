using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    [ValidatorOrder(4)]
    class WeeklyIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.PlanType == Model.Enum.PlanType.Weekly)
            {
                foreach (var adPeriod in context.AdsPeriods)
                {
                    if ((adPeriod.Date == null && adPeriod.DayOfWeek == null) ||          
                         adPeriod.Date?.DayOfWeek == context.DayOfWeek ||
                         adPeriod.DayOfWeek == context.DayOfWeek)
                    {
                        if (adPeriod.StartTime <= context.StartTime && adPeriod.EndTime >= context.StartTime ||
                            adPeriod.StartTime <= context.EndTime && adPeriod.EndTime >= context.EndTime ||
                            adPeriod.StartTime >= context.StartTime && adPeriod.EndTime <= context.EndTime)
                        {
                            var scheduleTimeMode = $"{context.DayOfWeek}, {context.StartTime} - {context.EndTime}";

                            var adPeriodDayOfWeek = adPeriod.DayOfWeek != null ? adPeriod.DayOfWeek.ToString() : "not day of week";
                            var adPeriodTimeMode = $"{adPeriodDayOfWeek}, {adPeriod.StartTime} - {adPeriod.EndTime}";

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

using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    [ValidatorOrder(3)]
    public class DailyIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.PlanType == Model.Enum.PlanType.Daily)
            {
                foreach (var adPeriod in context.AdsPeriods)
                {
                    if (adPeriod.StartTime <= context.StartTime && adPeriod.EndTime >= context.StartTime ||
                        adPeriod.StartTime <= context.EndTime && adPeriod.EndTime >= context.EndTime ||
                        adPeriod.StartTime >= context.StartTime && adPeriod.EndTime <= context.EndTime)
                    {
                        var schedulerTimeMode = $"{context.StartTime} - {context.EndTime}";
                        var adPeriodTimeMode = $"{adPeriod.StartTime} - {adPeriod.EndTime}"; 

                        var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, schedulerTimeMode, adPeriodTimeMode, adPeriod.AdPointLocation);
                        context.Errors.Add(validationMessage);
                    }
                }
            }
            
            _nextValidator?.Validate(context);
        }
    }
}

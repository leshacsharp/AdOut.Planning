using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    [ValidatorOrder(4)]
    public class DailyIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Plan.Type == PlanType.Daily)
            {
                var schedule = context.Schedule;
                foreach (var adPeriod in context.AdsPeriods)
                {
                    if (adPeriod.StartTime <= schedule.StartTime && adPeriod.EndTime >= schedule.StartTime ||
                        adPeriod.StartTime <= schedule.EndTime && adPeriod.EndTime >= schedule.EndTime ||
                        adPeriod.StartTime >= schedule.StartTime && adPeriod.EndTime <= schedule.EndTime)
                    {
                        var schedulerTimeMode = $"{schedule.StartTime} - {schedule.EndTime}";
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

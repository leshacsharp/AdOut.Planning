using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorOrder(4)]
    public class DailyIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Daily)
            {
                foreach (var apAdPeriod in context.AdPointAdsPeriods)
                {
                    foreach (var sAdPeriod in context.ScheduleAdsPeriods)
                    {
                        if (sAdPeriod.StartTime <= apAdPeriod.StartTime && sAdPeriod.EndTime >= apAdPeriod.StartTime ||  //left intersection
                            sAdPeriod.StartTime <= apAdPeriod.EndTime && sAdPeriod.EndTime >= apAdPeriod.EndTime ||      //right intersection
                            sAdPeriod.StartTime >= apAdPeriod.StartTime && sAdPeriod.EndTime <= apAdPeriod.EndTime)      //inner intersection   
                        {
                            var sAdPeriodTimeMode = $"{sAdPeriod.StartTime} - {sAdPeriod.EndTime}";
                            var apAdPeriodTimeMode = $"{apAdPeriod.StartTime} - {apAdPeriod.EndTime}";

                            var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, sAdPeriodTimeMode, apAdPeriodTimeMode);
                            context.Errors.Add(validationMessage);
                        }
                    }
                }
            }
            
            _nextValidator?.Validate(context);
        }
    }
}

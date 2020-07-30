using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.IntersectionTime)]
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
                foreach (var adPoint in context.AdPoints)
                {
                    foreach (var eAdPeriod in adPoint.AdPeriods)
                    {
                        foreach (var newAdPeriod in context.NewAdsPeriods)
                        {
                            if (newAdPeriod.StartTime <= eAdPeriod.StartTime && newAdPeriod.EndTime >= eAdPeriod.StartTime ||  //left intersection
                                newAdPeriod.StartTime <= eAdPeriod.EndTime && newAdPeriod.EndTime >= eAdPeriod.EndTime ||      //right intersection
                                newAdPeriod.StartTime >= eAdPeriod.StartTime && newAdPeriod.EndTime <= eAdPeriod.EndTime)      //inner intersection   
                            {
                                var sAdPeriodTimeMode = $"{newAdPeriod.StartTime} - {newAdPeriod.EndTime}";
                                var eAdPeriodTimeMode = $"{eAdPeriod.StartTime} - {eAdPeriod.EndTime}";

                                var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersection_T, sAdPeriodTimeMode, eAdPeriodTimeMode, adPoint.Location);
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

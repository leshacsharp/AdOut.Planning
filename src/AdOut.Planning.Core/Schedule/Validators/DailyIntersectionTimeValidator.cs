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



            //todo: refactoring
            if (context.ScheduleType == ScheduleType.Daily)
            {
                //todo: get existing ad periods that have same dates as in the new ad period

                foreach (var eAdPeriod in context.ExistingAdPeriods)
                {
                    foreach (var existingTR in eAdPeriod.TimeRanges)
                    {
                        foreach (var newTR in context.NewAdPeriod.TimeRanges)
                        {
                            if (existingTR.IsInterescted(newTR))
                            {
                                //refactor errors messages
                                var sAdPeriodTimeMode = $"{newAdPeriod.StartTime} - {newAdPeriod.EndTime}";
                                var eAdPeriodTimeMode = $"{eAdPeriod.StartTime} - {eAdPeriod.EndTime}";

                                var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersection_T, sAdPeriodTimeMode, eAdPeriodTimeMode);
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

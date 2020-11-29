using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.IntersectionTime)]
    public class IntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var napDate in context.NewAdPeriod.Dates)
            {
                var adPeriodsWithCommonDate = context.ExistingAdPeriods.Where(eap => eap.Dates.Contains(napDate));
                var existingTimeRanges = adPeriodsWithCommonDate.SelectMany(ap => ap.TimeRanges).ToList();
               
                foreach (var newTimeRange in context.NewAdPeriod.TimeRanges)
                {
                    var haveIntersecttion = existingTimeRanges.Any(etr => etr.IsInterescted(newTimeRange));
                    if (haveIntersecttion)
                    {
                        var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersection_T, napDate.ToShortDateString());
                        context.Errors.Add(validationMessage);
                        break;
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

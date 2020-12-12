using AdOut.Planning.Core.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Validators.Schedule
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

            foreach (var nspDate in context.NewSchedulePeriod.Dates)
            {
                var periodsWithCommonDate = context.ExistingSchedulePeriods.Where(esp => esp.Dates.Contains(nspDate));
                var existingTimeRanges = periodsWithCommonDate.SelectMany(ap => ap.TimeRanges).ToList();
               
                foreach (var newTimeRange in context.NewSchedulePeriod.TimeRanges)
                {
                    var haveIntersection = existingTimeRanges.Any(etr => etr.IsInterescted(newTimeRange));
                    if (haveIntersection)
                    {
                        var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersected_T, nspDate.ToShortDateString());
                        context.Errors.Add(validationMessage);
                        break;
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

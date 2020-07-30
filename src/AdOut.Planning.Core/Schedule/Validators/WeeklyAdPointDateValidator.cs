using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.AdPoint)]
    public class WeeklyAdPointDateValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Weekly)
            {
                var scheduleDayOfWeek = context.Schedule.DayOfWeek.Value;

                foreach (var adPoint in context.AdPoints)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(scheduleDayOfWeek);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ValidationMessages.Schedule.DayIsADayOff_T, scheduleDayOfWeek, adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            _nextValidator?.Validate(context);
        }  
    }
}

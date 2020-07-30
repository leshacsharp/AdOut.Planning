using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{ 
    [ValidatorType(ValidatorType.AdPoint)]
    public class SpecificAdPointDateValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Specific)
            {
                var schedulerDate = context.Schedule.Date.Value;
                var schedulerDayOfWeek = schedulerDate.DayOfWeek;

                foreach (var adPoint in context.AdPoints)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(schedulerDayOfWeek);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ValidationMessages.Schedule.DateIsADayOff_T, schedulerDate.ToShortDateString(), adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

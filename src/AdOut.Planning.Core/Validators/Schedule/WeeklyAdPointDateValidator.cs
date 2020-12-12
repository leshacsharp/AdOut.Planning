using AdOut.Planning.Core.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Validators.Schedule
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

            if (context.ScheduleType == ScheduleType.Weekly)
            {
                var scheduleDayOfWeek = context.ScheduleDayOfWeek.Value;

                foreach (var adPoint in context.AdPoints)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(scheduleDayOfWeek);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ValidationMessages.Schedule.DayDayOff_T, scheduleDayOfWeek, adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            _nextValidator?.Validate(context);
        }  
    }
}

using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class WeeklyAdPointDateValidator : BaseScheduleValidator
    {
        public override void Valid(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.PlanType == PlanType.Weekly)
            {
                var schedulerDayOfWeek = context.DayOfWeek.Value;

                foreach (var adPoint in context.AdPointValidations)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(schedulerDayOfWeek);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ScheduleValidationMessages.ScheduleDayIsADayOff_T, schedulerDayOfWeek, adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            _nextValidator?.Valid(context);
        }  
    }
}

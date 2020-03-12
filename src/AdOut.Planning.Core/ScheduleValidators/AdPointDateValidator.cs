using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class AdPointDateValidator : IScheduleValidator
    {
        private IScheduleValidator _nextValidator;

        public void Valid(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.PlanType == PlanType.Weekly)
            {
                var schedulerDay = (DayOfWeek)context.DayOfWeek;

                foreach (var adPoint in context.AdPointValidations)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(schedulerDay);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ScheduleValidationMessages.ScheduleDayIsADayOff_T, schedulerDay, adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            else if (context.PlanType == PlanType.Specific)
            {
                var schedulerDate = (DateTime)context.Date;
                var schedulerDay = schedulerDate.DayOfWeek;

                foreach (var adPoint in context.AdPointValidations)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(schedulerDay);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ScheduleValidationMessages.ScheduleDateIsADayOff_T, schedulerDate.ToShortDateString(), adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            _nextValidator?.Valid(context);
        }

        public void SetNextValidator(IScheduleValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _nextValidator = validator;
        }
    }
}

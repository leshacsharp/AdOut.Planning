using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class AdPointTimeValidator : IScheduleValidator
    {
        private IScheduleValidator _nextValidator;

        public void Valid(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            foreach (var adPoint in context.AdPointValidations)
            {
                if(context.StartTime < adPoint.StartWorkingTime || context.EndTime > adPoint.EndWorkingTime)
                {
                    var schedulerTimeMode = $"{context.StartTime} - {context.EndTime}";
                    var adPointTimeMode = $"{adPoint.StartWorkingTime} - {adPoint.EndWorkingTime}";

                    var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIsNotAllowed_T, schedulerTimeMode, adPoint.Location, adPointTimeMode);
                    context.Errors.Add(validationMessage);
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

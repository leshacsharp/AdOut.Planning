using AdOut.Planning.Core.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Validators.Schedule
{
    [ValidatorType(ValidatorType.AdPoint)]
    public class AdPointTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var adPoint in context.AdPoints)
            {
                if (context.ScheduleStartTime < adPoint.StartWorkingTime || context.ScheduleEndTime > adPoint.EndWorkingTime)
                {
                    var schedulerTimeMode = $"{context.ScheduleStartTime}-{context.ScheduleEndTime}";
                    var adPointTimeMode = $"{adPoint.StartWorkingTime}-{adPoint.EndWorkingTime}";

                    var validationMessage = string.Format(ValidationMessages.Schedule.TimeNotAllowed_T, schedulerTimeMode, adPoint.Location, adPointTimeMode);
                    context.Errors.Add(validationMessage);
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

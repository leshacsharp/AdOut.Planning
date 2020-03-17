using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ScheduleValidators
{
    [ValidatorOrder(2)]
    public class AdPointTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
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

            _nextValidator?.Validate(context);
        }
    }
}

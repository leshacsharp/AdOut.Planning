using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorOrder(2)]
    public class AdPointTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var schedule = context.Schedule;
            foreach (var adPoint in context.AdPointsValidations)
            {
                if(schedule.StartTime < adPoint.StartWorkingTime || schedule.EndTime > adPoint.EndWorkingTime)
                {
                    var schedulerTimeMode = $"{schedule.StartTime} - {schedule.EndTime}";
                    var adPointTimeMode = $"{adPoint.StartWorkingTime} - {adPoint.EndWorkingTime}";

                    var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIsNotAllowed_T, schedulerTimeMode, adPoint.Location, adPointTimeMode);
                    context.Errors.Add(validationMessage);
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

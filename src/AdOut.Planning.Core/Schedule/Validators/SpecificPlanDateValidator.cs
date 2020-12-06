using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.Plan)]
    public class SpecificPlanDateValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ScheduleType == ScheduleType.Specific)
            {  
                if (context.ScheduleDate < context.PlanStartDateTime || context.ScheduleDate > context.PlanEndDateTime)
                {
                    var scheduleDate = context.ScheduleDate.Value.ToShortDateString();
                    var planBounds = $"{context.PlanStartDateTime.ToShortDateString()}-{context.PlanEndDateTime.ToShortDateString()}";

                    var validationMessage = string.Format(ValidationMessages.Schedule.DateBounds_T, scheduleDate, planBounds);
                    context.Errors.Add(validationMessage);
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

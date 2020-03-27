using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.SchedulePlan)]
    public class SpecificPlanDateValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
        
            if(context.Plan.Type == PlanType.Specific)
            { 
                var schedule = context.Schedule;
                if (schedule.Date < context.Plan.StartDateTime || schedule.Date > context.Plan.EndDateTime)
                {
                    var scheduleDate = schedule.Date.Value.ToShortDateString();
                    var planBounds = $"{context.Plan.StartDateTime.ToShortDateString()} - {context.Plan.EndDateTime.ToShortDateString()}";

                    var validationMessage = string.Format(ScheduleValidationMessages.ScheduleDateOutOfBounds_T, scheduleDate, planBounds);
                    context.Errors.Add(validationMessage);
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

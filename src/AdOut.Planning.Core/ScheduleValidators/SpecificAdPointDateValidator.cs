using AdOut.Planning.Core.ScheduleValidators.Base;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using System.Linq;
using static AdOut.Planning.Model.Constants;
namespace AdOut.Planning.Core.ScheduleValidators
{
    public class SpecificAdPointDateValidator : BaseScheduleValidator
    {
        public override void Valid(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.PlanType == PlanType.Specific)
            {
                var schedulerDate = context.Date.Value;
                var schedulerDayOfWeek = schedulerDate.DayOfWeek;

                foreach (var adPoint in context.AdPointValidations)
                {
                    var isScheduleDayADayOff = adPoint.DaysOff.Contains(schedulerDayOfWeek);
                    if (isScheduleDayADayOff)
                    {
                        var validationMessage = string.Format(ScheduleValidationMessages.ScheduleDateIsADayOff_T, schedulerDate.ToShortDateString(), adPoint.Location);
                        context.Errors.Add(validationMessage);
                    }
                }
            }

            _nextValidator?.Valid(context);
        }
    }
}

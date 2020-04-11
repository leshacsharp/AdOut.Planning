using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.IntersectionTime)]
    public class SpecificIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Specific)
            {
                var schedule = context.Schedule;
                var scheduleDate = schedule.Date.Value;

                foreach (var eAdPeriod in context.ExistingAdsPeriods)
                {
                    if ((eAdPeriod.Date == null && eAdPeriod.DayOfWeek == null) ||   //daily adPeriods
                         eAdPeriod.DayOfWeek == scheduleDate.DayOfWeek ||            //weekly adPeriods
                         eAdPeriod.Date == scheduleDate)                             //specific adPeriods
                    {
                        foreach (var newAdPeriod in context.NewAdsPeriods)
                        {
                            if (newAdPeriod.StartTime <= eAdPeriod.StartTime && newAdPeriod.EndTime >= eAdPeriod.StartTime ||  //left intersection
                                newAdPeriod.StartTime <= eAdPeriod.EndTime && newAdPeriod.EndTime >= eAdPeriod.EndTime ||      //right intersection
                                newAdPeriod.StartTime >= eAdPeriod.StartTime && newAdPeriod.EndTime <= eAdPeriod.EndTime)      //inner intersection
                            {
                                var scheduleTimeMode = $"{scheduleDate.ToShortDateString()}, {scheduleDate.DayOfWeek}, {newAdPeriod.StartTime} - {newAdPeriod.EndTime}";

                                var eAdPeriodDate = eAdPeriod.Date != null ? eAdPeriod.Date.Value.ToShortDateString() : "not date";
                                var eAdPeriodDayOfWeek = eAdPeriod.DayOfWeek != null ? eAdPeriod.DayOfWeek.ToString() : "not day of week";
                                var eAdPeriodTimeMode = $"{eAdPeriodDate}, {eAdPeriodDayOfWeek}, {eAdPeriod.StartTime} - {eAdPeriod.EndTime}";

                                var validationMessage = string.Format(ScheduleValidationMessages.ScheduleTimeIntersection_T, scheduleTimeMode, eAdPeriodTimeMode);
                                context.Errors.Add(validationMessage);
                            }
                        }
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}


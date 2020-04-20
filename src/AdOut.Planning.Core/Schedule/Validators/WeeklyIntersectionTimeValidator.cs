using AdOut.Planning.Core.Schedule.Validators.Base;
using AdOut.Planning.Model.Attributes;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Schedule.Validators
{
    [ValidatorType(ValidatorType.IntersectionTime)]
    class WeeklyIntersectionTimeValidator : BaseScheduleValidator
    {
        public override void Validate(ScheduleValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Plan.Type == PlanType.Weekly)
            {
                var schedule = context.Schedule;
                foreach (var eAdPeriod in context.ExistingAdsPeriods)
                {
                    if ((eAdPeriod.Date == null && eAdPeriod.DayOfWeek == null) ||   //daily adPeriods
                         eAdPeriod.Date?.DayOfWeek == schedule.DayOfWeek ||          //specific adPeriods
                         eAdPeriod.DayOfWeek == schedule.DayOfWeek)                  //weekly adPeriods
                    {
                        foreach (var newAdPeriod in context.NewAdsPeriods)
                        {
                            if (newAdPeriod.StartTime <= eAdPeriod.StartTime && newAdPeriod.EndTime >= eAdPeriod.StartTime ||  //left intersection
                                newAdPeriod.StartTime <= eAdPeriod.EndTime && newAdPeriod.EndTime >= eAdPeriod.EndTime ||      //right intersection
                                newAdPeriod.StartTime >= eAdPeriod.StartTime && newAdPeriod.EndTime <= eAdPeriod.EndTime)      //inner intersection
                            {
                                var newAdPeriodTimeMode = $"{newAdPeriod.DayOfWeek.Value}, {newAdPeriod.StartTime} - {newAdPeriod.EndTime}";

                                var eAdPeriodDayOfWeek = eAdPeriod.DayOfWeek != null ? eAdPeriod.DayOfWeek.ToString() : "not day of week";
                                var eAdPeriodTimeMode = $"{eAdPeriodDayOfWeek}, {eAdPeriod.StartTime} - {eAdPeriod.EndTime}";

                                var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersection_T, newAdPeriodTimeMode, eAdPeriodTimeMode);
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

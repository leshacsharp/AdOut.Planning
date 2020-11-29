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

            if (context.Plan.Type == ScheduleType.Weekly)
            {
                foreach (var adPoint in context.AdPoints)
                {
                    foreach (var eAdPeriod in adPoint.AdPeriods)
                    {
                        //choosing existed adPeriods for needed days (for example, we have new schedule for Monday and therefore we need to take adPeriods that are daily or provide Monday)
                        if ((eAdPeriod.Date == null && eAdPeriod.DayOfWeek == null) ||   //daily adPeriods
                             eAdPeriod.Date?.DayOfWeek == context.Schedule.DayOfWeek ||  //specific adPeriods
                             eAdPeriod.DayOfWeek == context.Schedule.DayOfWeek)          //weekly adPeriods
                        {
                            foreach (var newAdPeriod in context.NewAdsPeriods)
                            {
                                if ((newAdPeriod.StartTime <= eAdPeriod.StartTime && newAdPeriod.EndTime >= eAdPeriod.StartTime) ||  //left intersection
                                    (newAdPeriod.StartTime <= eAdPeriod.EndTime && newAdPeriod.EndTime >= eAdPeriod.EndTime) ||      //right intersection
                                    (newAdPeriod.StartTime >= eAdPeriod.StartTime && newAdPeriod.EndTime <= eAdPeriod.EndTime))      //inner intersection
                                {
                                    var newAdPeriodTimeMode = $"{newAdPeriod.DayOfWeek.Value}, {newAdPeriod.StartTime} - {newAdPeriod.EndTime}";
                                    var eAdPeriodTimeMode = $"{eAdPeriod.DayOfWeek}, {eAdPeriod.StartTime} - {eAdPeriod.EndTime}";

                                    var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersection_T, newAdPeriodTimeMode, eAdPeriodTimeMode, adPoint.Location);
                                    context.Errors.Add(validationMessage);
                                }
                            }
                        }
                    }
                }
            }

            _nextValidator?.Validate(context);
        }
    }
}

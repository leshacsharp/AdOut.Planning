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
                var scheduleDate = context.Schedule.Date.Value;
                foreach (var adPoint in context.AdPoints)
                {
                    foreach (var eAdPeriod in adPoint.AdPeriods)
                    {
                        if ((eAdPeriod.Date == null && eAdPeriod.DayOfWeek == null) ||   //daily adPeriods
                             eAdPeriod.DayOfWeek == scheduleDate.DayOfWeek ||            //weekly adPeriods
                             eAdPeriod.Date == scheduleDate)                             //specific adPeriods
                        {
                            foreach (var newAdPeriod in context.NewAdsPeriods)
                            {
                                if ((newAdPeriod.StartTime <= eAdPeriod.StartTime && newAdPeriod.EndTime >= eAdPeriod.StartTime) ||  //left intersection
                                    (newAdPeriod.StartTime <= eAdPeriod.EndTime && newAdPeriod.EndTime >= eAdPeriod.EndTime) ||      //right intersection
                                    (newAdPeriod.StartTime >= eAdPeriod.StartTime && newAdPeriod.EndTime <= eAdPeriod.EndTime))      //inner intersection
                                {
                                    var eAdPeriodDate = eAdPeriod.Date.Value;
                                    var scheduleTimeMode = $"{scheduleDate.ToShortDateString()}, {scheduleDate.DayOfWeek}, {newAdPeriod.StartTime} - {newAdPeriod.EndTime}";
                                    var eAdPeriodTimeMode = $"{eAdPeriodDate.ToShortDateString()}, {eAdPeriodDate.DayOfWeek}, {eAdPeriod.StartTime} - {eAdPeriod.EndTime}";

                                    var validationMessage = string.Format(ValidationMessages.Schedule.TimeIntersection_T, scheduleTimeMode, eAdPeriodTimeMode, adPoint.Location);
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


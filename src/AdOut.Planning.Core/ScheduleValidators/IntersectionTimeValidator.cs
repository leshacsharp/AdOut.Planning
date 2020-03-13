using AdOut.Planning.Common.Collections;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdOut.Planning.Core.ScheduleValidators
{
    public class IntersectionTimeValidator : IScheduleValidator
    {
        private IScheduleValidator _nextValidator;

        public void Valid(ScheduleValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //initialize ads periods

            var adsPeriods = new List<AdPeriod>();

            foreach (var adPoint in context.AdPointValidations)
            {
                foreach (var plan in adPoint.Plans)
                {
                    var orderedPlanAds = plan.PlanAds.OrderBy(pa => pa.Order);
                    var ads = new CircleList<PlanAdValidation>(orderedPlanAds);

                    AdPeriod previosAdPeriod = null;
                    foreach (var schedule in plan.Schedules)
                    {
                        var adStartTime = TimeSpan.Zero;
                        if (previosAdPeriod == null)
                        {
                            adStartTime = schedule.StartTime;
                        }
                        else
                        {
                            adStartTime = previosAdPeriod.EndTime.Add(schedule.BreakTime);
                        }

                        var ad = ads.Next();
                        var adTimePlaying = TimeSpan.FromSeconds(ad.TimePlayingSec);
                        var adEndTime = adStartTime.Add(adTimePlaying);

                        var adPeriod = new AdPeriod()
                        {
                            StartTime = adStartTime,
                            EndTime = adEndTime,
                            Date = schedule.Date,
                            DayOfWeek = schedule.DayOfWeek
                        };

                        previosAdPeriod = adPeriod;
                        adsPeriods.Add(adPeriod);
                    }
                }
            }

            if (context.PlanType == Model.Enum.PlanType.Daily)
            {
                foreach (var adPeriod in adsPeriods)
                {
                    if (adPeriod.StartTime <= context.StartTime && adPeriod.EndTime >= context.StartTime ||
                       adPeriod.StartTime <= context.EndTime && adPeriod.EndTime >= context.EndTime ||
                       adPeriod.StartTime >= context.StartTime && adPeriod.EndTime <= context.EndTime)
                    {
                        context.Errors.Add("ou shit conflict by times");
                    }
                }
            }

            if (context.PlanType == Model.Enum.PlanType.Specific)
            {
                var scheduleDayOfWeek = context.Date.Value.DayOfWeek;

                foreach (var adPeriod in adsPeriods)
                {
                    if ((adPeriod.Date == null && adPeriod.DayOfWeek == null) || adPeriod.Date == context.Date || adPeriod.DayOfWeek == scheduleDayOfWeek)
                    {
                        if (adPeriod.StartTime <= context.StartTime && adPeriod.EndTime >= context.StartTime ||
                           adPeriod.StartTime <= context.EndTime && adPeriod.EndTime >= context.EndTime ||
                           adPeriod.StartTime >= context.StartTime && adPeriod.EndTime <= context.EndTime)
                        {
                            context.Errors.Add("ou shit conflict by times");
                        }
                    }
                }
            }

            if (context.PlanType == Model.Enum.PlanType.Weekly)
            {
                foreach (var adPeriod in adsPeriods)
                {
                    if (adPeriod.DayOfWeek == null || adPeriod.DayOfWeek == context.DayOfWeek)
                    {
                        if (adPeriod.StartTime <= context.StartTime && adPeriod.EndTime >= context.StartTime ||
                           adPeriod.StartTime <= context.EndTime && adPeriod.EndTime >= context.EndTime ||
                           adPeriod.StartTime >= context.StartTime && adPeriod.EndTime <= context.EndTime)
                        {
                            context.Errors.Add("ou shit conflict by times");
                        }
                    }
                }
            }


            _nextValidator?.Valid(context);
        }

        public void SetNextValidator(IScheduleValidator validator)
        {
            if (validator == null)
                throw new ArgumentNullException(nameof(validator));

            _nextValidator = validator;
        }
    }

    public class AdPeriod
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime? Date { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }
    }
}

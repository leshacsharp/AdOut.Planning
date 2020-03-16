using AdOut.Planning.Common.Collections;
using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdOut.Planning.Core.Managers
{
    public class ScheduleManager : BaseManager<Schedule>, IScheduleManager
    {
        public ScheduleManager(IScheduleRepository repository) :
            base(repository)
        {
        }

        private List<AdPeriod> GenerateTimeLine(List<AdPointValidation> adPoints)
        {
            var adsPeriods = new List<AdPeriod>();

            foreach (var adPoint in adPoints)
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
                            AdPointLocation = adPoint.Location,
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

            return adsPeriods;
        }
    }
}

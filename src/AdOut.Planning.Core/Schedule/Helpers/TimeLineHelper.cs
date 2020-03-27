using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class TimeLineHelper : ITimeLineHelper
    {
        public List<AdPeriod> GetScheduleTimeLine(ScheduleDto schedule, TimeSpan adsTimePlaying)
        {
            var adsPeriods = new List<AdPeriod>();

            AdPeriod currentAdPeriod = null;
            var adTimeWithBreak = adsTimePlaying + schedule.BreakTime;

            while (currentAdPeriod.EndTime + adTimeWithBreak <= schedule.EndTime)
            {
                var adStartTime = TimeSpan.Zero;
                if (currentAdPeriod == null)
                {
                    adStartTime = schedule.StartTime;
                }
                else
                {
                    adStartTime = currentAdPeriod.EndTime.Add(schedule.BreakTime);
                }

                var adEndTime = adStartTime.Add(adsTimePlaying);
                var adPeriod = new AdPeriod()
                {
                    StartTime = adStartTime,
                    EndTime = adEndTime,
                    Date = schedule.Date,
                    DayOfWeek = schedule.DayOfWeek
                };

                currentAdPeriod = adPeriod;
                adsPeriods.Add(adPeriod);
            }

            return adsPeriods;
        }
    }
}

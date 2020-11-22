using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class TimeLineHelper : ITimeLineHelper
    {
        public AdPeriod GetScheduleTimeLine(ScheduleDto schedule, DateTime planStart, DateTime planEnd)
        {     
            var adTimeRanges = new List<TimeRange>();
            var adTimeWithBreak = schedule.PlayTime + schedule.BreakTime;
            TimeRange currentTimeRange = null;

            while (currentTimeRange.End + adTimeWithBreak <= schedule.EndTime)
            {
                var adStartTime = TimeSpan.Zero;
                if (currentTimeRange == null)
                {
                    adStartTime = schedule.StartTime;
                }
                else
                {
                    adStartTime = currentTimeRange.End.Add(schedule.BreakTime);
                }

                var adEndTime = adStartTime.Add(schedule.PlayTime);
                var adTimeRange = new TimeRange(adStartTime, adEndTime);

                currentTimeRange = adTimeRange;
                adTimeRanges.Add(adTimeRange);
            }

            var adDates = new List<DateTime>();
            var currentDate = new DateTime(planStart.Year, planStart.Month, planStart.Day);

            while (currentDate <= planEnd)
            {
                adDates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            var sceduleAdPeriod = new AdPeriod()
            {
                Dates = adDates,
                TimeRanges = adTimeRanges
            };

            return sceduleAdPeriod;
        }

        //public List<AdPeriod> GetScheduleTimeLine(ScheduleDto schedule)
        //{
        //    var adsPeriods = new List<AdPeriod>();

        //    AdPeriod currentAdPeriod = null;
        //    var adTimeWithBreak = schedule.PlayTime + schedule.BreakTime;

        //    while (currentAdPeriod.EndTime + adTimeWithBreak <= schedule.EndTime)
        //    {
        //        var adStartTime = TimeSpan.Zero;
        //        if (currentAdPeriod == null)
        //        {
        //            adStartTime = schedule.StartTime;
        //        }
        //        else
        //        {
        //            adStartTime = currentAdPeriod.EndTime.Add(schedule.BreakTime);
        //        }

        //        var adEndTime = adStartTime.Add(schedule.PlayTime);
        //        var adPeriod = new AdPeriod()
        //        {
        //            StartTime = adStartTime,
        //            EndTime = adEndTime,
        //            Date = schedule.Date,
        //            DayOfWeek = schedule.DayOfWeek
        //        };

        //        currentAdPeriod = adPeriod;
        //        adsPeriods.Add(adPeriod);
        //    }

        //    return adsPeriods;
        //}
    }
}

using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface ITimeLineHelper
    {
        //todo: need to think about arguments (simple arguments will be better)
        List<AdPeriod> GetScheduleTimeLine(ScheduleDto schedule, TimeSpan adsTimePlaying);
    }
}

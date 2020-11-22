using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface ITimeLineHelper
    {
        AdPeriod GetScheduleTimeLine(ScheduleDto schedule, DateTime planStart, DateTime planEnd)
    }
}

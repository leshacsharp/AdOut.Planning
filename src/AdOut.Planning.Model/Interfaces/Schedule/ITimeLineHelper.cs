using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface ITimeLineHelper
    {
        List<AdPeriod> GetScheduleTimeLine(ScheduleDto schedule);
    }
}

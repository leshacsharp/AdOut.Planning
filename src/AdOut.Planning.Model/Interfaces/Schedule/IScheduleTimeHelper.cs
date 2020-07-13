using AdOut.Planning.Model.Classes;
using System;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleTimeHelper
    {
        TimeSpan GetTimeOfAdsShowing(AdScheduleTime timeInfo);
    }
}

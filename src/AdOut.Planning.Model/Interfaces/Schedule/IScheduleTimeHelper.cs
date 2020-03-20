using AdOut.Planning.Model.Database;
using System;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleTimeHelper
    {
        TimeSpan GetTimeOfAdsShowing(Plan plan, Database.Schedule schedule);
    }
}

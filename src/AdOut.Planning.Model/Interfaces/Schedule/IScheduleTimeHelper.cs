using AdOut.Planning.Model.Database;
using System;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleTimeHelper
    {
        //todo: need to think about arguments (simple arguments will be better)
        TimeSpan GetTimeOfAdsShowing(Plan plan, Database.Schedule schedule);
    }
}

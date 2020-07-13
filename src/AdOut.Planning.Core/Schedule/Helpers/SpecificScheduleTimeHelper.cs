using AdOut.Planning.Model.Classes;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class SpecificScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(AdScheduleTime timeInfo)
        {
            return timeInfo.ScheduleEndTime - timeInfo.ScheduleStartTime;
        } 
    }
}

using AdOut.Planning.Model.Dto;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class SpecificScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(AdScheduleTime scheduleInfo)
        {
            return scheduleInfo.ScheduleEndTime - scheduleInfo.ScheduleStartTime;
        } 
    }
}

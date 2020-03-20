using AdOut.Planning.Model.Database;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class SpecificScheduleTimeHelper : BaseScheduleTimeHelper
    {
        protected override TimeSpan GetTimeOfExecutingPlan(Plan plan, Model.Database.Schedule schedule)
        {
            return schedule.EndTime - schedule.StartTime;
        } 
    }
}

using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class ScheduleTimeHelperProvider : IScheduleTimeHelperProvider
    {
        public IScheduleTimeHelper CreateScheduleTimeHelper(PlanType planType)
        {
            return planType switch
            {
                PlanType.Daily => new DailyScheduleTimeHelper(),
                PlanType.Weekly => new WeeklyScheduleTimeHelper(),
                PlanType.Specific => new SpecificScheduleTimeHelper(),
                _ => throw new NotSupportedException($"PlanType={planType} is not supported")
            };
        }
    }
}

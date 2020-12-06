using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Schedule;
using System;

namespace AdOut.Planning.Core.Schedule.Helpers
{
    public class ScheduleTimeHelperProvider : IScheduleTimeHelperProvider
    {
        public IScheduleTimeHelper CreateScheduleTimeHelper(ScheduleType type)
        {
            return type switch
            {
                ScheduleType.Daily => new DailyScheduleTimeHelper(),
                ScheduleType.Weekly => new WeeklyScheduleTimeHelper(),
                ScheduleType.Specific => new SpecificScheduleTimeHelper(),
                _ => throw new NotSupportedException($"Schedule type={type} is not supported")
            };
        }
    }
}

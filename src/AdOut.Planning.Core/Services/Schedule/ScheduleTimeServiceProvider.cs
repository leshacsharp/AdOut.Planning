using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Services;
using System;

namespace AdOut.Planning.Core.Services.Schedule
{
    public class ScheduleTimeServiceProvider : IScheduleTimeServiceProvider
    {
        public IScheduleTimeService CreateScheduleTimeService(ScheduleType type)
        {
            return type switch
            {
                ScheduleType.Daily => new DailyScheduleTimeService(),
                ScheduleType.Weekly => new WeeklyScheduleTimeService(),
                ScheduleType.Specific => new SpecificScheduleTimeService(),
                _ => throw new NotSupportedException($"Schedule type={type} is not supported")
            };
        }
    }
}

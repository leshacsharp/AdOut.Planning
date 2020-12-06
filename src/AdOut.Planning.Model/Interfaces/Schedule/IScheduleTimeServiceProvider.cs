using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleTimeServiceProvider
    {
        IScheduleTimeService CreateScheduleTimeService(ScheduleType type);
    }
}

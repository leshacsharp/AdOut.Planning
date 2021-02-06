using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IScheduleTimeServiceProvider
    {
        IScheduleTimeService CreateScheduleTimeService(ScheduleType type);
    }
}

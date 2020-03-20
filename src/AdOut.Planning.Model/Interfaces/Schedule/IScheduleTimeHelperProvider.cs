using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleTimeHelperProvider
    {
        IScheduleTimeHelper CreateScheduleTimeHelper(PlanType planType);
    }
}

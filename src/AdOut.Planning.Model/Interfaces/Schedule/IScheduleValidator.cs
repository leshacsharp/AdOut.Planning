using AdOut.Planning.Model.Classes;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleValidator
    {
        void Valid(ScheduleValidationContext context);
        void SetNextValidator(IScheduleValidator validator);
    }
}

using AdOut.Planning.Model.Classes;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleValidator
    {
        void Validate(ScheduleValidationContext context);
        void SetNextValidator(IScheduleValidator validator);
    }
}

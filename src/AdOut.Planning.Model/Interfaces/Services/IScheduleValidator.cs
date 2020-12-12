using AdOut.Planning.Model.Classes;

namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IScheduleValidator
    {
        void Validate(ScheduleValidationContext context);
        void SetNextValidator(IScheduleValidator validator);
    }
}

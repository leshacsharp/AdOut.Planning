using AdOut.Planning.Model.Classes;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Schedule
{
    public interface IScheduleValidator
    {
        Task ValidAsync(ScheduleValidationContext context);
        void SetNextValidator(IScheduleValidator validator);
    }
}

using AdOut.Planning.Model.Api;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IScheduleManager : IBaseManager<Database.Schedule>
    {
        Task ValidateScheduleAsync(ScheduleValidationModel scheduleModel);
    }
}

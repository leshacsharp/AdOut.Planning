using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IScheduleManager : IBaseManager<Database.Schedule>
    {
        Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleModel scheduleModel);
        Task<double> CalculateSchedulePriceAsync(string planId, ScheduleModel schedule);
        Task CreateAsync(ScheduleModel createModel);
        Task UpdateAsync(UpdateScheduleModel updateModel);
    }
}

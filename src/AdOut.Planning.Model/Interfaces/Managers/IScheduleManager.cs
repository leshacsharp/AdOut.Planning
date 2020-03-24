using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IScheduleManager : IBaseManager<Database.Schedule>
    {
        Task<ValidationResult<string>> ValidateScheduleWithTempPlanAsync(ScheduleWithPlanValidationModel scheduleModel);
        Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleValidationModel scheduleModel);
        Task<List<AdPeriod>> GetAdPointTimeLine(int adPointId);
        Task CreateAsync(CreateScheduleModel createModel);
        Task UpdateAsync(UpdateScheduleModel updateModel);
    }
}

using AdOut.Planning.Model.Api;
using AdOut.Planning.Model.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IScheduleManager : IBaseManager<Database.Schedule>
    {
        Task<ValidationResult<string>> ValidateScheduleWithTempPlanAsync(TempScheduleValidationModel scheduleModel);
        Task<ValidationResult<string>> ValidateScheduleAsync(ScheduleModel scheduleModel);
        Task<List<PlanTimeLine>> GetPlansTimeLines(int adPointId, DateTime dateFrom, DateTime dateTo);
        Task CreateAsync(ScheduleModel createModel);
        Task UpdateAsync(UpdateScheduleModel updateModel);
    }
}

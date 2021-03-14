using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IPlanTimeRepository : IBaseRepository<PlanTime>
    {
        Task<List<StreamPlanTime>> GetStreamPlanTimesAsync(string adPointId, DateTime scheduleDate);
        Task<List<PlanPeriod>> GetPlanPeriodsAsync(string adPointId, DateTime scheduleStart, DateTime scheduleEnd);
    }
}

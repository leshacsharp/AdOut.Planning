using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanTimeManager
    {
        Task<List<StreamPlanTime>> GetTodaysStreamPlanTimesAsync(string adPointId);
        Task<List<PlanPeriod>> GetPlanPeriods(string adPointId, DateTime dateFrom, DateTime dateTo);
    }
}

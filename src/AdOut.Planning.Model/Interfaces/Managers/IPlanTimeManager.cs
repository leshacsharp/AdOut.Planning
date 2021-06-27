using AdOut.Planning.Model.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Managers
{
    public interface IPlanTimeManager
    {
        Task<List<StreamPlanTime>> GetStreamPlansTimeAsync(string adPointId, DateTime date);
        Task<List<PlanPeriod>> GetPlanPeriods(string adPointId, DateTime dateFrom, DateTime dateTo);
    }
}

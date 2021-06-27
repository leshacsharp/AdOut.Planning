using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Managers;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Managers
{
    public class PlanTimeManager : IPlanTimeManager
    {
        private readonly IPlanTimeRepository _planTimeRepository;
        public PlanTimeManager(IPlanTimeRepository planTimeRepository)
        {
            _planTimeRepository = planTimeRepository;
        }

        public Task<List<StreamPlanTime>> GetStreamPlansTimeAsync(string adPointId, DateTime date)
        {
            return _planTimeRepository.GetStreamPlansTimeAsync(adPointId, date);
        }

        public Task<List<PlanPeriod>> GetPlanPeriods(string adPointId, DateTime dateFrom, DateTime dateTo)
        {
            return _planTimeRepository.GetPlanPeriodsAsync(adPointId, dateFrom, dateTo);     
        }
    }
}

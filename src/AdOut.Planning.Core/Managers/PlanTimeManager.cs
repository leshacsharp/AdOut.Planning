using AdOut.Planning.Model.Database;
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

        public Task<List<PlanTime>> GetTodaysPlanTimesAsync(string adPointId)
        {
            return _planTimeRepository.GetPlanTimes(adPointId, DateTime.Now.Date);
        }
    }
}

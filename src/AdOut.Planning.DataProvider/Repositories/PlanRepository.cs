using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Dto;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanRepository : BaseRepository<Plan>, IPlanRepository
    {
        public PlanRepository(IDatabaseContext context) 
            : base(context)
        {
        }

        public Task<List<AdPointPlanDto>> GetByAdPoint(int adPointId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Plan> GetByIdAsync(int planId)
        {
            var query = from p in Context.Plans
                        where p.Id == planId
                        select p;

            return query.SingleOrDefaultAsync();
        }

        public Task<List<int>> GetAdPointsIds(int plaId)
        {
            var query = from pap in Context.PlanAdPoints
                        where pap.PlanId == plaId
                        select pap.AdPointId;

            return query.ToListAsync();
        }
    }
}

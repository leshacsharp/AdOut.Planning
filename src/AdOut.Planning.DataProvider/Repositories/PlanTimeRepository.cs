using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqKit;
using System.Data.Entity;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanTimeRepository : IPlanTimeRepository
    {
        private readonly ITimeLineContext _db;
        public PlanTimeRepository(ITimeLineContext db)
        {
            _db = db;
        }

        public void Create(PlanTime entity)
        {
            _db.Plans.InsertOne(entity);
        }

        public void Update(PlanTime entity)
        {
            _db.Plans.ReplaceOne(p => p.Id == entity.Id, entity);
        }

        public void Delete(PlanTime entity)
        {
            _db.Plans.DeleteOne(p => p.Id == entity.Id);
        }

        public async ValueTask<PlanTime> GetByIdAsync(params object[] id)
        {
            var entityId = id.FirstOrDefault()?.ToString();
            return await _db.Plans.AsQueryable().SingleOrDefaultAsync(p => p.Id == entityId);
        }

        public IQueryable<PlanTime> Read(Expression<Func<PlanTime, bool>> predicate)
        {
            return _db.Plans.AsQueryable().Where(predicate);
        }

        public Task<List<PlanTime>> GetPlanTimes(string adPointId, DateTime scheduleDate)
        {
            var query = _db.Plans.AsQueryable()
                                 .AsExpandable()
                                 .Where(p => p.AdPoints.Any(id => id == adPointId) &&
                                             scheduleDate < p.EndDateTime &&
                                             p.StartDateTime < scheduleDate)
                                 .Select(p => new PlanTime()
                                 {
                                     Id = p.Id,
                                     Title = p.Title,
                                     StartDateTime = p.StartDateTime,
                                     EndDateTime = p.EndDateTime,
                                     Creator = p.Creator,
                                     AdPoints = p.AdPoints,
                                     Ads = p.Ads,
                                     Schedules = p.Schedules.Where(sp => sp.Dates.Contains(scheduleDate))
                                 });

            return query.ToListAsync();
        }
    }
}

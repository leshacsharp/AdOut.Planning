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
using AdOut.Planning.Model.Dto;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class PlanTimeRepository : IPlanTimeRepository
    {
        private readonly ITimeLineContext _db;
        public PlanTimeRepository(ITimeLineContext db)
        {
            _db = db;
        }

        public Task<List<StreamPlanTime>> GetStreamPlansTimeAsync(string adPointId, DateTime scheduleDate)
        {
            var query = _db.Plans.AsQueryable()
                                 .AsExpandable()
                                 .Where(p => p.AdPoints.Any(id => id == adPointId) &&
                                             scheduleDate <= p.EndDateTime &&
                                             scheduleDate >= p.StartDateTime)
                                 .Select(p => new StreamPlanTime()
                                 {
                                     Id = p.Id,
                                     Title = p.Title,
                                     Ads = p.Ads,
                                     Schedules = p.Schedules
                                 });

            return query.ToListAsync();
        }

        //probably will be used in generating an AdPoint's board of schedules.
        public Task<List<PlanPeriod>> GetPlanPeriodsAsync(string adPointId, DateTime scheduleStart, DateTime scheduleEnd)
        {
            var query = _db.Plans.AsQueryable()
                                 .AsExpandable()
                                 .Where(p => p.AdPoints.Any(id => id == adPointId) &&
                                             scheduleStart < p.EndDateTime &&
                                             p.StartDateTime < scheduleEnd)
                                 .Select(p => new PlanPeriod()
                                 {
                                     PlanId = p.Id,
                                     //this condition won't work properly, mongodb stores dates in UTC, but the condition compares local datetimes 
                                     //[BsonDateTimeOptions(Kind = DateTimeKind.Local)] - mongodb; store dates in local time
                                     SchedulePeriods = p.Schedules.Where(sp => sp.Dates.Any(d => d >= scheduleStart && d <= scheduleEnd))
                                 });

            return query.ToListAsync();
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
    }
}

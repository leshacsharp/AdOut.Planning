﻿using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AdOut.Planning.DataProvider.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(IDatabaseContext context)
            : base(context)
        {
        }

        public Task<Schedule> GetByIdAsync(int scheduleId)
        {
            var query = from p in Context.Schedules
                        where p.Id == scheduleId
                        select p;

            return query.SingleOrDefaultAsync();
        }
    }
}

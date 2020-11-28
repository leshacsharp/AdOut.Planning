using AdOut.Planning.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IScheduleRepository : IBaseRepository<Database.Schedule>
    {
        Task<List<ScheduleDto>> GetByPlanAsync(string planId);
        Task<ScheduleTime> GetScheduleTimeAsync(string scheduleId);
    }
}

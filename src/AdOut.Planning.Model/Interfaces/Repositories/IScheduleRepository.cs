using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IScheduleRepository : IBaseRepository<Database.Schedule>
    {
        Task<List<ScheduleDto>> GetByPlanAsync(int planId);
        Task<Database.Schedule> GetByIdAsync(int scheduleId);
        Task<AdScheduleTime> GetScheduleInfo(int scheduleId);
    }
}

using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Dto;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IScheduleRepository : IBaseRepository<Database.Schedule>
    {
        Task<ScheduleTime> GetScheduleTimeAsync(string scheduleId);
    }
}

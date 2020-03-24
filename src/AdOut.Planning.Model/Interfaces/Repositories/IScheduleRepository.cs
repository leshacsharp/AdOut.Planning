using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Repositories
{
    public interface IScheduleRepository : IBaseRepository<Database.Schedule>
    {
        Task<Database.Schedule> GetByIdAsync(int scheduleId);   
    }
}

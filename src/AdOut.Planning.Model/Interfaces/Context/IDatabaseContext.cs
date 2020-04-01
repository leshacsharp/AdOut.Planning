using AdOut.Planning.Model.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Context
{
    public interface IDatabaseContext
    {
        DbSet<Ad> Ads { get; set; }

        DbSet<Plan> Plans { get; set; }

        DbSet<Database.Schedule> Schedules { get; set; }

        DbSet<AdPoint> AdPoints { get; set; }

        DbSet<PlanAd> PlanAds { get; set; }  

        DbSet<PlanAdPoint> PlanAdPoints { get; set; }

        DbSet<DayOff> DaysOff { get; set; }

        DbSet<AdPointDayOff> AdPointDaysOff { get; set; }

        DbSet<Configuration> Configurations { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

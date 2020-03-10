using AdOut.Planning.Model.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Context
{
    public interface IDatabaseContext
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Database.Schedule> Schedules { get; set; }
        public DbSet<AdPoint> AdPoints { get; set; }
        public DbSet<PlanAd> PlanAds { get; set; }  
        public DbSet<PlanAdPoint> PlanAdPoints { get; set; }
        public DbSet<Weekend> Weekends { get; set; }
        public DbSet<AdPointWeekend> AdPointWeekends { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

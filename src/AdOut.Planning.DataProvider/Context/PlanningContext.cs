using AdOut.Planning.Model.Database;
using AdOut.Planning.Model.Interfaces.Context;
using Microsoft.EntityFrameworkCore;

namespace AdOut.Planning.DataProvider.Context
{
    public class PlanningContext : DbContext, IDatabaseContext
    {
        public PlanningContext(DbContextOptions<PlanningContext> dbContextOptions)
            : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<AdPoint> AdPoints { get; set; }
        public DbSet<PlanAd> PlanAds { get; set; }
        public DbSet<PlanAdPoint> PlanAdPoints { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<DayOff> DaysOff { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlanAd>().HasKey(pa => new { pa.PlanId, pa.AdId });
            modelBuilder.Entity<PlanAdPoint>().HasKey(pap => new { pap.PlanId, pap.AdPointId });
        }
    }
}

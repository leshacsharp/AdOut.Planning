using AdOut.Planning.Model.Database;
using Microsoft.EntityFrameworkCore;

namespace AdOut.Planning.Model.Interfaces.Context
{
    public interface IDatabaseContext
    {
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanAd> PlanAds { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}

using AA.CommoditiesDashboard.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace AA.CommoditiesDashboard.Api.Infrastructure
{
    public class CommoditiesDashboardContext: DbContext
    {
        public CommoditiesDashboardContext(DbContextOptions<CommoditiesDashboardContext> options) : base(options)
        {
        }
        public DbSet<CommodityData> CommodityData { get; set; }
        public DbSet<Commodity> Commodity { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CommodityEntityTypeConfiguration());
            builder.ApplyConfiguration(new CommodityDataEntityTypeConfiguration());
        }
    }
}
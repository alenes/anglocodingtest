using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AA.CommoditiesDashboard.Api.Infrastructure;
using AA.CommoditiesDashboard.Api.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AA.CommoditiesDashboard.Api.UnitTests
{
    public class CommodityFixture : IAsyncLifetime
    {
        public DbContextOptions<CommoditiesDashboardContext> DbOptions { get; set; }
        public (DateTime start, DateTime end) Period { get; set; }
        public List<CommodityData> TestCommodityData { get; set; }
        public List<Commodity> FakeCommodityList { get; set; }
        
        
        public async Task InitializeAsync()
        {
            DbOptions = new DbContextOptionsBuilder<CommoditiesDashboardContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            var testFactory = new TestFactory();
            Period = (DateTime.UtcNow.AddDays(-10), DateTime.UtcNow);

            await using var dbContext = new CommoditiesDashboardContext(DbOptions);
            FakeCommodityList = testFactory.GetFakeCommodityList();
            dbContext.AddRange(FakeCommodityList);
            TestCommodityData = testFactory.GetFakeCommodityDataList(Period.start, Period.end);
            dbContext.AddRange(TestCommodityData);
            await dbContext.SaveChangesAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
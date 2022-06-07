using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AA.CommoditiesDashboard.Api.Infrastructure;
using AA.CommoditiesDashboard.Api.Model;
using AA.CommoditiesDashboard.Api.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AA.CommoditiesDashboard.Api.UnitTests
{
    public class CommodityRepositoryTests : IClassFixture<CommodityFixture>
    {
        private readonly DbContextOptions<CommoditiesDashboardContext> _dbOptions;
        private readonly (DateTime start, DateTime end) _period;
        private readonly List<CommodityData> _testCommodityData;
        private readonly List<Commodity> _fakeCommodityList;

        public CommodityRepositoryTests(CommodityFixture commodityFixture)
        {
            _period = commodityFixture.Period;
            _dbOptions = commodityFixture.DbOptions;
            _fakeCommodityList = commodityFixture.FakeCommodityList;
            _testCommodityData = commodityFixture.TestCommodityData;
        }
        
        [Fact]
        public async Task ShouldReturnCommodityResults_GivenStartAndEndDates()
        {
            //Arrange
            var sut = new CommodityRepository(new CommoditiesDashboardContext(_dbOptions));

            //Act
            var result = await sut.GetCommodityResults(_period.start, _period.end);

            //Assert
            result.Should().HaveCount(2);
        }
        
        [Fact]
        public async Task ShouldReturnEmptyList_GivenStartAndEndDates_WhenNoRecordsExistForThePeriod()
        {
            //Arrange
            var sut = new CommodityRepository(new CommoditiesDashboardContext(_dbOptions));

            //Act
            var result = await sut.GetCommodityResults(_period.end.AddDays(1), _period.end.AddDays(1));

            //Assert
            result.Should().BeEmpty();
        }
        
        [Fact]
        public async Task ShouldReturnMetricsList_GivenAsOfDate()
        {
            //Arrange
            var sut = new CommodityRepository(new CommoditiesDashboardContext(_dbOptions));

            //Act
            var result = (await sut.GetMetrics(_period.end)).ToList();

            //Assert
            result.Should().HaveCount(2);
            result.GroupBy(r => r.CommodityId).Should().HaveCount(2);
        }
        
        [Fact]
        public async Task ShouldReturnYearToDate_GivenAsOfDate()
        {
            //Arrange
            var sut = new CommodityRepository(new CommoditiesDashboardContext(_dbOptions));

            //Act
            var result = await sut.GetMetrics(_period.end);

            //Assert
            var testPnlYtd = _testCommodityData
                .Where(t => t.Date <= _period.end && t.Date.Year == _period.end.Year 
                                                  && t.CommodityId == _fakeCommodityList.Last().Id)
                .Sum(t => t.PnlDaily);
            
            result.Last().PnlYTD.Should().Be(testPnlYtd);
        }
        
        [Fact]
        public async Task ShouldReturnCommodities_GivenAsOfDate_WhenNoResultsOnSelectedData()
        {
            //Arrange
            var sut = new CommodityRepository(new CommoditiesDashboardContext(_dbOptions));

            //Act
            var result = (await sut.GetMetrics(_period.end)).ToList();

            //Assert
            var testPnlYtd = _testCommodityData
                .Where(t => t.Date <= _period.end && t.Date.Year == _period.end.Year 
                                                  && t.CommodityId == _fakeCommodityList.First().Id)
                .Sum(t => t.PnlDaily);
            
            result.First().PnlYTD.Should().Be(testPnlYtd);
            result.First().Position.Should().Be(0);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AA.CommoditiesDashboard.Api.Controllers;
using AA.CommoditiesDashboard.Api.Exceptions;
using AA.CommoditiesDashboard.Api.Model;
using AA.CommoditiesDashboard.Api.Repositories;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AA.CommoditiesDashboard.Api.UnitTests
{
    public class CommoditiesControllerTests
    {
        private readonly CommoditiesController _sut;
        private readonly Mock<ICommodityRepository> _commodityRepository;
        private readonly TestFactory _testFactory;
        private readonly Fixture _fixture;
        
        public CommoditiesControllerTests()
        {
            _testFactory = new TestFactory();
            _fixture = new Fixture();
            _commodityRepository = new Mock<ICommodityRepository>();
            _sut = new CommoditiesController(_commodityRepository.Object);
        }

        [Fact]
        public async Task ShouldReturnCommodityResults_GivenStartAndEndDates()
        {
            //Arrange
            var period = (start:DateTime.UtcNow.AddDays(-10), end :DateTime.UtcNow);
            var results = _testFactory.GetFakeCommodityDataList(period.start, period.end);
            _commodityRepository.Setup(r => r.GetCommodityResults(period.start, period.end))
                .ReturnsAsync(results);

            //Act
            var result = await _sut.GetTimeSeriesData(period.start, period.end);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var resultGroups = (okResult!.Value as IEnumerable<CommodityResultData>)?.ToList();
            resultGroups.Should().NotBeNull();
            resultGroups!.Should().HaveCount(2);
            resultGroups!.First().CommodityResults.Should().HaveCount(2);
            resultGroups!.First().Commodity.Should().Be(results.First().Commodity.Name);
            resultGroups!.First().CommodityResults.First().PnlDaily.Should().Be(results.First().PnlDaily);
        }
        
        [Fact]
        public async Task ShouldThrowValidationException_GivenNullStartDate_WhenRequestedCommodityResults()
        {
            //Arrange
            var period = (start: DateTime.UtcNow, end :DateTime.UtcNow);
            var results = _testFactory.GetFakeCommodityDataList(period.start, period.end);
            _commodityRepository.Setup(r => r.GetCommodityResults(DateTime.MinValue, period.end))
                .ReturnsAsync(results);

            //Act
            Func<Task<ActionResult<IEnumerable<CommodityResultData>>>> act =  async () => await _sut.GetTimeSeriesData(null, period.end);

            //Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Please provide start date as a query string parameter");
        }

        [Fact]
        public async Task ShouldThrowValidationException_GivenNullEndDate_WhenRequestedCommodityResults()
        {
            //Arrange
            var period = (start: DateTime.UtcNow, end :DateTime.UtcNow);
            var results = _testFactory.GetFakeCommodityDataList(period.start, period.end);
            _commodityRepository.Setup(r => r.GetCommodityResults(period.start, DateTime.MaxValue))
                .ReturnsAsync(results);

            //Act
            Func<Task<ActionResult<IEnumerable<CommodityResultData>>>> act =  async () => await _sut.GetTimeSeriesData(period.start, null);

            //Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Please provide end date as a query string parameter");
        }
        
        [Fact]
        public async Task ShouldReturnCommodityMetrics_GivenStartAndEndDates()
        {
            //Arrange
            var asOfDate = DateTime.UtcNow.AddDays(-10);
            var metrics = _fixture.CreateMany<CommodityMetrics>(5).ToList();
            _commodityRepository.Setup(r => r.GetMetrics(asOfDate))
                .ReturnsAsync(metrics);

            //Act
            var result = await _sut.GetMetrics(asOfDate);

            //Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var metricsResult = (okResult!.Value as IEnumerable<CommodityMetrics>)?.ToList();
            metricsResult.Should().HaveCount(metrics.Count);
            _commodityRepository.Verify(r => r.GetMetrics(asOfDate), Times.Once);
        }
        
        [Fact]
        public async Task ShouldThrowValidationException_GivenNullAsOfDate_WhenRequestedCommodityMetrics()
        {
            //Arrange
            var asOfDate = DateTime.UtcNow.AddDays(-10);
            var metrics = _fixture.CreateMany<CommodityMetrics>(5).ToList();
            _commodityRepository.Setup(r => r.GetMetrics(asOfDate))
                .ReturnsAsync(metrics);

            //Act
            Func<Task<ActionResult<IEnumerable<CommodityMetrics>>>> act =  async () => await _sut.GetMetrics(null);

            //Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("Please provide date in the format YYYY-MM-DD as a part of the url, metrics/YYYY-MM-DD");
        }
    }
}
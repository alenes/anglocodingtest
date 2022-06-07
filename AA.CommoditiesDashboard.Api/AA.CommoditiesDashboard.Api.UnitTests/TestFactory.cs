using System;
using System.Collections.Generic;
using System.Linq;
using AA.CommoditiesDashboard.Api.Model;
using AutoFixture;

namespace AA.CommoditiesDashboard.Api.UnitTests
{
    public class TestFactory
    {
        private readonly Fixture _fixture;
        private readonly List<Commodity> _commodities;

        public TestFactory()
        {
            _fixture = new Fixture();
            _commodities = _fixture.CreateMany<Commodity>(2).ToList();
        }
        
        public List<CommodityData>  GetFakeCommodityDataList(DateTime startDate, DateTime endDate)
        {
            var commodities0 = _fixture.Build<CommodityData>()
                .With(d => d.Date, startDate.AddYears(-1))
                .With(d => d.Commodity, _commodities.First())
                .With(d => d.CommodityId, _commodities.First().Id)
                .Create();
            var commodities1 = _fixture.Build<CommodityData>()
                .With(d => d.Date, startDate)
                .With(d => d.Commodity, _commodities.First())
                .With(d => d.CommodityId, _commodities.First().Id)
                .Create();
            var commodities2 = _fixture.Build<CommodityData>()
                .With(d => d.Date, endDate)
                .With(d => d.Commodity, _commodities.Last())
                .With(d => d.CommodityId, _commodities.Last().Id)
                .Create();
            var nextEndDate = endDate.AddDays(10);
            var commodities3 = _fixture.Build<CommodityData>()
                .With(d => d.Date, nextEndDate)
                .With(d => d.Commodity, _commodities.Last)
                .With(d => d.CommodityId, _commodities.Last().Id)
                .Create();
            var result = new List<CommodityData>();
            result.Add(commodities0);
            result.Add(commodities1);
            result.Add(commodities2);
            result.Add(commodities3);
            return result;
        }

        public List<Commodity> GetFakeCommodityList()
        {
            return _commodities;
        }
    }
}
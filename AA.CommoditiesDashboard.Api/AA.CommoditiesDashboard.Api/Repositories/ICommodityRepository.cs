using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AA.CommoditiesDashboard.Api.Infrastructure;
using AA.CommoditiesDashboard.Api.Model;

namespace AA.CommoditiesDashboard.Api.Repositories
{
    public interface ICommodityRepository
    {
        Task<List<CommodityData>> GetCommodityResults(DateTime startDate, DateTime endDate);

        Task<IEnumerable<CommodityMetrics>> GetMetrics(DateTime metricsDate);
    }
}
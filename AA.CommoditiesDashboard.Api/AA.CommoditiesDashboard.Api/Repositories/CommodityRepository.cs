using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AA.CommoditiesDashboard.Api.Infrastructure;
using AA.CommoditiesDashboard.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace AA.CommoditiesDashboard.Api.Repositories
{
    public class CommodityRepository: ICommodityRepository
    {
        private readonly CommoditiesDashboardContext _commoditiesDashboardContext;

        public CommodityRepository(CommoditiesDashboardContext commoditiesDashboardContext)
        {
            _commoditiesDashboardContext = commoditiesDashboardContext;
        }
 
        public async Task<List<CommodityData>> GetCommodityResults(DateTime startDate, DateTime endDate) =>
            await _commoditiesDashboardContext.CommodityData.Where(d => d.Date >= startDate && d.Date <= endDate)
                .Include(d => d.Commodity)
                .ToListAsync();

        public async Task<IEnumerable<CommodityMetrics>> GetMetrics(DateTime metricsDate)
        {
            var query = _commoditiesDashboardContext.Commodity
                .GroupJoin(_commoditiesDashboardContext.CommodityData.Where(d => d.Date == metricsDate), commodity => commodity.Id,
                    cData => cData.Commodity.Id, (commodity, gj) => new { commodity, gj })
                .SelectMany(@t => @t.gj.DefaultIfEmpty(),
                    (@t, results) => new CommodityMetrics
                    {
                        CommodityId = @t.commodity.Id,
                        Commodity = @t.commodity.Name,
                        Position = results != null ? results.Position : 0,
                        Price = results != null ? results.Price : 0,
                        PnlDaily = results != null ? results.PnlDaily : 0
                    })
                .Join(
                    _commoditiesDashboardContext.CommodityData
                        .Where(r => r.Date <= metricsDate && r.Date.Year == metricsDate.Year)
                        .GroupBy(r => r.CommodityId)
                        .Select(gr => new { Id = gr.Key, PnLYTD = gr.Sum(r => r.PnlDaily)}),
                    c => c.CommodityId,
                    s => s.Id,
                    (c, s) => new CommodityMetrics
                    {
                        CommodityId = c.CommodityId,
                        Commodity = c.Commodity,
                        Position = c.Position,
                        Price = c.Price,
                        PnlDaily = c.PnlDaily,
                        PnlYTD = s.PnLYTD
                    }
                 ); 


            return (await query.ToListAsync());
        }
    }
}
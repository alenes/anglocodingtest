using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AA.CommoditiesDashboard.Api.Exceptions;
using AA.CommoditiesDashboard.Api.Model;
using AA.CommoditiesDashboard.Api.Repositories;

namespace AA.CommoditiesDashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommoditiesController
    {
        private readonly ICommodityRepository _repository;

        public CommoditiesController(ICommodityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommodityResultData>>> GetTimeSeriesData(
            [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var results = (await _repository.GetCommodityResults(
                        startDate ?? throw new ValidationException("Please provide start date as a query string parameter"),
                        endDate ?? throw new ValidationException("Please provide end date as a query string parameter")))
                    .GroupBy(d => d.Commodity.Id)
                    .Select(gr => new CommodityResultData
                    {
                        CommodityId = gr.Key, Commodity = gr.First().Commodity.Name, CommodityResults = gr.Select(r =>
                            new CommodityResult
                            {
                                Date = r.Date,
                                Contract = r.Contract,
                                Position = r.Position,
                                Price = r.Price,
                                NewTradeAction = r.NewTradeAction,
                                PnlDaily = r.PnlDaily
                            })
                    });

            return new OkObjectResult(results);
        }

        [HttpGet("metrics/{asOfDate:datetime?}")] 
        public async Task<ActionResult<IEnumerable<CommodityMetrics>>> GetMetrics(DateTime? asOfDate)
        {
            var metrics = await _repository.GetMetrics(asOfDate ?? throw new ValidationException("Please provide date in the format YYYY-MM-DD as a part of the url, metrics/YYYY-MM-DD"));
            return new OkObjectResult(metrics);
        }
    }
}
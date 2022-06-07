using System.Collections.Generic;
using AA.CommoditiesDashboard.Api.Infrastructure;

namespace AA.CommoditiesDashboard.Api.Model
{
    public class CommodityResultData
    {
        public string Commodity { get; set; }
        public int CommodityId { get; set; }
        public IEnumerable<CommodityResult> CommodityResults { get; set; }
    }
}
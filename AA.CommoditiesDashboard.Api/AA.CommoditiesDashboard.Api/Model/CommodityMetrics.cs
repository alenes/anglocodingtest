using System;

namespace AA.CommoditiesDashboard.Api.Model
{
    public class CommodityMetrics
    {
        public int CommodityId { get; set; }
        public string Commodity { get; set; }

        public decimal Price { get; set; }

        public int Position { get; set; }

        public decimal PnlDaily { get; set; }
        
        public decimal PnlYTD { get; set; }
    }
}
using System;

namespace AA.CommoditiesDashboard.Api.Model
{
    public class CommodityData
    {
        public int Id { get; set; }
        
        public int CommodityId { get; set; }
        
        public Commodity Commodity { get; set; }

        public DateTime Date { get; set; }

        public string Contract { get; set; }

        public decimal Price { get; set; }

        public int Position { get; set; }

        public int NewTradeAction { get; set; }

        public decimal PnlDaily { get; set; }
    }
}
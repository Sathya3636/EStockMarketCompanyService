using System;

namespace EStockMarketCompanyService.Domain.Entities
{
    public class Stock
    {
        public string CompanyCode { get; set; }

        public double StockPrice { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}

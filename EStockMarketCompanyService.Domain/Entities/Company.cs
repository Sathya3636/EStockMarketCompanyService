using Newtonsoft.Json;

namespace EStockMarketCompanyService.Domain.Entities
{
    public class Company
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string CeoName { get; set; }

        public double TurnOver { get; set; }

        public string Website { get; set; }

        public string StockExchange { get; set; }
    }
}

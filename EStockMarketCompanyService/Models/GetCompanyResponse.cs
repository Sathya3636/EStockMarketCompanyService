using Newtonsoft.Json;
using System.Collections.Generic;

namespace EStockMarketCompanyService.Models
{
    public class GetCompanyResponse
    {
        public List<Company> Companies { get; set; }
    }

    public class Company
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ceoName")]
        public string CeoName { get; set; }

        [JsonProperty("turnOver")]
        public double TurnOver { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("stockExchange")]
        public string StockExchange { get; set; }

        [JsonProperty("latestStockPrice")]
        public double? LatestStockPrice { get; set; }
    }
}

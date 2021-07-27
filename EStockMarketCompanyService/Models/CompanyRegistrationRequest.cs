using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EStockMarketCompanyService.Models
{
    public class CompanyRegistrationRequest
    {
        [JsonProperty("code")]
        [Required]
        public string Code { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("ceoName")]
        public string CeoName { get; set; }

        [Required]
        [JsonProperty("turnOver")]
        [Range(100000000, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public double TurnOver { get; set; }

        [Required]
        [JsonProperty("website")]
        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", ErrorMessage = "Invalid website")]
        public string Website { get; set; }

        [Required]
        [JsonProperty("stockExchange")]
        public string StockExchange { get; set; }
    }
}

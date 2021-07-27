using EStockMarketCompanyService.Domain.Entities;
using EStockMarketCompanyService.Domain.Interfaces;
using EStockMarketCompanyService.Message.Send.Interfaces;
using EStockMarketCompanyService.Models;
using EStockMarketCompanyService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EStockMarketCompanyService.Services
{
    public class CompanyService : ICompanyService
    {
        public readonly ICompanyRepository _companyRepository;
        public readonly IDeleteStockRabbitMqService _rabbitMqService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public CompanyService(ICompanyRepository companyRepository, IHttpClientFactory clientFactory, IConfiguration configuration, IDeleteStockRabbitMqService rabbitMqService)
        {
            _companyRepository = companyRepository;
            _rabbitMqService = rabbitMqService;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task CreateCompany(CompanyRegistrationRequest request)
        {
            var company = new Domain.Entities.Company
            {
                Id = request.Code,
                Code = request.Code,
                Name = request.Name,
                CeoName = request.CeoName,
                TurnOver = request.TurnOver,
                StockExchange = request.StockExchange,
                Website = request.Website
            };

            await _companyRepository.AddCompanyAsync(company);
        }

        public async Task DeleteCompany(string companyCode)
        {
            await _companyRepository.DeleteCompanyAsync(companyCode);
            _rabbitMqService.SendDeleteCompany(companyCode);
        }

        public async Task<GetCompanyResponse> GetCompanies()
        {
            var companiesResponse = new GetCompanyResponse
            {
                Companies = new List<Models.Company>()
            };

            var companies = await _companyRepository.GetCompanies();
            if (companies?.Any() == true)
            {
                var companyIds = companies.Select(x => x.Code).ToList();
                dynamic obj = new ExpandoObject();
                obj.CompanyCodes = companyIds;

                var json = JsonConvert.SerializeObject(obj);
                var url = _configuration.GetValue<string>("StockServiceBaseUrl");
                url += "get/latestprice";
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                List<Stock> stocks = null;

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    stocks = JsonConvert.DeserializeObject<List<Stock>>(responseStream);
                }

                foreach (var com in companies)
                {
                    var stock = stocks?.FirstOrDefault(x => x.CompanyCode == com.Code);
                    companiesResponse.Companies.Add(new Models.Company
                    {
                        Code = com.Code,
                        Name = com.Name,
                        CeoName = com.CeoName,
                        StockExchange = com.StockExchange,
                        TurnOver = com.TurnOver,
                        Website = com.Website,
                        LatestStockPrice = stock?.StockPrice
                    });
                }
            }

            return companiesResponse;
        }

        public async Task<GetCompanyResponse> GetCompanybyCode(string companyCode)
        {
            var companiesResponse = new GetCompanyResponse
            {
                Companies = new List<Models.Company>()
            };

            var company = await _companyRepository.GetCompanybyCodeAsync(companyCode);
            if (company != null)
            {
                var companyIds = new List<string> { company.Code };
                dynamic obj = new ExpandoObject();
                obj.CompanyCodes = companyIds;

                var json = JsonConvert.SerializeObject(obj);
                var url = _configuration.GetValue<string>("StockServiceBaseUrl");
                url += "get/latestprice";
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                List<Stock> stocks = null;

                if (response.IsSuccessStatusCode)
                {
                    var responseStream = await response.Content.ReadAsStringAsync();
                    stocks = JsonConvert.DeserializeObject<List<Stock>>(responseStream);
                }

                companiesResponse.Companies.Add(new Models.Company
                {
                    Code = company.Code,
                    Name = company.Name,
                    CeoName = company.CeoName,
                    StockExchange = company.StockExchange,
                    TurnOver = company.TurnOver,
                    Website = company.Website,
                    LatestStockPrice = stocks?.FirstOrDefault()?.StockPrice
                });
            }

            return companiesResponse;
        }
    }
}

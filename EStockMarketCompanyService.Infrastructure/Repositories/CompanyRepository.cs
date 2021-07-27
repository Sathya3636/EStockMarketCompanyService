using EStockMarketCompanyService.Domain.Entities;
using EStockMarketCompanyService.Domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStockMarketCompanyService.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly Container _container;
        private readonly IConfiguration _configuration;

        [Obsolete]
        public CompanyRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var configurationSection = _configuration.GetSection("CosmosCompanyDb");
            string databaseName = configurationSection.GetSection("DatabaseName").Value;
            string containerName = configurationSection.GetSection("ContainerName").Value;
            string account = configurationSection.GetSection("Account").Value;
            string key = configurationSection.GetSection("Key").Value;
            CosmosClient client = new CosmosClient(account, key);

            _ = InitializeCosmosClientInstanceAsync(client, databaseName, containerName);
            _container = client.GetContainer(databaseName, containerName);
        }

        private static async Task InitializeCosmosClientInstanceAsync(CosmosClient client, string databaseName, string containerName)
        {
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/Code");
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var queryString = @"select * from c";
            var query = this._container.GetItemQueryIterator<Company>(new QueryDefinition(queryString));
            List<Company> results = new List<Company>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task<Company> GetCompanybyCodeAsync(string companyCode)
        {
            try
            {
                ItemResponse<Company> response = await this._container.ReadItemAsync<Company>(companyCode, new PartitionKey(companyCode));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task AddCompanyAsync(Company company)
        {
            await this._container.CreateItemAsync<Company>(company, new PartitionKey(company.Code));
        }

        public async Task DeleteCompanyAsync(string companyCode)
        {
            await this._container.DeleteItemAsync<Company>(companyCode, new PartitionKey(companyCode));
        }
    }
}

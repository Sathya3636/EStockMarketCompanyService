using EStockMarketCompanyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EStockMarketCompanyService.Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompanies();

        Task<Company> GetCompanybyCodeAsync(string companyCode);

        Task AddCompanyAsync(Company company);

        Task DeleteCompanyAsync(string companyCode);
    }
}
